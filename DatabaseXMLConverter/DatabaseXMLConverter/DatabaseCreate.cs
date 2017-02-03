using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Xml.Linq;
using System.Data;

namespace DatabaseXMLConverter
{
    static class DatabaseCreate
    {
        private static MySqlCommand sqlCommand;
        private static XElement databaseFile;

        public static void CreateDatabase(string server,string databaseAsXML, string uid, string port, string password)
        {
            databaseFile = XElement.Load(databaseAsXML); //Der Datenbank Name steht in der Datei und ist gleich das erste Element.
            string createDatabaseQuery;
            string dropQuery;
            createDatabaseQuery = "create database " + databaseFile.Name + ";"+
                "use " + databaseFile.Name + ";";
            dropQuery = "drop database if exists " + databaseFile.Name + ";";

            DatabaseConnection.Connection = new MySqlConnection(
                "server=" + server + "; user=" + uid + "; port=" + port + "; password=" + password);

            try
            {
                Console.WriteLine("Datenbank wird erzeugt ...");
                DatabaseConnection.Connection.Open();
                sqlCommand = DatabaseConnection.Connection.CreateCommand();
                sqlCommand.CommandText = dropQuery;
                sqlCommand.ExecuteNonQuery();
                sqlCommand.CommandText = createDatabaseQuery;
                sqlCommand.ExecuteNonQuery();
                Console.WriteLine("Datenbank wurde erzeugt!");
            }
            catch(Exception e1)
            {
                Console.WriteLine("Ein Fehler ist bei der Datenbankerstellung aufgetreten aufgetreten!");
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
            List<XName> tableNames = (from x in databaseFile.Elements()
                                       select x.Name).ToList();
            List<XName> columnNames = new List<XName>();

            for (int i = 0; i < tableNames.Count; i++)
            {
                columnNames = (from x in databaseFile.Element(tableNames[i]).Descendants().First().Attributes()
                                    select x.Name).ToList();
                XAttribute datatypes = (from x in databaseFile.Element(tableNames[i]).Attributes()
                                       select x).First();
                
                createTables += "create table " + tableNames[i] + "(";
                for (int j = 0; j < columnNames.Count; j++)
                {
                    string datatype = datatypes.Value.ToString().Split(';')[j].ToString();
                    switch (datatype)
                    {
                        case "String":
                            datatype = "CHAR(100)";
                            break;
                        case "Int16":
                        case "Int32":
                            datatype = "INT(10)";
                            break;
                        case "Single":
                            datatype = "FLOAT(10)";
                            break;
                        case "TimeSpan":
                            datatype = "TIME";
                            break;
                    }
                    if (j == columnNames.Count - 1)
                        createTables += columnNames[j] + " " + datatype;
                    else
                        createTables += columnNames[j] + " " + datatype +",";
                }
                createTables += ");";
            }

            try
            {
                Console.WriteLine("Tabellen werden erzeugt ...");
                sqlCommand.Connection.Open();
                sqlCommand.CommandText = createTables;
                sqlCommand.ExecuteNonQuery();

                Console.WriteLine("Tabellen wurden erzeugt!");
            }
            catch(Exception e1)
            {
                Console.WriteLine("\nEin Fehler ist bei der Tabellenerstellung aufgetreten!");
                Console.WriteLine(e1.Message);

            }
            finally
            {
                DatabaseConnection.Connection.Close();
            }

            InsertData(tableNames);

        }

        private static void InsertData(List<XName> tableNames)
        {
            string insertQuery = "";
            List<XAttribute> rowValues = new List<XAttribute>();
            for (int i = 0; i < tableNames.Count; i++)
            {
                int count = (from x in databaseFile.Element(tableNames[i]).Descendants().First().Attributes()
                                           select x.Name).ToList().Count;
                int limit = count;
                rowValues = databaseFile.Element(tableNames[i]).Descendants().Attributes().ToList();
                insertQuery += "INSERT INTO " + tableNames[i] + " VALUES (";
                for (int j = 0; j < rowValues.Count; j++)
                {
                    if (rowValues[j].Value.Contains('\''))
                        rowValues[j].Value.Replace('\'', '′');
                    if (j == rowValues.Count - 1)
                        insertQuery += (rowValues[j].Value == "") ? ("NULL);") : ("\'" + rowValues[j].Value + "\');");
                    else if (j == limit-1)
                    { 
                        insertQuery += (rowValues[j].Value == "") ? ("NULL),(") : ("\'" + rowValues[j].Value + "\'),(");
                        limit += count;
                    }
                    else
                        insertQuery += (rowValues[j].Value == "") ? ("NULL,") : ("\'" + rowValues[j].Value + "\',");
                }
            }

            try
            {
                Console.WriteLine("Tabellen werden mit Daten befüllt ...");
                sqlCommand.Connection.Open();
                sqlCommand.CommandText = insertQuery;
                sqlCommand.ExecuteNonQuery();

                Console.WriteLine("Tabellen wurden mit Daten befüllt!");
            }
            catch (Exception e1)
            {
                Console.WriteLine("\nEin Fehler ist beim einfuegen!");
                Console.WriteLine(e1.Message);

            }
            finally
            {
                DatabaseConnection.Connection.Close();
            }
        }
    }
}
