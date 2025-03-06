using System;
using System.Collections.Generic;
using System.IO;
using MySql.Data.MySqlClient;

public class DatabaseManager
{
    private string connectionString;

    public DatabaseManager()
    {
        // Use environment variables or a configuration file instead of hardcoded credentials
        string server = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost";
        string user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
        string password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";

        // First, connect without specifying a database to create 'world' if needed
        string tempConnectionString = $"server={server};user={user};password={password};";

        using (MySqlConnection conn = new MySqlConnection(tempConnectionString))
        {
            try
            {
                conn.Open();
                string createDbQuery = "CREATE DATABASE IF NOT EXISTS world";
                MySqlCommand cmd = new MySqlCommand(createDbQuery, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating database: " + ex.Message);
            }
        }

        // Now, update the connection string to use 'world' database
        connectionString = $"server={server};database=world;user={user};password={password};";

        // Ensure the 'country' table exists
        CreateCountryTable();
    }

    private void CreateCountryTable()
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS country (
                        Code VARCHAR(3) PRIMARY KEY,
                        Name VARCHAR(255) NOT NULL,
                        Continent VARCHAR(50) NOT NULL,
                        Region VARCHAR(50) NOT NULL,
                        Population INT NOT NULL,
                        Capital INT
                    )";
                MySqlCommand cmd = new MySqlCommand(createTableQuery, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating country table: " + ex.Message);
            }
        }
    }

    public List<Country> GetCountriesByPopulation()
    {
        List<Country> countries = new List<Country>();
        string query = "SELECT Code, Name, Continent, Region, Population, Capital FROM country ORDER BY Population DESC";

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Country country = new Country()
                {
                    Code = reader["Code"] is DBNull ? string.Empty : reader["Code"].ToString(),
                    Name = reader["Name"] is DBNull ? string.Empty : reader["Name"].ToString(),
                    Continent = reader["Continent"] is DBNull ? string.Empty : reader["Continent"].ToString(),
                    Region = reader["Region"] is DBNull ? string.Empty : reader["Region"].ToString(),
                    Population = reader["Population"] is DBNull ? 0 : Convert.ToInt32(reader["Population"]),
                    Capital = reader["Capital"] is DBNull ? 0 : Convert.ToInt32(reader["Capital"])
                };

                countries.Add(country);
            }

        }
        return countries;
    }
}
