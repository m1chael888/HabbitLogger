using Microsoft.Data.Sqlite;
using System;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
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
                Console.WriteLine("\n1 - View habit list and management");
                Console.WriteLine("2 - Add new habit occurances");
                Console.WriteLine("3 - Create new habit to track"); // challenge
                Console.WriteLine("0 - Close trackington");
                Console.Write("\nEnter the number of your menu option: ");

                while (!done)
                {
                    input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            ReadHabit(); //
                            break;
                        case "2":
                            InsertOccurance(); 
                            break;
                        case "3":
                            CreateHabit(); // challenge
                            break;
                        case "0":
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
                Console.Clear();
                Console.WriteLine(@"//// Manage habits \\\\" + "\n");

                using var connection = new SqliteConnection(db);
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText = $"SELECT * FROM hydrate";

                    var habitList = new List<habit>();

                    SqliteDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            habitList.Add(
                                new habit
                                {
                                    Id = reader.GetInt32(0),
                                    Date = reader.GetString(1),
                                    Qty = reader.GetInt32(2)
                                });                        
                        }
                    }
                    foreach (var h in habitList)
                    {
                        Console.WriteLine($"{h.Id}: Hydrated {h.Qty} times on {h.Date}");
                    }
                }
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
                Console.WriteLine("0 - Close trackington");
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
                        case "0":
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
                Console.Write("\nWhat day did you hydrate? (MM/dd/yyyy format) You can enter 'today' to use todays date: ");
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
                //TODO: if user enters existing date, prompt them to combine qtys

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

    public class habit
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int Qty { get; set; }
    }
}
