using UnityEngine;
using System.IO;
using System.Collections.Generic;


namespace du {

    namespace File {

        public interface IFileWriter {
            void Write(string line);
            void WriteBlankLine();
            void Close();
        }

        public class FileWriter : IFileWriter {

            #region field
            StreamWriter m_sw = null;
            string m_fileName = null;
            #endregion

            #region ctor/dtor
            private FileWriter(string fileName, bool fromResources) {
                m_fileName = Application.dataPath + (fromResources ? "/Resources/" : "/") + fileName;
            }
            ~FileWriter() {
                Close();
            }
            #endregion

            #region private
            private void OpenRewrite() {
                FileInfo fi = new FileInfo(m_fileName);
                m_sw = fi.CreateText();
            }
            private void OpenAppend() {
                FileInfo fi = new FileInfo(m_fileName);
                m_sw = fi.AppendText();
            }
            #endregion

            #region public
            public void Write(string line) {
                if (line != null && line != "") {
                    m_sw.WriteLine(line);
                }
            }
            public void WriteBlankLine() {
                m_sw.WriteLine();
            }
            public void Close(){
                m_sw?.Flush();
                m_sw?.Close();
            }
            #endregion

            #region static
            public static IFileWriter OpenFile4Rewrite(string fileName, bool fromResources)
            {
                var writer = new FileWriter(fileName, fromResources);
                writer.OpenRewrite();
                return writer;
            }
            public static IFileWriter OpenFile4Append(string fileName, bool fromResources)
            {
                var writer = new FileWriter(fileName, fromResources);
                writer.OpenAppend();
                return writer;
            }
            #endregion

        }

    }

}