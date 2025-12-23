using System;
using System.Data;

namespace HabbitLogger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // todo: check for sqlite database, create if not found
            Menu();

            void Menu()
            {
                bool done = false;
                string? input;

                Console.WriteLine(@"//// Trackington Menu \\\\");
                while (!done)
                {
                    Console.WriteLine("\n1 - View your habit progress");
                    Console.WriteLine("2 - Update your habit progress");
                    Console.WriteLine("3 - Remove a habit and its history");
                    Console.WriteLine("4 - Add a new habit to track");
                    Console.WriteLine("5 - Close trackington");
                    Console.Write("\nEnter the number of your desired operation: ");
                    input = Console.ReadLine();

                    switch (input)
                    {
                        /*case "1":

                            break;
                        case "2":

                            break;
                        case "3":

                            break;
                        case "4":

                            break;*/
                        case "5":
                            Console.WriteLine();
                            Console.WriteLine(@"\\\\ Come again soon ////");
                            Environment.Exit(0);
                            break;
                        default:
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Please enter a valid menu number (1-5)");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }
                }
            }
        }
    }
}
