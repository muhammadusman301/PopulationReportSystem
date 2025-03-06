using System;
using System.IO;
using MySql.Data.MySqlClient;

class Program
{
    static void Main()
    {
        // Step 1: Read the config file
        string[] configLines = File.ReadAllLines("config.txt");

        string server = configLines[0].Split('=')[1].Trim();
        string database = configLines[1].Split('=')[1].Trim();
        string user = configLines[2].Split('=')[1].Trim();
        string password = configLines[3].Split('=')[1].Trim();

        // Step 2: Create the connection string
        string connectionString = $"server={server};database={database};user={user};password={password};SslMode=None;";

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                Console.WriteLine("✅ MySQL Connection Successful!\n");

                // Step 3: Define SQL query to fetch data
                string query = "SELECT Code, Name, Population FROM country LIMIT 10;";  // Fix column selection

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    // Step 4: Read and display data
                    Console.WriteLine("Code | Country Name      | Population");
                    Console.WriteLine("-------------------------------------");

                    while (reader.Read()) // Read each row
                    {
                        string code = reader.GetString(0);  // First column is a string (Code)
                        string name = reader.GetString(1);  // Second column is a string (Name)
                        int population = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);  // Handle NULL population

                        Console.WriteLine($"{code}  | {name,-15} | {population}");
                    }
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Connection failed: " + ex.Message);
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
