using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using SearchEngine.Analysis.Configuration;

namespace SearchEngine.Analysis.Coder
{
    /// <summary>
    /// �򷱾���
    /// </summary>
    public class CodeMap
    {
        private static Hashtable m_CodeMap = new Hashtable();

        private static Hashtable m_UnCodeMap = new Hashtable();

        private static void Init()
        {
            if (m_CodeMap.Count == 0)
            {
                StreamReader reader = new StreamReader(Path.Combine(Directorys.DictsDirectory, @"code.map"), Encoding.UTF8);
                String map = null;
                while ((map = reader.ReadLine()) != null)
                {
                    if ((map = map.Trim()).Length > 1 && !map.StartsWith("#"))
                    {
                        if(!m_CodeMap.ContainsKey(map[0]))
                            m_CodeMap.Add(map[0], map[1]);
                        if (!m_UnCodeMap.ContainsKey(map[1]))
                            m_UnCodeMap.Add(map[1], map[0]);
                    }
                }
                reader.Close();
            }
        }
        private static Char Escape(Char c)
        {
            return m_CodeMap.ContainsKey(c) ? (char)m_CodeMap[c] : c;
        }

        private static Char UnEscape(Char c)
        {
            return m_UnCodeMap.ContainsKey(c) ? (char)m_UnCodeMap[c] : c;
        }

        /// <summary>
        /// ���ַ������еķ����ַ�ת��Ϊ����
        /// </summary>
        /// <param name="c">�ַ�����</param>
        /// <param name="startIndex">����ת������ʼ��ʼ</param>
        /// <param name="count">Ҫת�����ַ�����</param>
        public static void Escape(Char[] c, int startIndex, int count)
        {
            Init();
            for (; startIndex < count; startIndex++)
            {
                c[startIndex] = Escape(c[startIndex]);
            }
        }

        /// <summary>
        /// ���ַ������еļ����ַ�ת��Ϊ����
        /// </summary>
        /// <param name="c">�ַ�����</param>
        /// <param name="startIndex">����ת������ʼ��ʼ</param>
        /// <param name="count">Ҫת�����ַ�����</param>
        public static void UnEscape(Char[] c, int startIndex, int count)
        {
            Init();
            for (; startIndex < count; startIndex++)
            {
                c[startIndex] = UnEscape(c[startIndex]);
            }
        }
    }
}
