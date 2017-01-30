using System;

using System.Diagnostics;

namespace DatabaseXMLConverter
{
    class ConsoleDialog
    {
        public static void CreateDatabaseFromXML()
        {

            Console.Write("\nGeben sie die Adresse des Servers an, wo Sie die Datenbank erzeugen wollen: ");
            string server = Console.ReadLine();

            Console.Write("\nGeben Sie den Namen der Datenbank ein, die Sie erzeugen wollen: ");
            string database = Console.ReadLine();

            Console.Write("\nGeben Sie den Benutzernamen ein mit dem Sie auf die Datenbank zugreifen wollen: ");
            string username = Console.ReadLine();

            Console.Write("\nGeben Sie Ihr Passwort ein: ");
            string password = Console.ReadLine();

            DatabaseCreate.CreateDatabase(server, database, username, password);

            //DatabaseCreate.createTables();
            DatabaseCreate.ReadXML();
        }

        public static void CreateXMLFileFromDatabase()
        {
            Console.ResetColor();
            Console.Write("\nGeben Sie den Server ein auf dem ihre Datenbank ausgeführt wird: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string server = Console.ReadLine();
            Console.ResetColor();

            Console.Write("\nGeben Sie den Namen der Datenbank ein die zu bearbeiten ist: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string database = Console.ReadLine();
            Console.ResetColor();

            Console.Write("\nGeben Sie den Benutzernamen ein mit dem auf die Datenbank zugegriffen werden soll: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string uid = Console.ReadLine();
            Console.ResetColor();

            Console.Write("\nGeben Sie Ihr Passwort ein: ");
            string password = "";
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                var key = Console.ReadKey(true);
                Console.ResetColor();
                if (key.Key == ConsoleKey.Enter)
                    break;
                if (key.Key != ConsoleKey.Backspace)
                {
                    password += key.KeyChar;
                    Console.Write('*');
                }
                else if (password.Length > 0)
                {
                    password = password.Substring(0, (password.Length - 1));
                    Console.Write("\b \b");
                }
            }

            Console.WriteLine("\n------------------------------------------------");
            Console.Write("################### OPTIONEN ###################\n" +
                "Wie sollen Datensätze einer Tabelle benannt werden?\n" +
                "1.Option: '<Autos_Element .... />'\n" +
                "2.Option: '<Auto .... />' (Letzter Buchstabe des Tabellennamens wird entfernt)\n" +
                "'1' oder '2' ... ");
            int datasetStyle = 0;
            while (true)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    datasetStyle = Int32.Parse(Console.ReadKey().KeyChar + "");
                    Console.ResetColor();
                    if (datasetStyle != 1 && datasetStyle != 2)
                        throw new Exception();
                    else
                        break;
                }
                catch
                {
                    Console.WriteLine("\nFalsche Eingabe! Bitte geben Sie 1 oder 2 ein.");
                }
            }
            Console.WriteLine("\n");

            Console.Write("Output Destination-Path: ");
            string outputPath = Console.ReadLine();
            Console.WriteLine("\n");

            DatabaseConnection.Connect(server, database, uid, password);

            outputPath = outputPath.Replace('/', '\\');
            XMLConverter.ParseDatabase(DatabaseConnection.Database, DatabaseConnection.GetTables(), datasetStyle == 1 ? DatasetStyles.ELEMENT : DatasetStyles.CUT).Save(outputPath);

            Process.Start(outputPath);

            DatabaseConnection.Close();
        }
    }
}
