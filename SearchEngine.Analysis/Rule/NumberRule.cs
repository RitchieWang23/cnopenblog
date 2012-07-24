using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SearchEngine.Analysis.Rule
{
    /// <summary>
    /// ��д���ֹ���
    /// </summary>
    public class NumberRule
    {
        private static Char[] NUMBER_CHAR = new Char[] {
            '��', 'һ', '��', '��', '��', '��', '��', '��', '��', '��', 'ʮ', '��', 'ǧ', '��', '��',
            'Ҽ', '��', '��', '��', '��', '½', '��', '��', '��', '��', 'ʰ', '��', 'Ǫ', '��', '��', '��', 'ئ'
        };

        private static Hashtable m_NumberCharTbl = new Hashtable();

        /// <summary>
        /// ��д���ּ��
        /// </summary>
        /// <param name="number">������ַ�</param>
        /// <returns></returns>
        public static bool Test(Char number)
        {
            if (m_NumberCharTbl.Count == 0)
            {
                foreach (Char numberChar in NUMBER_CHAR)
                {
                    m_NumberCharTbl.Add(numberChar, null);
                }
            }

            return m_NumberCharTbl.ContainsKey(number);
        }
    }
}
