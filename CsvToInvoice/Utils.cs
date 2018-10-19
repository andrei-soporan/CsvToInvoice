using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToInvoice
{
    class Utils
    {

        public static string GetConnectionString(string pathToDbf)
        {
            string constr = "Provider=VFPOLEDB; Data Source=" + pathToDbf +";Extended Properties=dBASE IV;";

            return constr;
        }


        public static string GetAppPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static string GetDataPath()
        {
            string DataFilePath = ConfigurationManager.AppSettings["DataFilePath"];
            
            if (!Path.IsPathRooted(DataFilePath))
            {
                DataFilePath = Utils.GetAppPath() + DataFilePath + "\\";
            }

            return DataFilePath;
        }

        public static string GetDBTemplatePath()
        {
            string DBTemplateFolder = ConfigurationManager.AppSettings["DBTemplateFolder"];

            if (!Path.IsPathRooted(DBTemplateFolder))
            {
                DBTemplateFolder = Utils.GetDataPath() + DBTemplateFolder + "\\";
            }

            return DBTemplateFolder;
        }

        public static string GetOutputPath()
        {
            string OutputFolder = ConfigurationManager.AppSettings["OutputFolder"];

            if (!Path.IsPathRooted(OutputFolder))
            {
                OutputFolder = Utils.GetDataPath() + OutputFolder + "\\";
            }

            return OutputFolder;
        }

        public static string GetInCsvFileName()
        {
            string InCsvFileName = ConfigurationManager.AppSettings["InCsvFileName"];
            return InCsvFileName;
        }

        public static string GetClientTableName()
        {
            string ClientTableName = ConfigurationManager.AppSettings["ClientTableName"];
            return ClientTableName;
        }

        public static string GetInvoiceTableName()
        {
            string InvoiceTableName = ConfigurationManager.AppSettings["InvoiceTableName"];
            return InvoiceTableName;
        }

        public static int GetClientStartCode()
        {
            int startCode = 0;
            string strStartCode = ConfigurationManager.AppSettings["ClientStartCode"];

            if (!Int32.TryParse(strStartCode, out startCode))
            {
                startCode = -1;
            }

            return startCode;
        }

        public static string ArrayToString(string[] fields)
        {
            return string.Join(" ", fields);
        }

        public static String RemoveDiacritics(string text)
        {
            String normalized = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < normalized.Length; i++)
            {
                Char c = normalized[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString();
        }

        public static bool InitTextWriteListener()
        {
            RollOverTextWriter rollOverTextWitter = new RollOverTextWriter("Import.log");
            Trace.Listeners.Add(rollOverTextWitter);

            return true;
        }


    }
}
