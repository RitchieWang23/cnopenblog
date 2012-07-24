using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using SearchEngine.Configuration;

namespace SearchEngine.Store
{

    /// <summary>
    /// �ĵ������ȡ��
    /// </summary>
    internal class StoreReader : StreamReader
    {
        private Stream baseStream = null;

        /// <summary>
        /// �ĵ������ȡ���Ĺ��캯��
        /// </summary>
        /// <param name="stream">��ѹ����</param>
        public StoreReader(GZipInputStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// �ĵ������ȡ���Ĺ��캯��
        /// </summary>
        /// <param name="stream">������</param>
        public StoreReader(Stream stream)
            : this(new GZipInputStream(stream))
        {
            baseStream = stream;
        }

        /// <summary>
        /// �ĵ������ȡ���Ĺ��캯��
        /// </summary>
        /// <param name="id">�浵ID��</param>
        public StoreReader(string path)
            : this(File.OpenRead(path))
        {
        }

        /// <summary>
        /// �رոö�ȡ��
        /// </summary>
        public override void Close()
        {
            if (baseStream != null)
                baseStream.Close();
        }
    }
}
