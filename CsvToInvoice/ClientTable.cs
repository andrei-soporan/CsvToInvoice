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
            Client client = (Client)Clients[clientName.ToLower()];

            if (client == null)
                throw new SystemException("Clientul cu numele: " + clientName + " nu a fost gasit in tabela de clienti!");

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


    }
}
