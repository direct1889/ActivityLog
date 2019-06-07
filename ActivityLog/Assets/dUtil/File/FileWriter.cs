using UnityEngine;
using System.IO;
using System.Collections.Generic;


namespace du.File {

    public interface IFWriter : System.IDisposable {
        void Write(string line);
        void WriteBlankLine();
        void Flush();
        void Close();
    }

    public class FWriter : IFWriter {

        #region field
        StreamWriter m_sw;
        string m_filePath;
        #endregion

        #region ctor/dtor
        private FWriter(string filePath) {
            m_filePath = filePath;
        }
        #endregion

        #region private
        private void OpenRewrite() {
            if (m_sw == null) {
                FileInfo fi = new FileInfo(m_filePath);
                m_sw = fi.CreateText();
            }
        }
        private void OpenAppend() {
            if (m_sw == null) {
                FileInfo fi = new FileInfo(m_filePath);
                m_sw = fi.AppendText();
            }
        }
        #endregion

        #region public
        public void Write(string line) {
            if (line != null && line != "") {
                m_sw.WriteLine(line);
            }
        }
        public void WriteBlankLine() { m_sw.WriteLine(); }
        public void Flush(){ m_sw?.Flush(); }
        public void Close(){
            m_sw?.Flush();
            m_sw?.Close();
        }
        public void Dispose() { Close(); }
        #endregion

        #region static
        /// <param name="filePath"> 拡張子も含めて渡す </param>
        public static IFWriter OpenFile4Rewrite(string filePath) {
            var writer = new FWriter(filePath);
            writer.OpenRewrite();
            return writer;
        }
        public static IFWriter OpenFile4Append(string filePath) {
            var writer = new FWriter(filePath);
            writer.OpenAppend();
            return writer;
        }
        #endregion

    }

}