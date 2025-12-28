using Microsoft.Data.Sqlite;
using System;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;
//// m1chael888 \\\\
namespace HabbitLogger
{ 
    internal class Program
    {
        static void Main(string[] args)
        {
            string db = "Data Source=trackington.db";

            try
            {
                InitializeDb(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing db: {ex.Message}");
            }
            
            Menu();

            //////////
            void Menu()
            {
                Console.Clear();

                bool done = false;
                string? input;

                Console.WriteLine(@"//// Trackington Menu \\\\");
                Console.WriteLine("\n1 - View your habit progress");
                Console.WriteLine("2 - Update your habit progress");
                Console.WriteLine("3 - Remove a habit and its history");
                Console.WriteLine("4 - Add a new habit to track");
                Console.WriteLine("5 - Close trackington");
                Console.Write("\nEnter the number of your menu option: ");

                while (!done)
                {
                    input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            ReadHabit();
                            break;
                        case "2":
                            InsertOccurance();
                            break;
                        case "3":
                            DeleteHabit();
                            break;
                        case "4":
                            CreateHabit();
                            break;
                        case "5":
                            CloseApp();
                            break;
                        default:
                            ErrorMsg("Please enter a valid menu number (1-5): ");
                            break;
                    }
                }
            }

            ///////////////
            void ReadHabit()
            {
                
            }

            /////////////////
            void InsertOccurance()
            {
                Console.Clear();
                Console.WriteLine(@"//// New hydration entry \\\\");
                
                string date = CaptureDate();
                int qty = CaptureQty();

                using var connection = new SqliteConnection(db);
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText = $"INSERT INTO hydrate(Date, Qty) VALUES('{date}', {qty})";

                    command.ExecuteNonQuery();
                    connection.Close();
                }

                Console.Clear();
                Console.WriteLine(@"//// Success! Whats next? \\\\");
                Console.WriteLine("\n1 - Make another entry");
                Console.WriteLine("2 - Return to menu");
                Console.WriteLine("3 - Close trackington");
                Console.Write("\nEnter the number of your menu option: ");
                string input = Console.ReadLine();

                bool done = false;
                while (!done)
                {
                    switch (input)
                    {
                        case "1":
                            InsertOccurance();
                            break;
                        case "2":
                            Menu();
                            break;
                        case "3":
                            CloseApp();
                            break;
                        default:
                            ErrorMsg("Please enter a valid menu number (1-3): ");
                            input = Console.ReadLine();
                            break;
                    }
                }
            }

            int CaptureQty()
            {
                Console.Write("Enter the number of times you hydrated: ");
                string input = Console.ReadLine();

                bool done = false;
                while (!done)
                {
                    if (!int.TryParse(input, out int qty))
                    {
                        ErrorMsg("Please enter a whole number");
                        input = Console.ReadLine();
                    }
                    else done = true;
                }
                return Convert.ToInt32(input);
            }

            ///////////////////
            string CaptureDate()
            {
                Console.Write("\nWhat day did you hydate? (MM/dd/yyyy format) You can enter 'today' to use todays date: ");
                string result = Console.ReadLine();

                bool done = false;
                while (!done)
                {
                    if (result != "today" && !DateTime.TryParse(result, out DateTime date))
                    {
                        ErrorMsg("Please enter a valid date (MM/dd/yyyy format) or 'today' to use today's date: ");
                        result = Console.ReadLine();
                    }
                    else done = true;
                }
                
                if (result.ToLower() == "today") result = DateTime.Today.ToString("MM/dd/yyyy");
                Console.WriteLine($"Chosen date: {result}\n");
                //TODO: validation. if user enters existing date, prompt them to combine qtys

                return result;
            }

            /////////////////
            void DeleteHabit()
            {

            }
            
            /////////////////
            void CreateHabit()
            {

            }

            //////////////////
            void InitializeDb()
            {
                using var connection = new SqliteConnection(db);
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText =
                        @"CREATE TABLE IF NOT EXISTS hydrate(
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Qty INTEGER
                        )";

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            void ErrorMsg(string message)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"!!! {message}");
                Console.ForegroundColor = ConsoleColor.White;
            }

            void CloseApp()
            {
                Console.WriteLine();
                Console.WriteLine(@"\\\\ Come again soon ////");
                Environment.Exit(0);
            }
        }
    }
}
