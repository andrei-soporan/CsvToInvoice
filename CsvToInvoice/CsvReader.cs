using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;


namespace CsvToInvoice
{
    public class CsvReader
    {

        public CsvReader( string csvFile )
        {
            m_path = csvFile;

            Init();
        }

        ~CsvReader()
        {
            if (m_csvParser != null)
                m_csvParser.Close();
        }

        private bool Init()
        {
            //m_csvParser = new TextFieldParser(m_path, System.Text.Encoding.GetEncoding(28591));
            m_csvParser = new TextFieldParser(m_path, System.Text.Encoding.UTF8);

            m_csvParser.CommentTokens = new string[] { "#" };
            m_csvParser.SetDelimiters(new string[] { "," });
            //m_csvParser.SetDelimiters( new String(new Char() { vbTab}));
            m_csvParser.HasFieldsEnclosedInQuotes = true;

            // read the row with the column names
            m_HeaderLine = m_csvParser.ReadLine();

            return true;
        }

        public bool GetNextLine( ref string[] csvFields )
        {
            if (m_csvParser.EndOfData)
                return false;
            try
            {
                csvFields = m_csvParser.ReadFields();
            }
            catch( MalformedLineException e)
            {
                Trace.TraceError(e.Message);
            }

            return true;
        }
        
        public string GetCsvHeader()
        {
            return m_HeaderLine;
        }

        public long GetLineNumber()
        {
            return m_csvParser.LineNumber;
        }

        private string m_path;

        private TextFieldParser m_csvParser;

        private string m_HeaderLine;
    }
}
