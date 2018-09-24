using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToInvoice
{

    class InvoiceTable
    {
        protected readonly string DbfTableName;

        protected readonly string DbfPath;

        IList<Invoice> Invoices = new List<Invoice>();

        public InvoiceTable(string path, string dbfTableName)
        {
            DbfPath = path;
            DbfTableName = dbfTableName;
        }

        public void Add(Invoice invoice)
        {
            Invoices.Add(invoice);
        }

        public void TruncateTable()
        {
            string ConnectionString = Utils.GetConnectionString(DbfPath);

            using (OleDbConnection con = new OleDbConnection(ConnectionString))
            {
                //var sql = "delete from " + DbfTableName;
                var sql = "zap " + DbfTableName;

                OleDbCommand cmd = new OleDbCommand(sql, con);
                con.Open();

                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void Insert(Invoice invoice)
        {
            string ConnectionString = Utils.GetConnectionString(DbfPath);

            using (OleDbConnection con = new OleDbConnection(ConnectionString))
            {
                var sql = String.Format("insert into {0} ( NR_IESIRE,[COD],[DATA],[SCADENT],[TIP],[TVAI],[DEN_TIP],[GESTIUNE],[DEN_GEST],[COD_ART],[DEN_ART],[TVA_ART],[UM],[CANTITATE],[VALOARE],[TVA],[CONT],[GRUPA],[N_NULLFLAG]) values ( ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                    DbfTableName);

                OleDbCommand cmd = new OleDbCommand(sql, con);

                cmd.Parameters.Add(new OleDbParameter("parmNR_IESIRE", invoice.NrIesire));
                cmd.Parameters.Add(new OleDbParameter("parmCOD", invoice.Cod));
                cmd.Parameters.Add(new OleDbParameter("parmDATA", invoice.Data));
                cmd.Parameters.Add(new OleDbParameter("parmSCADENT", invoice.Scadent));
                cmd.Parameters.Add(new OleDbParameter("parmTIP", invoice.Tip));
                cmd.Parameters.Add(new OleDbParameter("parmTVAI", invoice.TvaI));
                cmd.Parameters.Add(new OleDbParameter("parmDEN_TIP", invoice.DenTip));
                cmd.Parameters.Add(new OleDbParameter("parmGESTIUNE", invoice.Gestiune));
                cmd.Parameters.Add(new OleDbParameter("parmDEN_GEST", invoice.DenGest));
                cmd.Parameters.Add(new OleDbParameter("parmCOD_ART", invoice.CodArt));
                cmd.Parameters.Add(new OleDbParameter("parmDEN_ART", invoice.DenArt));
                cmd.Parameters.Add(new OleDbParameter("parmTVA_ART", invoice.TvaArt));
                cmd.Parameters.Add(new OleDbParameter("parmUM", invoice.Um));
                cmd.Parameters.Add(new OleDbParameter("parmCANTITATE", invoice.Cantitate));
                cmd.Parameters.Add(new OleDbParameter("parmVALOARE", invoice.Valoare));
                cmd.Parameters.Add(new OleDbParameter("parmTVA", invoice.Tva));
                cmd.Parameters.Add(new OleDbParameter("parmCONT", invoice.Cont));
                cmd.Parameters.Add(new OleDbParameter("parmGRUPA", invoice.Grupa));
                cmd.Parameters.Add(new OleDbParameter("parmN_NULLFLAG", invoice.NullFlag));

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void InsertAll()
        {
            string ConnectionString = Utils.GetConnectionString(DbfPath);

            using (OleDbConnection con = new OleDbConnection(ConnectionString))
            {
                con.Open();

                foreach (Invoice invoice in Invoices)
                {
                    Insert(con, invoice);
                }

                con.Close();
            }
        }


        protected void Insert(OleDbConnection con, Invoice invoice)
        {

            var sql = String.Format("insert into {0} ( NR_IESIRE,[COD],[DATA],[SCADENT],[TIP],[TVAI],[DEN_TIP],[GESTIUNE],[DEN_GEST],[COD_ART],[DEN_ART],[TVA_ART],[UM],[CANTITATE],[VALOARE],[TVA],[CONT],[GRUPA],[N_NULLFLAG]) values ( ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                DbfTableName);

            OleDbCommand cmd = new OleDbCommand(sql, con);

            cmd.Parameters.Add(new OleDbParameter("parmNR_IESIRE", invoice.NrIesire));
            cmd.Parameters.Add(new OleDbParameter("parmCOD", invoice.Cod));
            cmd.Parameters.Add(new OleDbParameter("parmDATA", invoice.Data));
            cmd.Parameters.Add(new OleDbParameter("parmSCADENT", invoice.Scadent));
            cmd.Parameters.Add(new OleDbParameter("parmTIP", invoice.Tip));
            cmd.Parameters.Add(new OleDbParameter("parmTVAI", invoice.TvaI));
            cmd.Parameters.Add(new OleDbParameter("parmDEN_TIP", invoice.DenTip));
            cmd.Parameters.Add(new OleDbParameter("parmGESTIUNE", invoice.Gestiune));
            cmd.Parameters.Add(new OleDbParameter("parmDEN_GEST", invoice.DenGest));
            cmd.Parameters.Add(new OleDbParameter("parmCOD_ART", invoice.CodArt));
            cmd.Parameters.Add(new OleDbParameter("parmDEN_ART", invoice.DenArt));
            cmd.Parameters.Add(new OleDbParameter("parmTVA_ART", invoice.TvaArt));
            cmd.Parameters.Add(new OleDbParameter("parmUM", invoice.Um));
            cmd.Parameters.Add(new OleDbParameter("parmCANTITATE", invoice.Cantitate));
            cmd.Parameters.Add(new OleDbParameter("parmVALOARE", invoice.Valoare));
            cmd.Parameters.Add(new OleDbParameter("parmTVA", invoice.Tva));
            cmd.Parameters.Add(new OleDbParameter("parmCONT", invoice.Cont));
            cmd.Parameters.Add(new OleDbParameter("parmGRUPA", invoice.Grupa));
            cmd.Parameters.Add(new OleDbParameter("parmN_NULLFLAG", invoice.NullFlag));

            cmd.ExecuteNonQuery();

        }
    }

}

