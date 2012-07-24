using System;
using System.Collections.Generic;
using System.Text;
using SearchEngine.Analysis.Thesaurus;
using SearchEngine.Analysis.Rule;

namespace SearchEngine.Analysis.Match
{
    /// <summary>
    /// ���ʼ��
    /// </summary>
    public class StrangeWord
    {
        /// <summary>
        /// ���ַ����н������ʼ��
        /// </summary>
        /// <param name="word">�ַ�����</param>
        /// <param name="length">��Ч�ַ�����</param>
        /// <returns></returns>
        public static int Match(Char[] word, int length)
        {
            int nStart = 0;
            //���ϼ��
            if (FamilyNameRule.Test(((int)word[0] << 2) + (int)word[1]))
            {
                nStart = 2;
            }
            else if (FamilyNameRule.Test((int)word[0]) && !LinkRule.Test(word[1]))
            {
                nStart = 1;
            }
            else
            {
                AssociateStream assoStream = new AssociateStream();
                //���ʼ��,������������������д���ƥ��
                if (!LinkRule.Test(word[1]))
                {
                    for (nStart = 1; nStart < length; )
                    {
                        assoStream.Associate(word[nStart++]);
                        if (!assoStream.IsBegin && assoStream.Associate(word[nStart]))
                        {
                            return nStart - 1;
                        }
                        else
                        {
                            assoStream.Reset();
                        }
                    }
                }
                //�������
                for (nStart = 0; nStart < length - 1;)
                {
                    assoStream.Reset();
                    if (!LinkRule.Test(word[nStart++]) && PlaceRule.Test(word[nStart]))
                    {
                        return nStart + 1;
                    }
                }
                return 1;
            }
            //�����⵽���ϲ���ʣ���ַ���С��3���ַ�����������һ����������
            if(length - nStart <= 2)
            {
                return length;
            }
            //������ĳ��ȴ������ϵĳ�����ʼ�����Ч�����ֳ���
            if (length > nStart)
            {
                int nEnd = nStart + 1;
                AssociateStream assoStream = new AssociateStream();

                //�����һ�����������ֻ��ߵ�ǰ�ַ�����һ�����޷���ϳ�һ����֪���򽫵�ǰ�ֱ�ȷ��Ϊʱ������һ����
                if (LinkRule.Test(word[nEnd + 1]) || !(assoStream.Associate(word[nEnd]) && assoStream.Associate(word[nEnd + 1])))
                {
                    nEnd++;
                }
                nStart = nEnd;
            }
            return nStart;
        }
    }
}