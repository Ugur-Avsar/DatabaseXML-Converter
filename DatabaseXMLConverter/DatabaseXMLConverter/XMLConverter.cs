using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data;


namespace DatabaseXMLConverter
{
    class XMLConverter
    {
        public enum DatasetStyle
        {
            CUT, ELEMENT
        }

        public static XElement ParseDatabase(string database, List<DataTable> tables, DatasetStyle style)
        {
            XElement db = new XElement(DatabaseConnection.Database, from table in DatabaseConnection.GetTables()
                    select new XElement(table.TableName, 
                        from row in table.Rows.Cast<DataRow>()
                        where row.Table.TableName == table.TableName
                        select new XElement((style == DatasetStyle.CUT ? (table.TableName.Substring(0, table.TableName.Length - 1)) : (style == DatasetStyle.ELEMENT ? (table.TableName + "_element") : (table.TableName + "_element"))), 
                            from attr in table.Columns.Cast<DataColumn>()
                            select new XAttribute(attr.ColumnName, row[attr.ColumnName])
                            )
                        )
                    );
            return db;
        }

    }
}