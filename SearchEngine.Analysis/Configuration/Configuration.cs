using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SearchEngine.Analysis.Configuration
{
    /// <summary>
    /// �ֵ�Ŀ¼����
    /// </summary>
    public static class Directorys
    {
        /// <summary>
        /// ��ȡ�ֵ�Ŀ¼
        /// </summary>
        public static string DictsDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["DictsDirectory"];
            }
        }
    }
}
