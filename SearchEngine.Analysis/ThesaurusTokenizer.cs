using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Analysis;
using System.IO;
using ThesaurusAnalysis.Rule;

namespace SearchEngine.Analysis
{
    /// <summary>
    /// ThesaurusTokenizer
    /// </summary>
    public class ThesaurusTokenizer : Tokenizer
    {
        private ThesaurusSpliter spliter;

        /// <summary>
        /// ThesaurusTokenizer���캯��
        /// </summary>
        /// <param name="input"></param>
        public ThesaurusTokenizer(TextReader input)
        {
            spliter = new ThesaurusSpliter(input);
        }

        /// <summary>
        /// ������һ��Token
        /// </summary>
        /// <returns></returns>
        public override Token Next()
        {
            for (string word = spliter.Next(); word != null; word = spliter.Next())
            {
                if (word.Length > 0 && !StopWord.Test(word))
                {
                    return new Token(word, spliter.Offset - word.Length, spliter.Offset);
                }
            }

            //�ر��ı���
            Close();
            return null;
        }
    }
}
