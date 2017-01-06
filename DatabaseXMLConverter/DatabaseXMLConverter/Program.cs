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
            Console.Write("\nGeben Sie den Server ein auf dem ihre Datenbank ausgeführt wird: ");
            string server = Console.ReadLine();

            Console.Write("\nGeben Sie den Namen der Datenbank ein die zu bearbeiten ist: ");
            string database = Console.ReadLine();

            Console.Write("\nGeben Sie den Benutzernamen ein mit dem auf die Datenbank zugegriffen werden soll: ");
            string uid = Console.ReadLine();

            Console.Write("\nGeben Sie Ihr Passwort ein: ");
            string password = Console.ReadLine();

            Console.WriteLine("\n------------------------------------------------");
            Console.Write("################### OPTIONEN ###################\n" +
                "Wie sollen Datensätze einer Tabelle benannt werden?\n" +
                "1.Option: '<Autos_Element .... />'\n" +
                "2.Option: '<Auto .... />' (Letzter Buchstabe des Tabellennamens wird entfernt)\n" +
                "'1' oder '2' ... ");
            int datasetStyle = (int)Char.GetNumericValue(Console.ReadKey(false).KeyChar);
            Console.WriteLine("\n");

            Console.Write("Output Destination-Path: ");
            string outputPath = Console.ReadLine();
            Console.WriteLine("\n");

            DatabaseConnection.Connect(server, database, uid, password);

            outputPath = outputPath.Replace('/', '\\').Replace("\\", "\\\\");
            XMLConverter.ParseDatabase(DatabaseConnection.Database, DatabaseConnection.GetTables(), XMLConverter.DatasetStyle.ELEMENT).Save(outputPath);

            Process.Start(outputPath);

            DatabaseConnection.Close();
        }
    }
}