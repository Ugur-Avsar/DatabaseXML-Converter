using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;

namespace DatabaseXMLConverter
{
    static class DatabaseConnection
    {
        public static string Server
        {
            get
            {
                return Connection.DataSource;
            }
        }
        public static string Database
        {
            get
            {
                return Connection.Database;
            }
        }
        public static string ConnectionString
        {
            get
            {
                return Connection.ConnectionString;
            }
        }
        public static string UserID
        {
            get
            {
                return new MySqlConnectionStringBuilder(Connection.ConnectionString).UserID;
            }
        }
        public static string Password
        {
            get
            {
                return new MySqlConnectionStringBuilder(Connection.ConnectionString).Password;
            }
        }

        public static MySqlConnection Connection { get; set; }
        public static MySqlDataAdapter Adapter { get; set; }

        private static MySqlCommand command;

        public static void Connect(string server, string database, string uID, string password)
        {
            initConnection();
            Connection = new MySqlConnection("SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uID + ";" + "PASSWORD=" + password + ";");
            Connection.Open();

            command = Connection.CreateCommand();
            Adapter = new MySqlDataAdapter(command.CommandText, Connection);
        }

        private static void initConnection()
        {
            if (Connection != null && Connection.State != ConnectionState.Closed)
                Connection.Close();

        }

        public static void Close()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
                Connection = null;
            }
        }

        private static List<string> getTableNames()
        {
            command.CommandText = "show tables";
            MySqlDataReader reader = command.ExecuteReader();

            List<string> names = new List<string>();
            while (reader.Read())
            {
                names.Add(reader.GetString(0));
            }
            reader.Close();
            return names;
        }

        public static List<DataTable> GetTables()
        {
            List<DataTable> tables = new List<DataTable>();

            foreach (string tableName in getTableNames())
            {
                command.CommandText = "select * from " + tableName;
                Adapter.SelectCommand = command;

                DataTable c = new DataTable(tableName);
                Adapter.Fill(c);
                tables.Add(c);
            }

            return tables;
        }

        public static string[] GetDatatypesOfTable(string tablename)
        {
            command.CommandText = "SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tablename + "'";
            Adapter.SelectCommand = command;

            List<string> types = new List<string>();

            DataTable c = new DataTable(tablename);
            Adapter.Fill(c);

            var data = c.AsEnumerable().ToList<DataRow>();

            for (int i = 0; i < data.Count; i++)
            {
                types.Add((string)data[i][0]);
            }

            return types.ToArray<string>();
        }
    }
}