using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Foundations.Extensions
{
    public static class DataSetExtension
    {
        public static string ToCsv(this DataSet dataSet)
        {
            if (dataSet.Tables.Count == 0)
            {
                return null;
            }

            return dataSet.Tables[0].ToCsv();
        }

        public static void ToCsv(this DataSet dataSet, string filePath)
        {
            if (dataSet.Tables.Count == 0)
            {
                File.WriteAllText(filePath, "");
            }
            dataSet.Tables[0].ToCsv(filePath);
        }
    }

    public static class DataTableExtension
    {
        public static string ToCsv(this DataTable dataTable)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            ToCsv(dataTable, sw);
            return sb.ToString();
        }

        public static void ToCsv(this DataTable dataTable, string filePath)
        {
            StreamWriter sw = new StreamWriter(filePath, false, Encoding.Unicode);
            ToCsv(dataTable, sw);
        }

        public static void ToCsv(this DataTable dataTable, TextWriter sw)
        {
            string delimiter = ";";

            //headers  
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                sw.Write(dataTable.Columns[i]);
                if (i < dataTable.Columns.Count - 1)
                {
                    sw.Write(delimiter);
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(delimiter))
                        {
                            sw.Write($@"""{value}""");
                        }
                        else
                        {
                            sw.Write(value);
                        }
                    }
                    if (i < dataTable.Columns.Count - 1)
                    {
                        sw.Write(delimiter);
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
    }
}
