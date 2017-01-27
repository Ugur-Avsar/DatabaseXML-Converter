using System;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace DatabaseXMLConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"  _         ___         _          __   _    _                            _                                     
 | \   /\    |    /\   |_)   /\   (_   |_     )    /  \/  |\/|  |   \    /    _   ._        _   ._  _|_   _   ._
 |_/  /--\   |   /--\  |_)  /--\  __)  |_    /_    \  /\  |  |  |_  /    \_  (_)  | |  \/  (/_  |    |_  (/_  | 
                                                                                                                
            ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("by Ugur Avsar & Andreas Erdes");
            Console.ResetColor();

            Console.WriteLine("\nWollen Sie eine Datenbank in ein XML umwandeln \noder aus einem XML eine Datenbank generieren?");
            Console.WriteLine("\nDatenbank in XML umwandeln ... 1");
            Console.WriteLine("Aus XML eine Datenbank erzeugen ... 2");
            Console.Write("\nEingabe:");
            int input = 0;
            while (input != 1 || input != 2)
            {
                input = (int)Char.GetNumericValue(Console.ReadKey().KeyChar);
                switch (input)
                {
                    case 1:
                        Console.WriteLine("\nSie haben Option 1 ausgewaehlt.");
                        createXMLFileFromDatabase();
                        break;

                    case 2:
                        Console.WriteLine("\nSie haben Option 2 ausgewaehlt.");
                        //createDatabaseFromXML();
                        break;

                    default:
                        Console.WriteLine("\nFalsche Eingabe! Bitte geben Sie 1 oder 2 ein.");
                        break;
                }
            }
            Console.WriteLine("Vorgang abgeschlossen!");
            Console.Read();
        }

        public static void createDatabaseFromXML()
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

        public static void createXMLFileFromDatabase()
        {
            Console.ResetColor();
            Console.Write("\nGeben Sie den Server ein auf dem ihre Datenbank ausgeführt wird: ");
            string server = Console.ReadLine();

            Console.Write("\nGeben Sie den Namen der Datenbank ein die zu bearbeiten ist: ");
            string database = Console.ReadLine();

            Console.Write("\nGeben Sie den Benutzernamen ein mit dem auf die Datenbank zugegriffen werden soll: ");
            string uid = Console.ReadLine();

            Console.Write("\nGeben Sie Ihr Passwort ein: ");
            string password = "";
            while(true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                if(key.Key != ConsoleKey.Backspace)
                {
                    password += key.KeyChar;
                    Console.Write('*');
                }
                else if(password.Length > 0)
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
            while(datasetStyle != 1 ||datasetStyle != 2)
            {
                datasetStyle = (int)Char.GetNumericValue(Console.ReadKey().KeyChar);
                Console.WriteLine("\nFalsche Eingabe! Bitte geben Sie 1 oder 2 ein.");
            }
            Console.WriteLine("\n");

            Console.Write("Output Destination-Path: ");
            string outputPath = Console.ReadLine();
            Console.WriteLine("\n");

            DatabaseConnection.Connect(server, database, uid, password);

            outputPath = outputPath.Replace('/', '\\');
            XMLConverter.ParseDatabase(DatabaseConnection.Database, DatabaseConnection.GetTables(), XMLConverter.DatasetStyle.ELEMENT).Save(outputPath);

            Process.Start(outputPath);

            DatabaseConnection.Close();
        }
    }
}