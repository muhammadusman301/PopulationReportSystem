using System;
using PopulationReport.Database;
using PopulationReport.Reports;

namespace PopulationReport
{
    class Program
    {
        private static DatabaseManager dbManager;

        static void Main(string[] args)
        {
            Console.WriteLine("Population Reporting System");
            Console.WriteLine("==========================");

            // Initialize database connection
            InitializeDatabase();

            // Main menu loop
            bool exitProgram = false;
            while (!exitProgram)
            {
                Console.Clear();
                PrintMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        new CountryReports(dbManager).GenerateReport();
                        break;
                    case "2":
                        new CityReports(dbManager).GenerateReport();
                        break;
                    case "3":
                        new CapitalCityReports(dbManager).GenerateReport();
                        break;
                    case "4":
                        new PopulationReports(dbManager).GenerateReport();
                        break;
                    case "5":
                        new LanguageReports(dbManager).GenerateReport();
                        break;
                    case "6":
                        exitProgram = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void PrintMainMenu()
        {
            Console.WriteLine("Population Reporting System");
            Console.WriteLine("==========================");
            Console.WriteLine("1. Country Reports");
            Console.WriteLine("2. City Reports");
            Console.WriteLine("3. Capital City Reports");
            Console.WriteLine("4. Population Reports");
            Console.WriteLine("5. Language Speaker Statistics");
            Console.WriteLine("6. Exit");
            Console.Write("\nSelect an option (1-6): ");
        }

        private static void InitializeDatabase()
        {
            bool connected = false;

            while (!connected)
            {
                Console.Clear();
                Console.WriteLine("Database Connection Setup");
                Console.WriteLine("========================");
                Console.WriteLine("Please enter your database connection details:");

                Console.Write("Server (default: localhost): ");
                string server = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(server))
                    server = "localhost";

                Console.Write("Database (default: world): ");
                string database = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(database))
                    database = "world";

                Console.Write("User (default: root): ");
                string user = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(user))
                    user = "root";

                Console.Write("Password: ");
                string password = Console.ReadLine();

                Console.WriteLine("\nConnecting to database...");

                dbManager = new DatabaseManager(server, database, user, password);
                connected = dbManager.TestConnection();

                if (!connected)
                {
                    Console.WriteLine("Failed to connect to the database. Please check your connection details.");
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("Successfully connected to the database!");
            Console.WriteLine("Press any key to continue to the main menu...");
            Console.ReadKey();
        }
    }
}