using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Xml.Linq;
using System.Data;

namespace DatabaseXMLConverter
{
    static class DatabaseCreate
    {
        private static MySqlCommand sqlCommand;

        public static void CreateDatabase(string server, string database, string uid, string password)
        {
            string createDatabaseQuery;
            string dropQuery;
            createDatabaseQuery = "create database if not exists " + database + ";";
            dropQuery = "drop database if exists " + database + ";";

            DatabaseConnection.Connection = new MySqlConnection(
                "server=" + server + "; user=" + uid + "; port=" + 3306 + "; password=" + password);
            
            try
            {
                DatabaseConnection.Connection.Open();
                sqlCommand = DatabaseConnection.Connection.CreateCommand();
                sqlCommand.CommandText = dropQuery;
                sqlCommand.ExecuteNonQuery();
                sqlCommand.CommandText = createDatabaseQuery;
                sqlCommand.ExecuteNonQuery();
                
                Console.WriteLine("Datenbank wurde erzeugt!");
            }catch(Exception e1)
            {
                Console.WriteLine("Ein Fehler ist aufgetreten!");
                Console.WriteLine(e1.Message);
            }
            finally
            {
                DatabaseConnection.Connection.Close();
            }
            return;
        }


        public static void CreateTables()
        {
            string createTables = "";
            var XMLFile = XElement.Load("../../xml/test.xml");

            List<string> tableNames = (from x in XMLFile.Elements()
                              select x.Name.ToString()).Distinct().ToList();

            /*foreach (var item in tableNames)
            {
                Console.WriteLine(item);
            }*/
            
            List<XName> firstDataRows = new List<XName>();
            foreach (var item in tableNames)
            {
                firstDataRows.Add(XMLFile.Element(item).Descendants().First().Name);
            }
            /*foreach (var item in firstDataRows)
            {
                Console.WriteLine(item);
            }*/

            List<XAttribute> attributes = new List<XAttribute>();
            foreach (var item in tableNames)
            {
                attributes.AddRange((from x in XMLFile.Element(item).Descendants().First().Attributes() //Remove First() for all Attributes of all Elements
                     select x).ToList());
            }
            foreach (var item in attributes)
            {
                Console.WriteLine(item);//Alle Attribute
            }

            for (int i = 0; i < tableNames.Count; i++)
            {
                List<XName> attr = (from x in XMLFile.Element(tableNames[i]).Descendants().First().Attributes() //Remove First() for all Attributes of all Elements
                                     select x.Name).ToList();
                createTables += "create table " + tableNames[i] + "(";
                for (int j = 0; j < attr.Count; j++)
                    {
                        createTables += attr[j]+" <Type> <primarykey>";
                    }
                createTables += ")engine=InnoDB;";
            }

        }

        public static void ReadXML()
        {

            DataSet d = new DataSet();
            d.ReadXmlSchema("../../xml/test.xml");

            Console.WriteLine(d.GetXmlSchema());
            
        }
    }
}
