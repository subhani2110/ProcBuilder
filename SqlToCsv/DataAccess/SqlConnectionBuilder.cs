using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;
using SqlToCsv.Models;

namespace SqlToCsv.DataAccess
{
    public class SqlConnectionBuilder
    {
        public async Task<List<string>> GetServers()
        {
            var list = new List<string>();

            try
            {
                var rows = await Task.Run(() => SqlDataSourceEnumerator.Instance.GetDataSources().Rows);

                foreach (DataRow row in rows)
                {
                    list.Add(row.ItemArray[0] + @"\" + row.ItemArray[1]);
                }
            }
            catch { }

            return list;
        }

        public List<string> GetDbs(string server)
        {
            var list = new List<string>();

            var sqlConnection = new SqlConnectionStringBuilder
            {
                DataSource = server,
                IntegratedSecurity = true
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

            return list;
        }

        public List<string> GetTables(string server, string db)
        {
            var list = new List<string>();

            var sqlConnection = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = db,
                IntegratedSecurity = true
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

        public async Task<List<DbColumn>> GetColumns(string server, string db, string table)
        {
            var list = new List<DbColumn>();

            var sqlConnection = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = db,
                IntegratedSecurity = true
            };
            var sql = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'" + table + "'";
            try
            {
                using (var connection = new SqlConnection(sqlConnection.ConnectionString))
                {
                    var result = await connection.QueryAsync<DbColumn>(sql);

                    list = result.ToList();
                }
            }
            catch { }

            return list;
        }
    }
}
