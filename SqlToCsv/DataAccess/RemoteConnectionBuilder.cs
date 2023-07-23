using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlToCsv.DataAccess
{
    public class RemoteConnectionBuilder
    {
        public List<string> GetDbs(string server, string userId, string password)
        {
            var list = new List<string>();

            var sqlConnection = new SqlConnectionStringBuilder
            {
                DataSource = server,
                UserID=userId,
                Password = password
            };

            try
            {
                using (SqlConnection con = new SqlConnection(sqlConnection.ConnectionString))
                {
                    con.Open();
                    var rows = con.GetSchema("Databases").Rows;
                    foreach (DataRow row in rows)
                    {
                        list.Add(row.ItemArray[0].ToString());
                    }
                }
            }
            catch { }

            return list.OrderBy(q => q).ToList();
        }

        public List<string> GetTables(string server, string userId, string password, string db)
        {
            var list = new List<string>();

            var sqlConnection = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = db,
                UserID = userId,
                Password = password
            };

            try
            {
                using (SqlConnection con = new SqlConnection(sqlConnection.ConnectionString))
                {
                    con.Open();
                    var rows = con.GetSchema("Tables").Rows;

                    foreach (DataRow row in rows)
                    {
                        list.Add(row.ItemArray[2].ToString());
                    }
                }
            }
            catch { }

            return list;
        }
    }
}
