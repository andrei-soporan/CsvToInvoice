using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToInvoice
{
    class Invoice
    {
        public Invoice()
        {

        }

        public string NrIesire { get; set; }

        public string Cod { get; set; }

        public DateTime Data { get; set; }

        public DateTime Scadent { get; set; }

        public string Tip { get; set; }

        public int TvaI { get; set; }

        public string DenTip { get; set; }

        public string Gestiune { get; set; }

        public string DenGest { get; set; }

        public string CodArt { get; set; }

        public string DenArt { get; set; }

        public int TvaArt { get; set; }

        public string Um { get; set; }

        public double Cantitate { get; set; }

        public double Valoare { get; set; }

        public double Tva { get; set; }

        public string Cont { get; set; }

        public string Grupa { get; set; }

        public int NullFlag { get; set; }

    }
}
