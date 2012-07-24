using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SearchEngine.Analysis.Rule
{
    /// <summary>
    /// �����ֹ���
    /// </summary>
    public class LinkRule
    {
        private static Char[] LINK_CHAR = new Char[] {
            '��', '��', '��', '��', '��', 'Ϊ', '��', '��',
            '��', '��', '��', '��', '��', '��', 'Ҳ', '��',
            '��', '��', '��', '��', '��', '��', '��', '��',
            'ȥ', '��', 'ֻ', '��', '��', '��', '��', '��',
            '��', '��', '��', '��', '��', '��', '��', '��',
            '��', '��', '��', '��', 'ʹ', '��', '��', 'ȴ',
            '��', 'ƾ', '��', '��', '��', 'ô', '��', '��',
            '��', '��', '��', '��', '��', '��', '��', '��',
            '��', '��', '��', '��', '��', '��', '��', '��',
            '��', '��', '��', '��', '֮', '��', '��', 'ѽ',
            '��', '��', 'Լ', '��', '��', '��', '˵'
        };
        private static Hashtable m_LinkCharTbl = new Hashtable();

        /// <summary>
        /// �����ַ����
        /// </summary>
        /// <param name="c">�������ַ�</param>
        /// <returns></returns>
        public static bool Test(Char c)
        {
            if (m_LinkCharTbl.Count == 0)
            {
                foreach (Char linkChar in LINK_CHAR)
                {
                    m_LinkCharTbl.Add(linkChar, null);
                }
            }
            return m_LinkCharTbl.ContainsKey(c);
        }
    }
}
