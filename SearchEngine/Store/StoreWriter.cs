using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using SearchEngine.Configuration;

namespace SearchEngine.Store
{
    /// <summary>
    /// �ĵ����д����
    /// </summary>
    internal class StoreWriter : StreamWriter
    {
        /// <summary>
        /// �ĵ����д�����Ĺ��캯��
        /// </summary>
        /// <param name="stream">ѹ����</param>
        public StoreWriter(GZipOutputStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// �ĵ����д�����Ĺ��캯��
        /// </summary>
        /// <param name="stream">�����</param>
        public StoreWriter(Stream stream)
            : this(new GZipOutputStream(stream))
        {
        }

        /// <summary>
        /// �ĵ����д�����Ĺ��캯��
        /// </summary>
        /// <param name="stream">�浵ID��</param>
        public StoreWriter(string path)
            : this(new FileStream(path, FileMode.Create, FileAccess.Write))
        {
        }
    }
}
