using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SearchEngine.Analysis.Rule
{
    /// <summary>
    /// ��������
    /// </summary>
    public class PlaceRule
    {
        private static Char[] PLACE_CHAR = new Char[] {
            'ʡ', '��', '��', '��',
            '��', '��', '��', '��',
            '��', '��', 'Ӫ', '��',
            '·', '��', '��', 'Ū',
            '��', '��', '��', '��',
            'ɽ', '��', '��', '��',
            '��', '̶', 'Ϫ', '��',
            '��', '��', '��', '��',
            '��', '��', '��', '��',
            'Ȫ', '��', '��', '��'
        };

        private static Hashtable m_PlaceCharTbl = new Hashtable();

        /// <summary>
        /// ����β�ַ����
        /// </summary>
        /// <param name="c">������ַ�</param>
        /// <returns></returns>
        public static bool Test(Char c)
        {
            if (m_PlaceCharTbl.Count == 0)
            {
                foreach (Char placeChar in PLACE_CHAR)
                {
                    m_PlaceCharTbl.Add(placeChar, null);
                }
            }

            return m_PlaceCharTbl.ContainsKey(c);
        }
    }
}
