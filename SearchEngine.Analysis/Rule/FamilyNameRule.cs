using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SearchEngine.Analysis.Rule
{
    /// <summary>
    /// ���Ϲ���
    /// </summary>
    public class FamilyNameRule
    {
        private static string[] FAMILY_NAMES = {
            "��","��","��","��","��",
            "��","��","��","��","��",
            "��","��","��","Ҷ","��",
            "��","��","��","��","��",
            "ʯ","��","��","ë","��",
            "ʷ","��","��","��","��",
            "��","Ǯ","��","��","��",
            "��","��","��","��","��",
            "��","Ф","��","��","÷",
            "��","֣","��","��","ü",
            "��","��","��","��","��",
            "��","��","��","��","ţ",
            "��","��","��","��","��",
            "ʩ","��","��","��","��",
            "��","��","��","κ","ͯ",
            "��","��","��","л","Ϳ",
            "��","��","��","�","��",
            "��","��","��","��","��",
            "��","��","³","Τ","��",
            "��","��","Ԭ","ۺ","��",
            "��","��","��","��","Ԫ",
            "�","Ѧ","��","��","��",
            "��","��","��","��","��",
            "��","��","��","��","ˮ",
            "��","��","��","��","��",
            "��","��","Ҧ","��","��",
            "տ","��","��","��","��",
            "��","��","�","��","��",
            "��","��","é","��","��",
            "��","��","��","ף","��",
            "��","��","��","��","��",
            "��","��","¦","��","��",
            "��","��","��","��","Ī",
            "��","��","��","��","��",
            "��","��","¬","��","��",
            "��","��","��","��","չ",
            "��","��","��","��","��",
            "��","��","��","��","��",
            "��","��","½","��","��",
            "��","��","��","��","�",
            "��", "��", "��", "��", 
            "��", "��", "��", "��",
            "�", "��", "ۭ", "��",
            "��", "��", "��", "��",
            "��", "ղ", "��", "��",
            "۬", "��", "��", "��",
            "��", "ۢ", "��", "��",
            "׿", "��", "��", "��",
            "��", "��", "��", "ݷ",
            "��", "̷", "��", "��",
            "Ƚ", "۪", "Ӻ", "�",
            "ɣ", "��", "�", "��",
            "��", "��", "ׯ", "��",
            "��", "��", "Ľ", "��",
            "ϰ", "��", "��", "��",
            "��", "��", "��", "��",
            "��", "��", "��", "��",
            "��", "�", "��", "ε",
            "��", "¡", "��", "��",
            "��", "��", "��", "��",
            "��", "��", "��", "ؿ",
            "��", "��", "��", "��",
            "˾��", "��ٹ", "Ľ��",
            "˾��", "�Ϲ�", "ŷ��",
            "�ĺ�", "���", "����",
            "����", "����", "�ʸ�",
            "ξ��", "����", "�̨",
            "��ұ", "����", "���",
            "����", "����", "̫��",
            "����", "����", "����",
            "��ԯ", "���", "����",
            "����", "����", "˾ͽ",
            "�ذ�", "����", "���",
            "Ү��"
        };

        private static Hashtable m_FamilyNameTbl = new Hashtable();

        /// <summary>
        /// ���ϼ��
        /// </summary>
        /// <param name="hashCode">���ֵ�HASH����</param>
        /// <returns></returns>
        public static bool Test(int hashCode)
        {
            if (m_FamilyNameTbl.Count == 0)
            {
                foreach (String familyName in FAMILY_NAMES)
                {
                    Char[] cs = familyName.ToCharArray();
                    int code = (int)cs[0];
                    if (cs.Length > 1)
                    {
                        code = (code << 2) + (int)cs[1];
                    }
                    m_FamilyNameTbl.Add(code, null);
                }
            }
            return m_FamilyNameTbl.ContainsKey(hashCode);
        }
    }
}
