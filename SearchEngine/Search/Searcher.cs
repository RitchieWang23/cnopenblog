using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SearchEngine.Analysis;
using SearchEngine.Store;
using SearchEngine.Configuration;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace SearchEngine.Search
{
    /// <summary>
    /// �ĵ�������
    /// </summary>
    public class Searcher
    {
        private Lucene.Net.Search.IndexSearcher searcher;

        /// <summary>
        /// �������ļ������仯�����������������ڴ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIndexChanged(object sender, FileSystemEventArgs e)
        {
            OpenIndex();
        }

        /// <summary>
        /// ������
        /// </summary>
        private void OpenIndex()
        {
            //��������������򴴽��հ�����
            if (!File.Exists(Directorys.IndexDirectory + "segments.gen"))
            {
                IndexWriter empty = new IndexWriter(Directorys.IndexDirectory, new ThesaurusAnalyzer(), true);
                empty.Optimize();
                empty.Close();
            }

            //����������Ѿ��������ȹر�������
            if (searcher != null)
            {
                searcher.Close();
            }
            searcher = new Lucene.Net.Search.IndexSearcher(Directorys.IndexDirectory);
        }

        /// <summary>
        /// ���������캯��
        /// </summary>
        public Searcher()
        {
            OpenIndex();
            FileSystemWatcher fsWatcher = new FileSystemWatcher(Directorys.IndexDirectory, "segments.gen");
            fsWatcher.EnableRaisingEvents = true;
            fsWatcher.IncludeSubdirectories = false;
            fsWatcher.Changed += new FileSystemEventHandler(OnIndexChanged);
        }

        /// <summary>
        /// ����ָ���������ĵ�
        /// ���أ����з����������ĵ�
        /// </summary>
        /// <param name="keywords">��ѯ�ؼ���</param>
        /// <returns></returns>
        public Documents Search(string keywords)
        {
            return Search(null, 0, keywords, false);
        }

        /// <summary>
        /// ����ָ���������ĵ�
        /// ���أ����з����������ĵ�
        /// </summary>
        /// <param name="keywords">��ѯ�ؼ���</param>
        /// <param name="onlyTitle">�Ƿ�ֻ��������</param>
        /// <returns></returns>
        public Documents Search(string keywords, bool onlyTitle)
        {
            return Search(null, 0, keywords, onlyTitle);
        }

        /// <summary>
        /// ����ָ���������ĵ�
        /// ���أ����з����������ĵ�
        /// </summary>
        /// <param name="author">����</param>
        /// <param name="cat">���ID��</param>
        /// <param name="keywords">��ѯ�ؼ���</param>
        /// <param name="onlyTitle">�Ƿ�ֻ��������</param>
        /// <returns></returns>
        public Documents Search(string author, int cat, string keywords, bool onlyTitle)
        {
            Lucene.Net.Search.Hits hits = searcher.Search(Util.BuildQuery(author, cat, keywords, onlyTitle));

            return new SearchEngine.Store.Hits(hits, searcher.Reader, keywords.Split(new char[] { ' ', '+' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public Documents SearchTag(string keywords)
        {
            Lucene.Net.Search.Hits hits = searcher.Search(Util.BuildTagQuery(keywords));

            return new SearchEngine.Store.Hits(hits, searcher.Reader, keywords.Split(new char[] { ' ', '+' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
