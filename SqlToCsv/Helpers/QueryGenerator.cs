using SqlToCsv.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlToCsv.Helpers
{
    public class QueryGenerator
    {

        Dictionary<string, string> sqlDt = new Dictionary<string, string>();
        public QueryGenerator()
        {
            sqlDt.Add("bigint", "long");
            sqlDt.Add("binary", "byte[]");
            sqlDt.Add("bit", "bool");
            sqlDt.Add("char", "string");
            sqlDt.Add("date", "DateTime");
            sqlDt.Add("datetime", "DateTime");
            sqlDt.Add("datetime2", "DateTime");
            sqlDt.Add("datetimeoffset", "DateTimedffset");
            sqlDt.Add("decimal", "decimal");
            sqlDt.Add("Filestream", "byte[]");
            sqlDt.Add("Float", "double");
            sqlDt.Add("geography", "Microsoft.SqlServer.Types.SqlGeography");
            sqlDt.Add("geometry", "Microsoft.SqlServer.Types.SqlGeometry");
            sqlDt.Add("hierarchyid", "Microsoft.SqlServer.Types.SqlHierarchyId");
            sqlDt.Add("image", "byte[]");
            sqlDt.Add("int", "int");
            sqlDt.Add("money", "decimal");
            sqlDt.Add("nchar", "string");
            sqlDt.Add("ntext", "string");
            sqlDt.Add("numeric", "decimal");
            sqlDt.Add("nvarchar", "string");
            sqlDt.Add("real", "Single");
            sqlDt.Add("rowversion", "byte[]");
            sqlDt.Add("smalldatetime", "DateTime");
            sqlDt.Add("smallint", "short");
            sqlDt.Add("smallmoney", "decimal");
            sqlDt.Add("sql_variant", "object");
            sqlDt.Add("text", "string");
            sqlDt.Add("time", "TimeSpan");
            sqlDt.Add("timestamp", "byte[]");
            sqlDt.Add("tinyint", "byte");
            sqlDt.Add("uniqueidentifier", "Guid");
            sqlDt.Add("varbinary", "byte[]");
            sqlDt.Add("varchar", "string");
            sqlDt.Add("xml", "string");
            sqlDt.Add("else", "object");
        }
        public string SelectQuery(string table)
        {
            StringBuilder query = new StringBuilder();

            query.Append("\nCREATE PROCEDURE spGetAll_" + table + " \nAS\nBEGIN\n");

            query.Append("\tSELECT * FROM [" + table + "];");

            query.Append("\nEND\nGO\n");

            query.Append("\n-- =============================================\n");

            return query.ToString();
        }

        public string SelectByIdQuery(string table, List<DbColumn> columns)
        {
            StringBuilder query = new StringBuilder();

            var col = columns.FirstOrDefault(x => x.COLUMN_NAME.ToLower().Trim().Contains("id") && x.ORDINAL_POSITION == 1);

            if (col != null)
            {
                var dt = col.DATA_TYPE.ToLower().Trim().Equals("nvarchar") ? "nvarchar(MAX)" : col.DATA_TYPE;

                query.Append("\nCREATE PROCEDURE spGet_" + table + " \n@Id " + dt + " \nAS\nBEGIN\n");

                query.Append("\tSELECT * FROM [" + table + "] WHERE " + col.COLUMN_NAME + " = @Id;");
            }
            else
            {
                query.Append("CREATE PROCEDURE Get" + table + " \nAS\nBEGIN");

                query.Append("\tSELECT * FROM " + table + ";");
            }

            query.Append("\nEND\nGO\n");
            
            query.Append("\n-- =============================================\n");

            return query.ToString();
        }

        public string InsertQuery(string table, List<DbColumn> columns)
        {
            StringBuilder query = new StringBuilder();

            query.Append("\nCREATE PROCEDURE spInsert_" + table + "\n");
           
            var count = 1;
            
            foreach (var col in columns)
            {
                var dt = col.DATA_TYPE.ToLower().Trim().Equals("nvarchar") ? "nvarchar(MAX)" : col.DATA_TYPE;
               
                if (count == columns.Count)
                {
                    query.Append("\t\t@" + col.COLUMN_NAME + " as " + dt);
                }
                else
                {
                    query.Append("\t\t@" + col.COLUMN_NAME + " as " + dt + ",\n");
                }

                count++;
            }

            query.Append("\nAS\nBEGIN\n");

            query.Append("\tINSERT INTO [" + table + "] (\n");

            count = 1;

            foreach (var col in columns)
            {
                if (count == columns.Count)
                {
                    query.Append("\t\t[" + col.COLUMN_NAME + "])\n");
                }
                else
                {
                    query.Append("\t\t[" + col.COLUMN_NAME + "],\n");
                }
                count++;
            }

            query.Append("\n\t\tVALUES (\n");

            count = 1;

            foreach (var col in columns)
            {
                if (count == columns.Count)
                {
                    query.Append("\t\t@" + col.COLUMN_NAME + ")");
                }
                else
                {
                    query.Append("\t\t@" + col.COLUMN_NAME + ",\n");
                }

                count++;
            }

            query.Append("\nEND\nGO\n");

            query.Append("\n-- =============================================\n");
            
            return query.ToString();
        }

        public string UpdateQuery(string table, List<DbColumn> columns)
        {
            StringBuilder query = new StringBuilder();

            query.Append("\nCREATE PROCEDURE spUpdate_" + table + "\n");

            var count = 1;

            foreach (var col in columns)
            {
                var dt = col.DATA_TYPE.ToLower().Trim().Equals("nvarchar") ? "nvarchar(MAX)" : col.DATA_TYPE;

                if (count == columns.Count)
                {
                    query.Append("\t\t@" + col.COLUMN_NAME + " as " + dt);
                }
                else
                {
                    query.Append("\t\t@" + col.COLUMN_NAME + " as " + dt + ",\n");
                }

                count++;
            }

            query.Append("\nAS\nBEGIN\n");

            query.Append("\tUPDATE [" + table + "] SET\n");

            count = 1;

            foreach (var col in columns)
            {
                if (count == columns.Count)
                {
                    query.Append("\t\t[" + col.COLUMN_NAME + "] = @" + col.COLUMN_NAME + "\n");
                }
                else
                {
                    query.Append("\t\t[" + col.COLUMN_NAME + "] = @" + col.COLUMN_NAME + ",\n");
                }
                count++;
            }

            var idCol = columns.FirstOrDefault(x => x.COLUMN_NAME.ToLower().Trim().Contains("id") && x.ORDINAL_POSITION == 1);

            query.Append("\t\tWHERE [" + idCol.COLUMN_NAME + "] = @" + idCol.COLUMN_NAME);


            query.Append("\nEND\nGO\n");

            query.Append("\n-- =============================================\n");

            return query.ToString();
        }

        public string DeleteQuery(string table, List<DbColumn> columns)
        {
            StringBuilder query = new StringBuilder();

            var col = columns.FirstOrDefault(x => x.COLUMN_NAME.ToLower().Trim().Contains("id") && x.ORDINAL_POSITION == 1);

            if (col != null)
            {
                var dt = col.DATA_TYPE.ToLower().Trim().Equals("nvarchar") ? "nvarchar(MAX)" : col.DATA_TYPE;

                query.Append("\nCREATE PROCEDURE spDelete_" + table + " \n@Id " + dt + " \nAS\nBEGIN\n");

                query.Append("DELETE FROM [" + table + "] " +
                    "\t\t[IsActive] = 0\n" +
                " WHERE " + col.COLUMN_NAME + " = @Id;");
            }

            query.Append("\nEND\nGO\n");

            query.Append("\n-- =============================================\n");

            return query.ToString();
        }


        public string ClassQuery(string table, List<DbColumn> columns)
        {
            StringBuilder query = new StringBuilder();

            query.Append("\npublic Class " + table + "\n{\n");

            foreach (var col in columns)
            {
                sqlDt.TryGetValue(col.DATA_TYPE.Trim().ToLower(),out string dt);

                query.Append("\tpublic " + dt + " " + col.COLUMN_NAME + " { get; set; } \n");
            }
            query.Append("}");

            return query.ToString();
        }


    }
}
