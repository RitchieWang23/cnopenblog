using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using SearchEngine.Store;
using SearchEngine.Configuration;
using System.Globalization;

namespace SearchEngine.Store
{
    /// <summary>
    /// ��������ĵ�
    /// </summary>
    public abstract class Document
    {
        protected string id;
        protected string author;
        protected string cat;
        protected string title;
        protected string tag;
        protected string body;
        protected string path;
        protected DateTime lastIndex;

        /// <summary>
        /// ��ȡ����ĵ���ID��
        /// </summary>
        public string ID
        {
            get
            {
                return this.id;
            }
        }
        /// <summary>
        /// ��ȡ����ĵ�������
        /// </summary>
        public string Author
        {
            get
            {
                return this.author;
            }
        }
        /// <summary>
        /// ��ȡ����ĵ������
        /// </summary>
        public int Cat
        {
            get
            {
                int c = 0;
                int.TryParse(this.cat, out c);
                return c;
            }
        }
        /// <summary>
        /// ��ȡ����ĵ��ı���
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }
        }
        /// <summary>
        /// ��ȡ����ĵ��ı�ǩ
        /// </summary>
        public string Tag
        {
            get
            {
                return this.tag;
            }
        }
        /// <summary>
        /// ��ȡ����ĵ�������
        /// </summary>
        public string Body
        {
            get
            {
                return this.body;
            }
        }
        /// <summary>
        /// ��ȡ�ĵ�·��
        /// </summary>
        public string Path
        {
            get { return this.path; }
        }
        /// <summary>
        /// ��ȡ����ĵ��������������
        /// </summary>
        public DateTime LastIndex
        {
            get
            {
                return this.lastIndex;
            }
        }
    }

    /// <summary>
    /// ���н��
    /// </summary>
    internal class Hit : Document
    {
        /// <summary>
        /// ���н�����캯��
        /// </summary>
        /// <param name="doc">������</param>
        /// <param name="offset">�ؼ����������е�λ��</param>
        internal Hit(Lucene.Net.Documents.Document doc, int offset)
        {
            base.id = doc.Get("id");
            base.lastIndex = Lucene.Net.Documents.DateField.StringToDate(doc.Get("date"));

            //�����ⲿ��ŵ��ĵ�ʵ��
            StoreReader story = new StoreReader(Directorys.StoreDirectory + Math.Ceiling(Double.Parse(base.id) / 10000D).ToString("f0") + @"\" + base.id + ".gz");
            //��ȡ�ѱ��������ͷ
            base.author = story.ReadLine();
            base.cat = story.ReadLine();
            base.tag = story.ReadLine();
            base.title = story.ReadLine();
            base.path = story.ReadLine();

            int readed = 0;

            int len = 126;//��ʾ���ݳ���

            char[] block = new char[offset + len];

            //��ȡ�������ؼ��ֺ�len���ַ�
            readed = story.ReadBlock(block, 0, block.Length);

            story.Close();

            int index = offset;

            //����ؼ��ֲ��ڽ�β����ժҪ��ʼλ�ö�λ�ڹؼ���ǰһ��������֮�󣬷���ժҪȡĩβ��len���ַ�
            if (readed == block.Length)
            {
                UnicodeCategory category;
                for (; index > 0; index--)
                {
                    category = Char.GetUnicodeCategory(Char.ToLower(block[index]));
                    if (category == UnicodeCategory.OtherPunctuation)
                    {
                        index += 1;
                        break;
                    }
                }
            }
            else
            {
                index = Math.Max(0, readed - len);
            }

            //���ժҪ���ڽ�β�����ں�����ӡ�...��
            base.body = (new String(block, index, Math.Min(len - 1, readed))) + ((readed >= index + len) ? "..." : "");
        }
    }
}
