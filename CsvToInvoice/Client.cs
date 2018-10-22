using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToInvoice
{
    class Client
    {

        public static int NULL_INT = 0;

        public static double NULL_DOUBLE = 0.0;

        public static string NULL_STRING = "";

        // public static DateTime NULL_DATE;

        public Client()
        {

        }

        public Client(string Cod, string Denumire)
        {
            this.Cod = Cod;
            this.Denumire = Denumire;
        }

        public string Cod { get; set; }

        public string Denumire { get; set; }

        public string CodFiscal { get; set; }

        public string RegCom { get; set; }

        public string Analitic { get; set; }

        public string Adresa { get; set; }

        public int NullFlag { get; set; }

    }
}
