using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Globalization;
using Lucene.Net.Analysis;
using SearchEngine.Analysis.Thesaurus;
using SearchEngine.Analysis.Rule;
using SearchEngine.Analysis.Match;
using SearchEngine.Analysis.Coder;
using System.Text.RegularExpressions;

namespace SearchEngine.Analysis
{
	/// <summary>
    /// ThesaurusSpliter �ִʴ�����
	/// </summary>
    public class ThesaurusSpliter
    {
        private TextReader input;

        private AssociateStream assoStream;

        //��ǰ���������λ��
        private int offset = 0;

        //��ǰ����Ļ�����λ��
        private int bufferIndex = 0;

        //��ǰ�����������ݳ���
        private int dataLen = 0;

        //���ʳ����������������ǿ������ô���
        private static int MAX_WORD_LEN = 24;

        //���峤��
        private static int IO_BUFFER_SIZE = 1024;

        //��ǰ�ʵĳ���
        private int length = 0;

        //Ԥ�����ݳ���
        private int prepLength = 0;

        //��ǰ������
        private char[] buffer = new char[MAX_WORD_LEN];

        //Ԥ��ȡ����
        private char[] prepBuffer = new char[MAX_WORD_LEN];

        //Ԥ��������
        private char[] ioBuffer = new char[IO_BUFFER_SIZE];

        //��ǰһ�ζ�ȡ�������� MAX_WORD_LEN ���ֱ��ݣ��Ա����ݡ�
        private char[] backBuffer = new char[MAX_WORD_LEN];

        /// <summary>
        /// �ִʴ��������캯��
        /// </summary>
        /// <param name="input">�ı���ȡ��</param>
        public ThesaurusSpliter(TextReader input)
        {
            this.input = input;
            //����������.ģ�����뷨���������Ĺ���
            assoStream = new AssociateStream();
            ReadBuffer();
        }

        //��ȡ���е����ݵ�������
        private void ReadBuffer()
        {
            //���ݻ������е���� MAX_WORD_LEN ���ֱ��ݣ��Ա����ݡ�
            Array.Copy(ioBuffer, IO_BUFFER_SIZE - MAX_WORD_LEN, backBuffer, 0, MAX_WORD_LEN);
            dataLen = input.Read(ioBuffer, 0, ioBuffer.Length);
            CodeMap.Escape(ioBuffer, 0, dataLen);
            bufferIndex = 0;
        }

        //�ӻ�������ȡ��һ���ַ�
        private Char GetNextChar()
        {
            //ǰ���������λ��ǰ��һ���ַ�
            offset++;
            //�����ȡ���ַ���֮ǰ�Ļ��������ڱ��ݻ�������ȡ
            if (bufferIndex < 0)
            {
                return Char.ToLower(backBuffer[backBuffer.Length + (bufferIndex++)]);
            }
            //������������ַ��������ȡ��IO_BUFFER_SIZE���ַ��绺����
            else if (bufferIndex >= IO_BUFFER_SIZE)
            {
                ReadBuffer();
            }
            return Char.ToLower(ioBuffer[bufferIndex++]);
        }

        //���ַ�ѹ������ַ����鲢����һ������
        private void Push(char c)
        {
            buffer[length++] = c;
        }

        private void Prep(char c)
        {
            prepBuffer[prepLength++] = c;
        }
        //�����ݲ��ܺ�ǰһ���ַ���ɴ���ʱ��˷һ���ַ�
        private void Back()
        {
            bufferIndex--;
            offset--;
        }

        /// <summary>
        /// ��ȡ��ǰ�ִʵ�ƫ����
        /// </summary>
        public int Offset
        {
            get
            {
                return this.offset - this.length;
            }
        }

        //���ص�ǰ����
        private string Flush()
        {
            //�������ַ�������������ʱ����TOKEN,����رձ���ƥ��(�����Ѿ���β��)
            if (length > 0)
            {
                string token = new String(buffer, 0, length);

                //���Ԥ����������������Ԥ�����渴�Ƶ���������Ա��´δ���
                if (prepLength > 0)
                {
                    prepBuffer.CopyTo(buffer, 0);
                    length = prepLength;
                    prepLength = 0;
                }
                else
                {
                    length = 0;
                }
                return token;
            }
            else
            {
                return null;
            }
        }

