using Microsoft.Data.Sqlite;
//// m1chael888 \\\\
namespace HabbitLogger
{ 
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                InitializeDb(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            Menu();

            void Menu()
            {
                Console.Clear();
                int errorCount = 0;
                bool done = false;
                string? input;

                Console.WriteLine(@"//// Trackington Menu \\\\");
                Console.WriteLine("\n1 - View your habit progress");
                Console.WriteLine("2 - Update your habit progress");
                Console.WriteLine("3 - Remove a habit and its history");
                Console.WriteLine("4 - Add a new habit to track");
                Console.WriteLine("5 - Close trackington");
                Console.WriteLine("\nEnter the number of your desired operation:");

                while (!done)
                {
                    input = Console.ReadLine();

                    switch (input)
                    {
                        /*case "1":
                            ReadHabit();
                            break;
                        case "2":
                            UpdateHabit();
                            break;
                        case "3":
                            DeleteHabit();
                            break;
                        case "4":
                            CreateHabit();
                            break;*/
                        case "5":
                            Console.WriteLine();
                            Console.WriteLine(@"\\\\ Come again soon ////");
                            Environment.Exit(0);
                            break;
                        default:
                            errorCount++;
                            if (errorCount > 3) Menu();

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("!!! Please enter a valid menu number (1-5)");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }
                }
            }

            void ReadHabit()
            {

            }

            void UpdateHabit()
            {

            }

            void DeleteHabit()
            {

            }

            void CreateHabit()
                {

            }

            void InitializeDb()
            {
                string db = "Data Source=trackington.db";
                using var connection = new SqliteConnection(db);
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
    }
}
