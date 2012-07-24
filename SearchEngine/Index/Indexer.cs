using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SearchEngine.Analysis;
using SearchEngine.Configuration;

namespace SearchEngine.Index
{
    /// <summary>
    /// �ĵ�����������
    /// </summary>
    public class Indexer
    {
        private Writer writer;

        /// <summary>
        /// ��������д��
        /// </summary>
        /// <param name="doc">��д����ĵ�</param>
        public void Write(Lucene.Net.Documents.Document doc)
        {
            writer.Write(doc);
        }

        /// <summary>
        /// ɾ�������з����������ĵ�
        /// </summary>
        /// <param name="term">ɾ������</param>
        public void Delete(Lucene.Net.Index.Term term)
        {
            writer.Delete(term);
        }

        /// <summary>
        /// ���£���ӣ������е��ĵ�
        /// </summary>
        /// <param name="doc">�����ĵ������������Զ��ҵ���ͬID�ŵ��ĵ����и���</param>
        public void Update(Lucene.Net.Documents.Document doc)
        {
            writer.Update(doc);
        }

        /// <summary>
        /// �ĵ��������������캯��
        /// </summary>
        public Indexer()
        {
            writer = new Writer();
        }

        private sealed class Writer
        {
            /// <summary>
            /// ������д��
            /// </summary>
            public static Lucene.Net.Index.IndexWriter writer;
            /// <summary>
            /// ��󻺳��ĵ���
            /// </summary>
            public int maxBufferLength = 0x100;
            /// <summary>
            /// ִ���߳�
            /// </summary>
            public Thread thread;
            /// <summary>
            /// ����������
            /// </summary>
            public Queue<Lucene.Net.Documents.Document> addQueue;
            /// <summary>
            /// �����������
            /// </summary>
            public Queue<Lucene.Net.Documents.Document> updateQueue;
            /// <summary>
            /// ɾ���������
            /// </summary>
            public Queue<Lucene.Net.Index.Term> deleteQueue;

            /// <summary>
            /// �ĵ�д�������캯��
            /// </summary>
            public Writer()
            {
                addQueue = new Queue<Lucene.Net.Documents.Document>();
                updateQueue = new Queue<Lucene.Net.Documents.Document>();
                deleteQueue = new Queue<Lucene.Net.Index.Term>();

                thread = new Thread(new ThreadStart(IndexWriteHandler));
                thread.IsBackground = true;
                thread.Priority = ThreadPriority.BelowNormal;
                thread.Start();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="bufferLength"></param>
            public Writer(int bufferLength)
                : this()
            {
                SetBufferLength(bufferLength);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="bufferLength"></param>
            public void SetBufferLength(int bufferLength)
            {
                this.maxBufferLength = bufferLength;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="doc"></param>
            public void Write(Lucene.Net.Documents.Document doc)
            {
                this.addQueue.Enqueue(doc);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="term"></param>
            public void Delete(Lucene.Net.Index.Term term)
            {
                this.deleteQueue.Enqueue(term);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="doc"></param>
            public void Update(Lucene.Net.Documents.Document doc)
            {
                this.updateQueue.Enqueue(doc);
            }
            ~Writer()
            {
                try
                {
                    writer.Optimize();
                    writer.Close();
                }
                catch
                {
                    File.Delete(Directorys.IndexDirectory + "write.lock");
                }
            }

            /// <summary>
            /// ����ִ����
            /// </summary>
            public void IndexWriteHandler()
            {
                //�����ĵ�������д��
                writer = new Lucene.Net.Index.IndexWriter(Directorys.IndexDirectory, new ThesaurusAnalyzer(), !File.Exists(Directorys.IndexDirectory + "segments.gen"));
                //���������Ƭ����
                writer.SetMaxBufferedDocs(maxBufferLength);
                //�״������Ż�
                writer.Optimize();
                int count = 0;

                //����ѭ��
                while (true)
                {
                    //����ɾ������
                    while (deleteQueue.Count > 0 && count < maxBufferLength)
                    {
                        count++;
                        writer.DeleteDocuments(deleteQueue.Dequeue());
                    }
                    //������¶���
                    while (updateQueue.Count > 0 && count < maxBufferLength)
                    {
                        count++;
                        Lucene.Net.Documents.Document doc = updateQueue.Dequeue();
                        writer.UpdateDocument(new Lucene.Net.Index.Term("id", doc.Get("id")), doc);
                    }
                    //������������
                    while (addQueue.Count > 0 && count < maxBufferLength)
                    {
                        count++;
                        writer.AddDocument(addQueue.Dequeue());
                    }
                    //������뵵�򱣴���Ƭ
                    if (writer.NumRamDocs() > 0)
                    {
                        writer.Flush();
                    }
                    //��⴦������Ƿ�ﵽ��󻺳���,��������󻺳���ʱ�Ż���Ƭ,�����߳���ͣ100����
                    if (count >= maxBufferLength)
                    {
                        writer.Optimize();
                        count = 0;
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }
        }
    }
}
