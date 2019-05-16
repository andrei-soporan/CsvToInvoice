using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToInvoice
{
    class ClientTable
    {
        protected readonly string DbfTableName;

        protected readonly string DbfPath;

        Hashtable Clients;

        public ClientTable( string path, string dbfTableName)
        {
            DbfPath = path;
            DbfTableName = dbfTableName;

            Clients = new Hashtable();
        }

        public Client GetClientByName(string clientName)
        {
            string key = clientName.ToLower();

            Client client = (Client)Clients[key];

            if (client == null)
            {
                throw new SystemException("Clientul cu numele: " + clientName + " nu a fost gasit in tabela de clienti!");
            }

            return client;
        }

        public void ReadClientTable()
        {
            string ConnectionString = Utils.GetConnectionString(DbfPath);

            using (OleDbConnection con = new OleDbConnection(ConnectionString))
            {
                var sql = "select * from " + DbfTableName;

                OleDbCommand cmd = new OleDbCommand(sql, con);
                con.Open();
                DataSet ds = new DataSet(); ;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);

                DataTable clientTable = ds.Tables[0];

                foreach (DataRow row in clientTable.Rows)
                {
                    Client client = new Client();

                    client.Cod = row["COD"].ToString();
                    client.Denumire = row["DENUMIRE"].ToString().ToLower().TrimEnd();

                    Clients.Add(client.Denumire, client);
                }
            }
        }

        public void Add(Client client)
        {
            String key = client.Denumire.ToString().ToLower().TrimEnd();

            Client existingClient = (Client)Clients[key];

            if (existingClient != null)
                //throw new SystemException("Clientul cu numele: " + existingClient.Denumire + " exista in tabela de clienti!");
                return;

            Clients.Add(key, client);
        }


        public int InsertAll()
        {
            int nCount = 0;

            string ConnectionString = Utils.GetConnectionString(DbfPath);

            using (OleDbConnection con = new OleDbConnection(ConnectionString))
            {
                con.Open();

                foreach (DictionaryEntry s in Clients)
                {
                    Insert(con, (Client)s.Value);
                    ++nCount;
                }

                con.Close();
            }

            return nCount;
        }


        protected void Insert(OleDbConnection con, Client client)
        {

            var sql = String.Format("insert into {0} ( [COD],[DENUMIRE],[COD_FISCAL],[REG_COM],[ANALITIC],[ZS],[DISCOUNT],[ADRESA],[JUDET],[BANCA],[CONT_BANCA],[FILIALA],[DELEGAT],[BI_SERIE],[BI_NUMAR],[BI_POL],[MASINA],[INF_SUPL],[AGENT],[DEN_AGENT],[GRUPA],[TIP_TERT],[TARA],[TEL],[EMAIL],[IS_TVA],[BLOCAT]) values ( ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                DbfTableName);

            OleDbCommand cmd = new OleDbCommand(sql, con);

            cmd.Parameters.Add(new OleDbParameter("parmCOD", client.Cod));
            cmd.Parameters.Add(new OleDbParameter("parmDENUMIRE", client.Denumire));
            cmd.Parameters.Add(new OleDbParameter("parmCOD_FISCAL", client.CodFiscal));
            cmd.Parameters.Add(new OleDbParameter("parmREG_COM", client.RegCom));
            cmd.Parameters.Add(new OleDbParameter("parmANALITIC", client.Analitic));
            cmd.Parameters.Add(new OleDbParameter("parmZS", Client.NULL_INT));
            cmd.Parameters.Add(new OleDbParameter("parmDISCOUNT", Client.NULL_DOUBLE));
            cmd.Parameters.Add(new OleDbParameter("parmADRESA", client.Adresa));
            cmd.Parameters.Add(new OleDbParameter("parmJUDET", client.Judet));

            cmd.Parameters.Add(new OleDbParameter("parmBANCA", Client.NULL_STRING));
            cmd.Parameters.Add(new OleDbParameter("parmCONT_BANCA", Client.NULL_STRING));
            cmd.Parameters.Add(new OleDbParameter("parmFILIALA", Client.NULL_STRING));
            cmd.Parameters.Add(new OleDbParameter("parmDELEGAT", Client.NULL_STRING));
            cmd.Parameters.Add(new OleDbParameter("parmBI_SERIE", Client.NULL_STRING));
            cmd.Parameters.Add(new OleDbParameter("parmBI_NUMAR", Client.NULL_STRING));
            cmd.Parameters.Add(new OleDbParameter("parmBI_POL", Client.NULL_STRING));
            cmd.Parameters.Add(new OleDbParameter("parmMASINA", Client.NULL_STRING));
            cmd.Parameters.Add(new OleDbParameter("parmINF_SUPL", Client.NULL_STRING));
            cmd.Parameters.Add(new OleDbParameter("parmAGENT", Client.NULL_STRING));
            cmd.Parameters.Add(new OleDbParameter("parmDEN_AGENT", Client.NULL_STRING));
            cmd.Parameters.Add(new OleDbParameter("parmGRUPA", Client.NULL_STRING));
            cmd.Parameters.Add(new OleDbParameter("parmTIP_TERT", Client.NULL_STRING));
            cmd.Parameters.Add(new OleDbParameter("parmTARA", Client.NULL_STRING));
            cmd.Parameters.Add(new OleDbParameter("parmTEL", Client.NULL_STRING));
            cmd.Parameters.Add(new OleDbParameter("parmEMAIL", Client.NULL_STRING));
            cmd.Parameters.Add(new OleDbParameter("parmIS_TVA", Client.NULL_INT));
            cmd.Parameters.Add(new OleDbParameter("parmBLOCAT", Client.NULL_INT));

            // https://blogs.msdn.microsoft.com/selvar/2007/11/10/ole-db-resource-pooling/
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

    }
}
