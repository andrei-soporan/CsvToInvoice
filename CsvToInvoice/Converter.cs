using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToInvoice
{
    class Converter
    {
        private const string NEDEFINIT = "Nedefinit";

        private const string BUC = "BUC";

        protected readonly string InCsvFile;

        protected readonly string ClientTableName;

        protected readonly string InvoiceTableName;

        protected ClientTable clnTable;

        public Converter()
        {
            InCsvFile = Utils.GetInCsvFileName();
            ClientTableName = Utils.GetClientTableName();
            InvoiceTableName = Utils.GetInvoiceTableName();
        }

        public void Run()
        {
            try
            {
                Trace.TraceInformation("Start import ...\n\n");

                DateTime startDate = DateTime.Now;

                CsvReader csvReader = new CsvReader(Utils.GetDataPath() + "\\" + InCsvFile);

                clnTable = new ClientTable(Utils.GetDataPath(), ClientTableName);

                clnTable.ReadClientTable();

                string outInvoiceTableName = CreateInvoiceTableFromTemplate();

                InvoiceTable invoiceTable = new InvoiceTable(Utils.GetOutputPath(), outInvoiceTableName);

                //invoiceTable.TruncateTable();

                string[] fields = new string[20];

                long TotalErrors = 0, TotalEntries = 0;

                while (csvReader.GetNextLine(ref fields))
                {
                    try
                    {
                        Invoice invoice = CreateInvoiceFromCsv(fields);

                        invoiceTable.Add(invoice);

                        TotalEntries = csvReader.GetLineNumber() != -1 ? csvReader.GetLineNumber() : TotalEntries;
                    }
                    catch (SystemException ex)
                    {
                        Trace.TraceError("Intrarea cu numarul: {0}, si continut {1} din fisierul *csv nu a putut fi convertita!", TotalEntries, Utils.ArrayToString(fields));
                        TotalErrors++;

                        Trace.TraceError("Message :{0} ", ex.Message);
                    }
                }

                Trace.TraceInformation("\n\nStart inserare in tabela the facturi: {0}", DateTime.Now);

                invoiceTable.InsertAll();

                Trace.TraceInformation("Terminat de inserat in tabela the facturi: {0}\n\n", DateTime.Now);


                DateTime endDate = DateTime.Now;

                Trace.TraceInformation(GetStats(startDate, endDate, TotalEntries, TotalErrors));

            }
            catch (SystemException ex)
            {
                Trace.TraceError("Message :{0} ", ex.Message);
                Trace.TraceError("Stack trace :{0} ", ex.StackTrace);
            }

        }


        public Invoice CreateInvoiceFromCsv(string[] fields)
        {
            Invoice invoice = new Invoice();

            invoice.NrIesire = fields[1];

            string NumeClient = Utils.RemoveDiacritics(fields[0]);

            invoice.Cod = clnTable.GetClientByName(NumeClient).Cod;

            string strData = fields[2];
            invoice.Data = DateTime.Parse(strData);
            invoice.Scadent = DateTime.Parse(strData);

            invoice.Tip = "";

            invoice.TvaI = 0;

            invoice.DenTip = NEDEFINIT;

            invoice.Gestiune = "";

            invoice.DenGest = "";

            invoice.CodArt = "";

            invoice.DenArt = fields[3];

            invoice.TvaArt = 0;

            invoice.Um = BUC;

            invoice.Cantitate = Double.Parse(fields[6]);

            invoice.Valoare = Double.Parse(fields[7]);
            invoice.Valoare = invoice.Valoare * invoice.Cantitate;

            invoice.Tva = 0;

            invoice.Cont = GetCont(invoice.DenArt);

            invoice.Grupa = "";

            invoice.NullFlag = 0;

            return invoice;

        }

        protected string GetCont(string productName)
        {
            string cont;
            if (productName.Equals("Taxa Transport"))
            {
                cont = "704";
            }
            else if (productName.Equals("Discount Valoric"))
            {
                cont = "767";
            }
            else
            {
                cont = "707.01";
            }

            return cont;
        }

        protected string GetStats(DateTime startDate, DateTime endDate, long TotalEntries, long TotalErrors)
        {
            String strMessage = String.Format("\nImport start: {0}\nNr. total de intrati in *.csv: {1}\nNr. total de intrari importate: {2}\nNr. total de erori: {3}\nImport finalizat la: {4}",
                startDate, TotalEntries, TotalEntries - TotalErrors, TotalErrors, endDate);

            return strMessage;
        }

        protected string CreateInvoiceTableFromTemplate()
        {
            string outInvoiceTableName = Path.GetFileNameWithoutExtension(Utils.GetInvoiceTableName()) + "_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + Path.GetExtension(Utils.GetInvoiceTableName());

            string templateInvoicePath = Utils.GetDBTemplatePath() + Utils.GetInvoiceTableName();
            string outInvoicePath = Utils.GetOutputPath() + outInvoiceTableName;

            Directory.CreateDirectory(Path.GetDirectoryName(outInvoicePath));

            File.Copy(templateInvoicePath, outInvoicePath);

            return outInvoiceTableName;
        }
    }
}
