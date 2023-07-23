using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlToCsv.Helpers
{
    public class CsvHandler
    {
        public List<T> CsvReader<T>(string file)
        {
            string template = file;

            using (var reader = new StreamReader(template))

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<T>().ToList();

                return records;
            }
        }

        public void CsvWriter<T>(List<T> csvList, string file)
        {
            string path = file;

            using (var writer = new StreamWriter(path))
            {
                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteRecords(csvList);
                }
            }
        }
    }
}
