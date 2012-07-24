using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using Lucene.Net.Store;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis;

using SearchEngine.Analysis;
using SearchEngine.Index;
using SearchEngine.Search;
using SearchEngine.Store;
using SearchEngine.Configuration;
using ThesaurusAnalysis.Rule;

namespace SearchEngine
{
    public class Util
    {
        static Analyzer analyzer = new WhitespaceAnalyzer();

        /// <summary>
        /// �����ı����ݣ�������Ч�ַ�
        /// </summary>
        /// <param name="text">������������</param>
        /// <returns></returns>
        public static string ParseText(string text)
        {
            text = Regex.Replace(text, "<br[^>]*>", "\n", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "<[^>]*>", " ");
            text = HttpUtility.HtmlDecode(text);
            text = Regex.Replace(text, @"\s+", " ");
            text = Regex.Replace(text, @"([^\w\d])(\1+){2,}",
                delegate(Match m)
                {
                    string s = m.Value;
                    if (s == "++" || s == "--")
                    {
                        return s;
                    }
                    return s[0].ToString();
                });

            text = Regex.Replace(text, @"[\u0000-\u0008\u000B\u000C\u000E-\u001A\uD800-\uDFFF]",
                delegate(Match m)
                {
                    int code = (int)m.Value.ToCharArray()[0];
                    return (code > 9 ? "&#" + code.ToString() : "&#0" + code.ToString()) + ";";
                }
            );
            return text;
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="id">�ĵ�ID��</param>
        /// <param name="author">����</param>
        /// <param name="cat">�������(����ID)</param>
        /// <param name="title">���±���</param>
        /// <param name="body">��������</param>
        /// <param name="tag">��ǩ</param>
        /// <param name="path">�ĵ�·��</param>
        /// <returns></returns>
        public static Lucene.Net.Documents.Document CreateDocument(string id, string author, string cat, string title, string body, string tag, string path)
        {
            Lucene.Net.Documents.Document doc = new Lucene.Net.Documents.Document();

            doc.Add(new Field("id", id, Field.Store.YES, Field.Index.UN_TOKENIZED, Field.TermVector.NO));
            doc.Add(new Field("author", author, Field.Store.NO, Field.Index.UN_TOKENIZED, Field.TermVector.NO));
            doc.Add(new Field("cat", cat, Field.Store.NO, Field.Index.UN_TOKENIZED, Field.TermVector.NO));
            doc.Add(new Field("title", title, Field.Store.NO, Field.Index.TOKENIZED, Field.TermVector.NO));
            doc.Add(new Field("body", body, Field.Store.NO, Field.Index.TOKENIZED, Field.TermVector.WITH_OFFSETS));
            doc.Add(new Field("tag", tag, Field.Store.NO, Field.Index.TOKENIZED, Field.TermVector.NO));
            doc.Add(new Field("path", path, Field.Store.NO, Field.Index.TOKENIZED, Field.TermVector.NO));
            doc.Add(new Field("date", DateField.DateToString(DateTime.Now), Field.Store.YES, Field.Index.NO, Field.TermVector.NO));

            //����Ȩ�أ�Խ���������Ȩ��Խ������������е�λ�ÿ�ǰ�Ļ����Խ��
            float boost = Single.Parse(DateTime.Now.ToString("0.yyyyMMddhh"));
            doc.SetBoost(boost);

            //ȷ�������ĵ�ѹ������·��
            string fpath = Directorys.StoreDirectory + Math.Ceiling(Double.Parse(id) / 10000D).ToString("f0");
            if (!System.IO.Directory.Exists(fpath))
            {
                System.IO.Directory.CreateDirectory(fpath);
            }

            //���ĵ���gzip��ʽ���浽��Ӧλ��
            StoreWriter store = new StoreWriter(fpath + @"\" + id + ".gz");
            store.WriteLine(author);
            store.WriteLine(cat);
            store.WriteLine(tag);
            store.WriteLine(title);
            store.WriteLine(path);
            store.WriteLine(body);
            store.Close();

            return doc;
        }

        /// <summary>
        /// �ؼ��ֲ�ֲ�����
        /// </summary>
        /// <param name="keyword">�ؼ���</param>
        /// <returns></returns>
        public static string KeywordSplit(string keyword)
        {
            //�������Ĵ��ﰴ�տո����
            string[] keyGroups = keyword.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder keywords = new StringBuilder(/*keyword*/);

            for (int i = 0; i < keyGroups.Length; i++)
            {
                ThesaurusSpliter ts = new ThesaurusSpliter(new StringReader(keyGroups[i]));
                for (string word = ts.Next(); word != null; word = ts.Next())
                {
                    if (word.Length > 0 && !StopWord.Test(word))
                    {
                        keywords.Append(" ");
                        keywords.Append(word);
                    }
                }

                keywords.Append("+");
            }

            string trem = keywords.ToString().Trim(new char[] { ' ', '+' });

            if (trem.Length > 0)
            {
                return trem;
            }

            return keyword.Trim();
        }

        /// <summary>
        /// ����������ѯ����
        /// </summary>
        /// <param name="keyword">�ؼ���</param>
        /// <returns></returns>
        public static Query BuildQuery(string keyword)
        {
            return BuildQuery(null, 0, keyword, false);
        }

        /// <summary>
        /// ����������ѯ����
        /// </summary>
        /// <param name="keyword">�ؼ���</param>
        /// <param name="onlyTitle">�Ƿ�ֻ��������</param>
        /// <returns></returns>
        public static Query BuildQuery(string keyword, bool onlyTitle)
        {
            return BuildQuery(null, 0, keyword, onlyTitle);
        }

        /// <summary>
        /// ����������ѯ����
        /// </summary>
        /// <param name="author">����</param>
        /// <param name="cat">���ID��</param>
        /// <param name="keyword">�ؼ���</param>
        /// <param name="onlyTitle">�Ƿ�ֻ��������</param>
        /// <returns></returns>
        public static Query BuildQuery(string author, int cat, string keyword, bool onlyTitle)
        {
            BooleanQuery query = new BooleanQuery();

            QueryParser parser;

            if (!String.IsNullOrEmpty(author))
            {
                parser = new QueryParser("author", analyzer);
                query.Add(parser.Parse(author), BooleanClause.Occur.MUST);
            }
            if (cat > 0)
            {
                parser = new QueryParser("cat", analyzer);
                query.Add(parser.Parse(cat.ToString()), BooleanClause.Occur.MUST);
            }
            //ֻ���ԡ�+���ָ���򲻶�
            string[] trems = keyword.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string key in trems)
            {
                if (onlyTitle)
                {
                    parser = new QueryParser("title", analyzer);
                    query.Add(parser.Parse(key), BooleanClause.Occur.MUST);
                }
                else
                {
                    BooleanQuery qu = new BooleanQuery();

                    parser = new QueryParser("title", analyzer);
                    qu.Add(parser.Parse(key), BooleanClause.Occur.SHOULD);

                    parser = new QueryParser("tag", analyzer);
                    qu.Add(parser.Parse(key), BooleanClause.Occur.SHOULD);

                    parser = new QueryParser("body", analyzer);
                    qu.Add(parser.Parse(key), BooleanClause.Occur.SHOULD);

                    query.Add(qu, BooleanClause.Occur.MUST);
                }
            }
            return query;
        }

        /// <summary>
        /// ����tag��ѯ����
        /// </summary>
        /// <param name="keyword">tag</param>
        /// <returns></returns>
        public static Query BuildTagQuery(string keyword)
        {
            BooleanQuery query = new BooleanQuery();

            QueryParser parser;

            string[] trems = keyword.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string key in trems)
            {
                parser = new QueryParser("tag", analyzer);
                query.Add(parser.Parse(key), BooleanClause.Occur.MUST);
            }
            return query;
        }
    }
}
