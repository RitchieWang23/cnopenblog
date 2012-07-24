using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SearchEngine.Configuration
{
    /// <summary>
    /// ��������Ŀ¼����
    /// </summary>
    public static class Directorys
    {
        /// <summary>
        /// ��ȡ����Ŀ¼
        /// </summary>
        public static string IndexDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["IndexDirectory"];
            }
        }

        /// <summary>
        /// ��ȡ�ĵ����Ŀ¼
        /// </summary>
        public static string StoreDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["StoreDirectory"];
            }
        }
    }
}
