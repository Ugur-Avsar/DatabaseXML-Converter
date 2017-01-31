using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Data;


namespace DatabaseXMLConverter
{
    class XMLConverter
    {

        public static XElement ParseDatabase(string database, List<DataTable> tables, DatasetStyles style)
        {
            XElement db = new XElement(DatabaseConnection.Database,
                from table in DatabaseConnection.GetTables()
                select new XElement(table.TableName, new XAttribute("Datatypes", getTypes(table.Columns)),
                from row in table.Rows.Cast<DataRow>()
                select new XElement((style == DatasetStyles.CUT ? (table.TableName.Substring(0, table.TableName.Length - 1)) : (style == DatasetStyles.ELEMENT ? (table.TableName + "_element") : (table.TableName + "_element"))),
                    from attr in table.Columns.Cast<DataColumn>()
                    select new XAttribute(attr.ColumnName, row[attr.ColumnName])
                    )));
            return db;
        }

        private static string getTypes(DataColumnCollection c)
        {
            string s = "";
            c.Cast<DataColumn>().ToList<DataColumn>().ForEach(x => s += ("" + x.DataType).Substring(7) + ";");
            return s;
        }

    }
}