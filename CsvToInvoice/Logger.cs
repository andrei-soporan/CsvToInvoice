using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;


namespace CsvToInvoice
{

    public class RollOverTextWriter : System.Diagnostics.TraceListener
    {
        string _fileName;
        System.DateTime _currentDate;
        System.IO.StreamWriter _traceWriter;
        int _maxTraceFileSize = 50000000;

        public RollOverTextWriter(string fileName)
        {

            string strfileName = ConfigurationManager.AppSettings["TraceFile"];

            if ( !String.IsNullOrEmpty(strfileName))
            {
                _fileName = strfileName;
            }
            else
            {
                _fileName = fileName;
            }

            string strMaxTraceFileSize = ConfigurationManager.AppSettings["TraceFileMaxSize"];
            if (!String.IsNullOrEmpty(strMaxTraceFileSize))
            {
                _maxTraceFileSize = Convert.ToInt32(strMaxTraceFileSize);
            }

            _traceWriter = new StreamWriter(generateFilename(), true, System.Text.Encoding.GetEncoding(28591));
            _traceWriter.AutoFlush = true;
        }

        public override void Write(string value)
        {
            checkRollover();
            _traceWriter.Write(value);
        }

        public override void WriteLine(string value)
        {
            checkRollover();
            _traceWriter.WriteLine(value);
        }

        private string generateFilename()
        {
            _currentDate = System.DateTime.Now;

            string strFullPath = Path.Combine(Path.GetDirectoryName(_fileName),
            Path.GetFileNameWithoutExtension(_fileName) + "_" +
            _currentDate.ToString("yyyy_mm_dd_H_mm_ss") + Path.GetExtension(_fileName));

            return strFullPath;
        }

        private void checkRollover()
        {
            long length = _traceWriter.BaseStream.Length;
            if (length >= _maxTraceFileSize)
            {
                _traceWriter.Close();
                _traceWriter = new StreamWriter(generateFilename(), true, System.Text.Encoding.GetEncoding(28591));
            }
        }

        public override void Flush() 
        {
            _traceWriter.Flush();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _traceWriter.Close();
            }
        }
    }
}
