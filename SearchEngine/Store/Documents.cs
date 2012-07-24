using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Index;

namespace SearchEngine.Store
{
    /// <summary>
    /// ��������ĵ�����
    /// </summary>
    public abstract class Documents
    {
        protected IndexReader reader;
        protected Lucene.Net.Search.Hits _hits;
        protected string[] terms;

        /// <summary>
        /// ��ȡ����ʹ�õĹؼ��ִַʽ���ַ���
        /// </summary>
        public string Keywords
        {
            get
            {
                return String.Join(" ", this.terms);
            }
        }

        /// <summary>
        /// ��ȡ���������ĵ�����
        /// </summary>
        public int Count
        {
            get
            {
                return this._hits.Length();
            }
        }

        /// <summary>
        /// ��ȡ��ǰ��������ĵ�������ָ������λ�õ��ĵ�
        /// </summary>
        /// <param name="index">�ĵ�����������ĵ������еĴ��㿪ʼ������</param>
        /// <returns></returns>
        public Document this[int index]
        {
            get
            {
                int offset = 0;

                //�����������е�λ������
                TermPositionVector termPositionVector = (TermPositionVector)this.reader.GetTermFreqVector(this._hits.Id(index), "body");

                //�������λ������
                if (termPositionVector != null)
                {
                    int pos = -1;

                    for (int i = 0; i < terms.Length; i++)
                    {
                        //��һ�����еĹؼ����������е�λ��
                        pos = System.Array.IndexOf<string>(termPositionVector.GetTerms(), terms[i]);
                        if (pos > -1)
                            break;
                    }

                    //������������ҵ���Ӧ�ؼ�����ȡ���ؼ����������е�ƫ����
                    if (pos > -1)
                    {
                        TermVectorOffsetInfo[] tvois = termPositionVector.GetOffsets(pos);
                        offset = tvois[0].GetStartOffset();
                    }
                }
                return new Hit(this._hits.Doc(index), offset);
            }
        }
    }

    /// <summary>
    /// ���н������
    /// </summary>
    internal class Hits : Documents
    {
        /// <summary>
        /// ���н������
        /// </summary>
        /// <param name="hits">�����������</param>
        /// <param name="reader">������ȡ��</param>
        /// <param name="terms">�ؼ��ִַʽ������</param>
        public Hits(Lucene.Net.Search.Hits hits, IndexReader reader, string[] terms)
        {
            base.reader = reader;
            base._hits = hits;
            base.terms = terms;
        }
    }
}
