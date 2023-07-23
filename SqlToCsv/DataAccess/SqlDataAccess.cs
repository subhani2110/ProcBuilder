using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlToCsv.DataAccess
{
    public class SqlDataAccess
    {
        public async Task<List<dynamic>> LoadData(string server, string db, string table)
        {
            var list = new List<dynamic>();
            
            try
            {
                var sqlQuery = "Select * FROM " + table;

                using (var connection = new SqlConnection(CreateConnStr(server, db)))
                {
                    var result = await connection.QueryAsync(sqlQuery);

                    list = result.ToList();
                }

            }
            catch { }

            return list;
        }

        public async Task<List<dynamic>> LoadDataRemote(string server,string userId,string password, string db, string table)
        {
            var list = new List<dynamic>();

            try
            {
                var sqlQuery = "Select * FROM " + table;

                using (var connection = new SqlConnection(CreateConnStrRemote(server, userId, password, db))) 
                {
                    var result = await connection.QueryAsync(sqlQuery);

                    list = result.ToList();
                }

            }
            catch { }

            return list;
        }

        string CreateConnStr(string server, string db)
        {
            var sqlConnection = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = db,
                IntegratedSecurity = true
            };

            return sqlConnection.ConnectionString;
        }

        string CreateConnStrRemote(string server, string userId, string password, string db)
        {
            var sqlConnection = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = db,
                UserID = userId,
                Password = password
            };

            return sqlConnection.ConnectionString;
        }
    }
}
