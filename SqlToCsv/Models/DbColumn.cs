using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlToCsv.Models
{
    public class DbColumn
    {
        public string COLUMN_NAME { get; set; }
        public string DATA_TYPE { get; set; }
        public string IS_NULLABLE { get; set; }
        public double NUMERIC_PRECISION { get; set; }
        public double NUMERIC_PRECISION_RADIX { get; set; }
        public double NUMERIC_SCALE { get; set; }
        public double ORDINAL_POSITION { get; set; }
        public string TABLE_CATALOG { get; set; }
        public string TABLE_NAME { get; set; }
        public string TABLE_SCHEMA { get; set; }
    }
}
