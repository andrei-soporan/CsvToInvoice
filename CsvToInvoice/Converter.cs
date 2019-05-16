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

        ClientTable clientTable;

        protected int ClientId = 0;

        DateTime minDate = DateTime.MaxValue;

        DateTime maxDate = DateTime.MinValue;


        public Converter()
        {
            InCsvFile = Utils.GetInCsvFileName();
            ClientTableName = Utils.GetClientTableName();
            InvoiceTableName = Utils.GetInvoiceTableName();
            ClientId = Utils.GetClientStartCode();
        }

        public void Run()
        {
            try
            {
                Trace.TraceInformation("Start import ...\n\n");

                DateTime startDate = DateTime.Now;

                CsvReader csvReader = new CsvReader(Utils.GetDataPath() + "\\" + InCsvFile);


                string outClientTableName = CreateClientTableFromTemplate();
                clientTable = new ClientTable(Utils.GetOutputPath(), outClientTableName);


                string outInvoiceTableName = CreateInvoiceTableFromTemplate();
                InvoiceTable invoiceTable = new InvoiceTable(Utils.GetOutputPath(), outInvoiceTableName);


                string[] fields = new string[20];

                long TotalErrors = 0, TotalEntries = 0;

                while (csvReader.GetNextLine(ref fields))
                {
                    try
                    {
                        Client client = CreateClientFromCsv(fields);
                        clientTable.Add(client);

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

                Trace.TraceInformation("\n\nStart inserare in tabela de clienti: {0}", DateTime.Now);
                int nClientCount = clientTable.InsertAll();
                RenameOutputTable(Utils.GetOutputPath(), outClientTableName, minDate, maxDate);
                Trace.TraceInformation("Import clienti finalizat la: {0}\n\n", DateTime.Now);


                Trace.TraceInformation("\n\nStart inserare in tabela de facturi: {0}", DateTime.Now);
                int nInvoiceItems = invoiceTable.InsertAll();
                RenameOutputTable(Utils.GetOutputPath(), outInvoiceTableName, minDate, maxDate);
                Trace.TraceInformation("Import facturi finalizat la: {0} \n\n", DateTime.Now);


                DateTime endDate = DateTime.Now;
                Trace.TraceInformation(GetStats(startDate, endDate, TotalEntries, nClientCount, nInvoiceItems, invoiceTable.GetInvoiceCount(), TotalErrors));
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

            string NumeClient = Utils.RemoveDiacritics(fields[0]).ToUpper();

            invoice.Cod = clientTable.GetClientByName(NumeClient).Cod;

            string strData = fields[2];
            invoice.Data = DateTime.Parse(strData);
            invoice.Scadent = DateTime.Parse(strData);

            if ( minDate > invoice.Data)
            {
                minDate = invoice.Data;
            }

            if (maxDate < invoice.Data)
            {
                maxDate = invoice.Data;
            }

            invoice.Tip = "";

            invoice.TvaI = 0;

            invoice.DenTip = NEDEFINIT;

            invoice.Gestiune = "";

            invoice.DenGest = "";

            invoice.CodArt = "";

            invoice.DenArt = fields[3];

            invoice.Um = BUC;

            invoice.Cantitate = Double.Parse(fields[4]);

            invoice.Valoare = Double.Parse(fields[5]);
            invoice.Valoare = invoice.Valoare * invoice.Cantitate;

            invoice.Tva = Double.Parse(fields[7]);

            invoice.TvaArt = (int)Math.Round(invoice.Tva != 0 ? (invoice.Tva*100/invoice.Valoare) : 0);

            invoice.Cont = GetCont(invoice);

            invoice.Grupa = "";

            invoice.NullFlag = 0;

            return invoice;

        }


        public Client CreateClientFromCsv(string[] fields)
        {
            string numeClient = Utils.RemoveDiacritics(fields[0]);

            Client client;

            try
            {
                client = clientTable.GetClientByName(numeClient);

                return client;
            }
            catch (SystemException ex)
            {
                int t = 0;
            }
                

            client = new Client();

            client.Cod = (ClientId++).ToString("D5");

            client.Denumire = numeClient.ToUpper();

            client.Analitic = "4111." + client.Cod;

            client.Adresa = Utils.RemoveDiacritics(fields[11]);
            client.Adresa += ", " + Utils.RemoveDiacritics(fields[12]);
            client.Judet = Utils.GetCountyShortcut(Utils.RemoveDiacritics(fields[13]));

            client.NullFlag = 0;

            return client;

        }


        //protected string GetCont(string productName)
        protected string GetCont(Invoice invoice)
        {
            string cont;
            if (invoice.DenArt.Equals("Taxa Transport", StringComparison.OrdinalIgnoreCase))
            {
                cont = "704";
            }
            else if (invoice.DenArt.Equals("Discount Valoric"))
            {
                cont = "767";
            }
            else if ( invoice.TvaArt == 5)
            {
                cont = "707.02";
            }
            else
            {
                cont = "707.01";
            }

            return cont;
        }

        protected string GetStats(DateTime startDate, DateTime endDate, long TotalEntries, long clientCount, long invoiceItems, long invoiceCount, long TotalErrors)
        {
            String strMessage = String.Format("\n Import start: {0}\n Nr. total de intrati in *.csv: {1}\n Nr. total de clienti: {2}\n Nr. total de facturi: {3}\n Nr. total de linii in facturi: {4}\n Nr. total de erori: {5}\n Import finalizat la: {6}",
                startDate, TotalEntries, clientCount, invoiceCount, invoiceItems, TotalErrors, endDate);

            return strMessage;
        }

        protected string CreateInvoiceTableFromTemplate()
        {
            string outInvoiceTableName = Utils.GetInvoiceTableName();
            string templateInvoicePath = Utils.GetDBTemplatePath() + Utils.GetInvoiceTableName();
            string outInvoicePath = Utils.GetOutputPath() + outInvoiceTableName;

            Directory.CreateDirectory(Path.GetDirectoryName(outInvoicePath));

            File.Copy(templateInvoicePath, outInvoicePath);

            return outInvoiceTableName;
        }

        protected string CreateClientTableFromTemplate()
        {
            string outClientTableName = Utils.GetClientTableName();

            string templateClientPath = Utils.GetDBTemplatePath() + Utils.GetClientTableName();
            string outClientPath = Utils.GetOutputPath() + outClientTableName;

            Directory.CreateDirectory(Path.GetDirectoryName(outClientPath));

            File.Copy(templateClientPath, outClientPath);

            return outClientTableName;
        }

        protected void RenameOutputTable(string outputPath, string tableName, DateTime startDate, DateTime endDate)
        {
            string newTableName = Path.GetFileNameWithoutExtension(tableName) + "_" + startDate.ToString("dd-MM-yyyy") + "_" + endDate.ToString("dd-MM-yyyy") + Path.GetExtension(Utils.GetClientTableName());
            System.IO.File.Move(outputPath + tableName, outputPath + newTableName);
        }

    }
}
