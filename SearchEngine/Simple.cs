using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Lucene.Net.Index;
using Lucene.Net.Documents;

using SearchEngine.Index;
using SearchEngine.Search;
using SearchEngine.Store;
using SearchEngine.Configuration;
using SearchEngine.Analysis;

namespace SearchEngine
{
    /// <summary>
    /// ����������׽ӿڵļ��׷�װ
    /// </summary>
    public sealed class Simple
    {
        private static Indexer indexer = new Indexer();
        private static Searcher seacher = new Searcher();

        /// <summary>
        /// ������������ĵ�
        /// </summary>
        /// <param name="id">�ĵ�ID��</param>
        /// <param name="author">����</param>
        /// <param name="cat">�������(����ID)</param>
        /// <param name="title">���±���</param>
        /// <param name="body">��������</param>
        /// <param name="tag">���±�ǩ�ؼ���</param>
        public static void AddDocument(string id, string author, string cat, string title, string body, string tag, string path)
        {
            body = Util.ParseText(body);

            Lucene.Net.Documents.Document doc = Util.CreateDocument(id, author, cat, title, body, tag, path);

            indexer.Write(doc);
        }

        /// <summary>
        /// ɾ���ĵ�����
        /// </summary>
        /// <param name="id">�ĵ�ID��</param>
        public static void Delete(string id)
        {
            indexer.Delete(new Lucene.Net.Index.Term("id", id));
        }

        /// <summary>
        /// ����/��������е��ĵ�
        /// </summary>
        /// <param name="id">�ĵ�ID��</param>
        /// <param name="author">����</param>
        /// <param name="cat">�������(����ID)</param>
        /// <param name="title">���±���</param>
        /// <param name="body">��������</param>
        /// <param name="tag">���±�ǩ�ؼ���</param>
        public static void Update(string id, string author, string cat, string title, string body, string tag, string path)
        {
            body = Util.ParseText(body);
            Lucene.Net.Documents.Document doc = Util.CreateDocument(id, author, cat, title, body, tag, path);
            indexer.Update(doc);
        }

        /// <summary>
        /// ���������ؼ��ֵ��ĵ�
        /// </summary>
        /// <param name="keywords">�ؼ���</param>
        /// <returns></returns>
        public static Documents Search(string keyword)
        {
            return seacher.Search(Util.KeywordSplit(keyword));
        }

        /// <summary>
        /// ���������ؼ��ֵ��ĵ�
        /// </summary>
        /// <param name="keywords">�ؼ���</param>
        /// <param name="onlyTitle">�Ƿ�ֻ��������</param>
        /// <returns></returns>
        public static Documents Search(string keyword, bool onlyTitle)
        {
            return seacher.Search(Util.KeywordSplit(keyword), onlyTitle);
        }

        /// <summary>
        /// ���������ؼ��ֵ��ĵ�
        /// </summary>
        /// <param name="author">����</param>
        /// <param name="cat">���(ID)</param>
        /// <param name="keyword">�ؼ���</param>
        /// <param name="onlyTitle">�Ƿ�ֻ��������</param>
        /// <returns></returns>
        public static Documents Search(string author, int cat, string keyword, bool onlyTitle)
        {
            return seacher.Search(author, cat, Util.KeywordSplit(keyword), onlyTitle);
        }

        /// <summary>
        /// �������tag
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static Documents SearchTag(string keyword)
        {
            return seacher.SearchTag(Util.KeywordSplit(keyword));
        }
    }
}
