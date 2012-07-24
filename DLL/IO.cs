using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace DLL
{
    public class IO
    {
        public static Encoding encode = Encoding.Default;

        public static string LoadFile(string path)
        {
            return LoadFile(path, encode);
        }
        /// <summary>
        /// ��ȡ�ļ�
        /// </summary>
        /// <param name="path">����·��</param>
        /// <returns></returns>
        public static string LoadFile(string path, Encoding encod)
        {
            path = path.Replace("/", "\\");
            string text = "";
            FileStream fs = null;
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                    break;
                }
                catch
                {
                    Thread.Sleep(50);
                }
            }
            if (fs != null)
            {
                StreamReader sr = new StreamReader(fs, encod);
                text = sr.ReadToEnd();
                sr.Close();
                fs.Close();
            }
            return text;
        }
        public static void WriteFile(string path, string content)
        {
            WriteFile(path, content, encode);
        }

        /// <summary>
        /// д���ļ�����������򱻸�д
        /// </summary>
        /// <param name="path">����·��</param>
        /// <param name="content">�ļ�����</param>
        public static void WriteFile(string path, string content, Encoding encod)
        {
            path = path.Replace("/", "\\");
            FileStream fs = null;
            string dir = path.Substring(0, path.LastIndexOf(@"\"));
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            for (int i = 0; i < 10; i++)
            {
                try
                {
                    fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                    break;
                }
                catch
                {
                    Thread.Sleep(50);
                }
            }
            if (fs != null)
            {
                StreamWriter sw = new StreamWriter(fs, encod);
                sw.Write(content);
                sw.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="path">����·��</param>
        public static void DeleteFile(string path)
        {
            path = path.Replace("/", "\\");
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    File.Delete(path);
                    break;
                }
                catch
                {
                    Thread.Sleep(50);
                }
            }
        }
    }
}