        //����ƥ��
        private void StrangeWordMacth()
        {
            Char c;
            //������������5���ַ�
            while (Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.OtherLetter && length < 6)
            {
                Push(c);
            }

            //�Ի������е����ݽ�������ƥ��
            int len = StrangeWord.Match(buffer, length);
            //���ƥ��ʧ����ʼ��д����ƥ��
            if (len == 1 && NumberRule.Test(buffer[0]))
            {
                for (int i = 1; i < MAX_WORD_LEN; i++)
                {
                    if (i > length && Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.OtherLetter)
                    {
                        Push(c);
                    }
                    if (NumberRule.Test(buffer[i]))
                    {
                        len++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            bufferIndex = bufferIndex - length + len;
            offset = offset - length + len;
            length = len;
        }
        //��������
        private void ClearDifferentMeanings()
        {
            int len = length - 1;
            //�����ǰ�����к��и��̵Ĵ���ʱ�����ٷִʴ����������������������
            if (assoStream.BackToLastWordEnd())
            {
                int start = assoStream.Step;
                int end = start;
                assoStream.Reset();

                char c = buffer[end];
                //�Զ���Ĵʼ������д���ƥ��
                while (end < length && assoStream.Associate(c = buffer[end]))
                {
                    Prep(c);
                    end++;
                }

                //����������е�����ȫ��ƥ�����������ı����ж�ȡ
                if (end == length)
                {
                    while (Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.OtherLetter && assoStream.Associate(c))
                    {
                        Prep(c);
                    }
                }

                //���Ԥ����Ĵʳ��ֹ��������˷�����һ������
                if (!assoStream.IsWordEnd && assoStream.IsOccurWord)
                {
                    assoStream.BackToLastWordEnd();
                    bufferIndex = bufferIndex - prepLength + assoStream.Step;
                    offset = offset - prepLength + assoStream.Step;
                    prepLength = assoStream.Step;
                }

                ///�������һ�������Ĵ��ﲢ�Ҵ��ﳤ�ȱȻ������е���һ���������ʱ���������ж������ﳤ�ȣ����������ַ��ŵ�Ԥ���������������˷���δ���
                if (assoStream.IsWordEnd && (prepLength > len || prepLength > start && len > start + 1 || prepLength >= start && Char.GetUnicodeCategory(c) != UnicodeCategory.OtherLetter))
                {
                    len = start;
                }
                else
                {
                    if (end == length)
                    {
                        bufferIndex = bufferIndex + len - prepLength - start;
                        offset = offset + len - prepLength - start;
                    }
                    prepLength = 0;
                }
            }
            else if (len == 2)
            {
                //�������
                //WordPart wp1 = assoStream.GetWordPart();
                assoStream.Reset();
                char c = buffer[1];
                //�Ӵ���ĵڶ����ʿ�ʼ���ν��д���ƥ��
                if (assoStream.Associate(c) && assoStream.Associate(buffer[2]))
                {
                    Prep(c);
                    Prep(buffer[2]);
                    while (Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.OtherLetter && assoStream.Associate(c))
                    {
                        Prep(c);
                    }
                    if (!assoStream.IsWordEnd && assoStream.IsOccurWord)
                    {
                        assoStream.BackToLastWordEnd();
                        bufferIndex = bufferIndex - prepLength + assoStream.Step;
                        offset = offset - prepLength + assoStream.Step;
                        prepLength = assoStream.Step;
                    }
                    if (assoStream.IsWordEnd)
                    {
                        if (prepLength == 2)
                        {
                            //������Ϲ�����δʵ��
                            //WordPart wp2 = assoStream.GetWordPart();
                            //if(WordPart.Combo(wp1, wp2)
                            //    len = 2;
                            //else
                            //    len = 1;

                            //��ʱ���� 
                            assoStream.Reset();
                            if (assoStream.Associate(prepBuffer[1]) && assoStream.Associate(c))
                            {
                                char[] tmp = new char[MAX_WORD_LEN];
                                tmp[0] = prepBuffer[1];
                                tmp[1] = c;
                                int tmpLength = 2;
                                while (Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.OtherLetter && assoStream.Associate(c))
                                {
                                    tmp[tmpLength++] = c;
                                }
                                if (!assoStream.IsWordEnd && assoStream.IsOccurWord)
                                {
                                    assoStream.BackToLastWordEnd();
                                    bufferIndex = bufferIndex - tmpLength + assoStream.Step;
                                    offset = offset - tmpLength + assoStream.Step;
                                    tmpLength = assoStream.Step;
                                }
                                if (assoStream.IsWordEnd)
                                {
                                    tmp.CopyTo(prepBuffer, 0);
                                    prepLength = tmpLength;
                                }
                                else
                                {
                                    bufferIndex = bufferIndex - tmpLength;
                                    offset = offset - tmpLength;
                                    prepLength = 0;
                                }
                            }
                            else if (LinkRule.Test(buffer[0]))
                                len = 1;
                            else
                            {
                                bufferIndex = bufferIndex - prepLength + 1;
                                offset = offset - prepLength + 1;
                                prepLength = 0;
                            }
                        }
                        else
                        {
                            len = 1;
                        }
                    }
                    else
                    {
                        bufferIndex = bufferIndex - prepLength + 1;
                        offset = offset - prepLength + 1;
                        prepLength = 0;
                    }
                }
            }
            length = len;
        }

        /// <summary>
        /// ��һ������
        /// </summary>
        /// <returns></returns>
        public string Next()
        {
            if (length > 0)
            {
                return Flush();
            }
            //����������
            assoStream.Reset();

            //��ȡ��һ���ַ�
            char c = GetNextChar();

            //������������Ѿ�û����������ֹ��ǰ��ȡ
            if (dataLen < bufferIndex)
                return Flush();

            //���ַ������������
            Push(c);

            //�������ַ�������ѡ��ͬ�Ķ�ȡ����
            switch (Char.GetUnicodeCategory(c))
            {
                //������������ȡ֮���ȫ������ֱ�������������ַ�
                case UnicodeCategory.DecimalDigitNumber:
                    while (
                            (
                                Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.DecimalDigitNumber
                                || c == '.' && buffer[length - 1] != '.'
                            )
                            && length < MAX_WORD_LEN
                        )
                    {
                        Push(c);
                    }
                    Back();
                    return Flush().Trim('.');
                //�����Ӣ���ַ����ȡ֮���ȫ��Ӣ���ַ�ֱ��������Ӣ���ַ�
                case UnicodeCategory.LowercaseLetter:
                    while (
                            (
                                Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.LowercaseLetter
                                || c == '+'
                                || c == '#'
                            )
                            && length < MAX_WORD_LEN
                        )
                    {
                            Push(c);
                    }
                    Back();
                    return Flush();
                //����������ַ���ʼ���ķִʹ���
                case UnicodeCategory.OtherLetter:
                    if (c > 19967 && c < 40870 || c > 12353 && c < 12436)
                    {
                        assoStream.Associate(c);
                        //��ȡ�������һ���ַ��Ƿ��������ַ�
                        while (Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.OtherLetter && length < MAX_WORD_LEN)
                        {
                            //ͨ���ֵ�������ƥ�����ֱ���޷�ƥ��
                            if (!assoStream.IsBegin && assoStream.HasChildren && assoStream.Associate(c))
                            {
                                Push(c);
                                continue;
                            }
                            //������ֹ���ƥ��ɹ��Ĵ������ѻ���һ������
                            if (!assoStream.IsWordEnd && assoStream.IsOccurWord)
                            {
                                assoStream.BackToLastWordEnd();
                                bufferIndex = bufferIndex - length + assoStream.Step;
                                offset = offset - length + assoStream.Step;
                                length = assoStream.Step;
                            }
                            //���������һ�������Ĵ������жϴ���
                            if (assoStream.IsWordEnd)
                            {
                                Push(c);
                                ClearDifferentMeanings();
                            }
                            //�������������ƥ��
                            else
                            {
                                if (!LinkRule.Test(buffer[0]))
                                {
                                    Push(c);
                                    StrangeWordMacth();
                                }
                                else
                                {
                                    bufferIndex = bufferIndex - length;
                                    offset = offset - length;
                                    length = 1;
                                    return Flush();
                                }
                            }
                            break;
                        }
                    }
                    else
                    {
                        while (Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.OtherLetter && length < MAX_WORD_LEN)
                        {
                            Push(c);
                        }
                    }
                    Back();
                    return Flush();
                //����Ƿǿɶ��ַ�(����������,�ո��)��ֱ�ӽ�����һ���������
                default:
                    length = 0;
                    return String.Empty;
            }
        }
    }
}
