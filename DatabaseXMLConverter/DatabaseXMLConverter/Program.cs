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
            /**
            DatabaseConnection.Connect("localhost", "world", "root", "Ugur1995");
            string[] lol = DatabaseConnection.GetDatatypesOfTable("city");
            foreach(string s in lol)
            {
                Console.WriteLine(s);
            }
            Console.ReadKey();
            **/
            
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
            Console.Write("\nAuswahl:");
            int input = 0;
            while (input != 1 && input != 2)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                input = (int)Char.GetNumericValue(Console.ReadKey().KeyChar);
                Console.ResetColor();
                switch (input)
                {
                    case 1:
                        Console.WriteLine("\nSie haben Option 1 ausgewaehlt.");
                        ConsoleDialog.CreateXMLFileFromDatabase();
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
            Console.WriteLine("Vorgang abgeschlossen! Das Ergebnis wird mit Ihrem Standartprogramm geöffnet!");
            Console.ReadKey();
            
        }

        
    }
}