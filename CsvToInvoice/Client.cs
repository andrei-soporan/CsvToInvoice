using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToInvoice
{
    class Client
    {
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
    }
}
