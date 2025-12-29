using Microsoft.Data.Sqlite;
//// m1chael888 \\\\
namespace HabbitLogger
{ 
    internal class Program
    {
        static void Main(string[] args)
        {
            string db = "Data Source=trackington.db";
            var habitList = new List<Habit>();

            try
            {
                NonQuery(@"CREATE TABLE IF NOT EXISTS hydrate(
                         id INTEGER PRIMARY KEY AUTOINCREMENT,
                         Date TEXT,
                         Qty INTEGER
                         )"); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing db: {ex.Message}");
            }
            
            Menu();

            ////
            void Menu()
            {
                Console.Clear();
                Console.WriteLine(@"//// Trackington Menu \\\\");
                Console.WriteLine("\n1 - View habit list and management");
                Console.WriteLine("2 - Create new habit to track"); // challenge
                Console.WriteLine("0 - Close trackington");
                Console.Write("\nEnter the number of your menu option: ");
                string input = Console.ReadLine();

                bool done = false;
                while (!done)
                {
                    switch (input)
                    {
                        case "1":
                            HabitDashboard();
                            done = true;
                            break;
                        case "2":
                            NewHabit(); // challenge
                            break;
                        case "0":
                            CloseApp();
                            break;
                        default:
                            ErrorMsg("Please enter a valid menu number (0-2): ");
                            input = Console.ReadLine();
                            break;
                    }
                }
            }

            ////
            void HabitDashboard()
            {
                Console.Clear();
                Console.WriteLine(@"//// Manage habits \\\\" + "\n");

                ShowHabits();

                Console.WriteLine("\n1 - Insert a record");
                Console.WriteLine("2 - Update a record");
                Console.WriteLine("3 - Delete a record");
                Console.WriteLine("4 - Return to menu");
                Console.WriteLine("0 - Close Trackington");
                Console.Write("\nEnter the number of your menu option: ");
                string input = Console.ReadLine();

                bool done = false;
                while (!done)
                {
                    switch (input)
                    {
                        case "1":
                            InsertOccurance();
                            done = true;
                            break;
                        case "2":
                            UpdateRecord();
                            done = true;
                            break;
                        case "3":
                            DeleteRecord();
                            done = true;
                            break;
                        case "4":
                            Menu();
                            done = true;
                            break;
                        case "0":
                            CloseApp();
                            break;
                        default:
                            ErrorMsg("Please enter a valid menu number (0-4): ");
                            input = Console.ReadLine();
                            break;
                    }
                }
            }

            ////
            void ShowHabits()
            {
                habitList.Clear();
                using var connection = new SqliteConnection(db);
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText = $"SELECT * FROM hydrate";

                    SqliteDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            habitList.Add(
                                new Habit
                                {
                                    Id = reader.GetInt32(0),
                                    Date = reader.GetString(1),
                                    Qty = reader.GetInt32(2)
                                });
                        }
                    }
                    foreach (var h in habitList)
                    {
                        Console.WriteLine($"Record {h.Id}: Hydrated {h.Qty} times on {h.Date}");
                    }
                }
            }

            ////
            void InsertOccurance()
            {
                string date = CaptureDate();
                int qty = CaptureQty();

                NonQuery($"INSERT INTO hydrate(Date, Qty) VALUES('{date}', {qty})");

                HabitDashboard();
            }

            ////
            void UpdateRecord()
            {
                Console.Write("\nEnter the record number you would like to update: ");
                string input = Console.ReadLine();

                while (!int.TryParse(input, out int Id) || !validRecordNum(Id))
                {
                    ErrorMsg($"Please enter a valid record number listed above: ");
                    input = Console.ReadLine();
                }

                string newDate = CaptureDate();
                int newQty = CaptureQty();

                NonQuery($"UPDATE hydrate SET Date = '{newDate}', Qty = {newQty} WHERE Id = {Convert.ToInt32(input)}");

                Console.Clear();
                HabitDashboard();
            }

            ////
            void DeleteRecord()
            {
                Console.Write("\nEnter the record number you would like to delete: ");
                string input = Console.ReadLine();

                while (!int.TryParse(input, out int Id) || !validRecordNum(Id))
                {
                    ErrorMsg($"Please enter a valid record number listed above: ");
                    input = Console.ReadLine();
                }

                NonQuery($"DELETE FROM hydrate WHERE Id='{Convert.ToInt32(input)}'");

                Console.Clear();
                HabitDashboard();
            }

            ////
            void NewHabit()
            {

            }

            ////
            void NonQuery(string commandText)
            {
                using var connection = new SqliteConnection(db);
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText = commandText;

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            ///
            int CaptureQty()
            {
                Console.Write("Enter the number of times you hydrated: ");
                string input = Console.ReadLine();

                bool done = false;
                while (!done)
                {
                    if (!int.TryParse(input, out int qty))
                    {
                        ErrorMsg("Please enter a whole number: ");
                        input = Console.ReadLine();
                    }
                    else done = true;
                }
                return Convert.ToInt32(input);
            }

            //////
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
                Console.WriteLine($"Chosen date: {result}");
                //TODO: if user enters existing date, prompt them to combine qtys

                return result;
            }

            ////
            bool validRecordNum(int id)
            {
                foreach (var h in habitList)
                {
                    if (h.Id == id) return true;
                }
                return false;
            }

            ////
            void ErrorMsg(string message)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"!!! {message}");
                Console.ForegroundColor = ConsoleColor.White;
            }

            ////
            void CloseApp()
            {
                Console.WriteLine();
                Console.WriteLine(@"\\\\ Come again soon ////");
                Environment.Exit(0);
            }
        }
    }

    public class Habit
    {
        public int Id { get; set; }
        public required string Date { get; set; }
        public int Qty { get; set; }
    }
}
