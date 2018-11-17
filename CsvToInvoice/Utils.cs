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


        public static string GetCountyShortcut(string strCountyName)
        {
            string strCountyShortcut = "";

            switch (strCountyName.ToUpper()) {
                case "ALBA" :
                    strCountyShortcut = "AB";
                    break;
                case "ARAD" :
                    strCountyShortcut = "AR";
                    break;
                case "ARGES" :
                    strCountyShortcut = "AG";
                    break;
                case "BACAU":
                    strCountyShortcut = "BC";
                    break;
                case "BIHOR":
                    strCountyShortcut = "BH";
                    break;
                case "BISTRITA-NASAUD":
                    strCountyShortcut = "BN";
                    break;
                case "BOTOSANI":
                    strCountyShortcut = "BT";
                    break;
                case "BRAILA":
                    strCountyShortcut = "BR";
                    break;
                case "BRASOV":
                    strCountyShortcut = "BV";
                    break;
                case "BUZAU":
                    strCountyShortcut = "BZ";
                    break;
                case "CALARASI":
                    strCountyShortcut = "CL";
                    break;
                case "CARAS-SEVERIN":
                    strCountyShortcut = "CS";
                    break;
                case "CLUJ":
                    strCountyShortcut = "CJ";
                    break;
                case "CONSTANTA":
                    strCountyShortcut = "CT";
                    break;
                case "COVASNA":
                    strCountyShortcut = "CV";
                    break;
                case "DIMBOVITA":
                    strCountyShortcut = "DB";
                    break;
                case "DAMBOVITA":
                    strCountyShortcut = "DB";
                    break;
                case "DOLJ":
                    strCountyShortcut = "DJ";
                    break;
                case "GALATI":
                    strCountyShortcut = "GL";
                    break;
                case "GIURGIU":
                    strCountyShortcut = "GR";
                    break;
                case "GORJ":
                    strCountyShortcut = "GJ";
                    break;
                case "HARGHITA":
                    strCountyShortcut = "HR";
                    break;
                case "HUNEDOARA":
                    strCountyShortcut = "HD";
                    break;
                case "IALOMITA":
                    strCountyShortcut = "IL";
                    break;
                case "IASI":
                    strCountyShortcut = "IS";
                    break;
                case "ILFOV":
                    strCountyShortcut = "IF";
                    break;
                case "MARAMURES":
                    strCountyShortcut = "MM";
                    break;
                case "MEHEDINTI":
                    strCountyShortcut = "MH";
                    break;
                case "MURES":
                    strCountyShortcut = "MS";
                    break;
                case "NEAMT":
                    strCountyShortcut = "NT";
                    break;
                case "OLT":
                    strCountyShortcut = "OT";
                    break;
                case "PRAHOVA":
                    strCountyShortcut = "PH";
                    break;
                case "SALAJ":
                    strCountyShortcut = "SJ";
                    break;
                case "SATU MARE":
                    strCountyShortcut = "SM";
                    break;
                case "SIBIU":
                    strCountyShortcut = "SB";
                    break;
                case "SUCEAVA":
                    strCountyShortcut = "SV";
                    break;
                case "TELEORMAN":
                    strCountyShortcut = "TL";
                    break;
                case "TIMIS":
                    strCountyShortcut = "TM";
                    break;
                case "TULCEA":
                    strCountyShortcut = "TL";
                    break;
                case "VASLUI":
                    strCountyShortcut = "VS";
                    break;
                case "VILCEA":
                    strCountyShortcut = "VL";
                    break;
                case "VALCEA":
                    strCountyShortcut = "VL";
                    break;
                case "VRANCEA":
                    strCountyShortcut = "VN";
                    break;
                case "BUCURESTI":
                    strCountyShortcut = "B";
                    break;
                default:
                    strCountyShortcut = strCountyName;
                    break;
            }

            return strCountyShortcut;
        }

    }
}
