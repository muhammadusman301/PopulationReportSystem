using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace PopulationReport.Database
{
    public partial class DatabaseManager
    {
        private readonly string connectionString;

        // Constructor to initialize the database connection
        public DatabaseManager(string server, string database, string user, string password)
        {
            connectionString = $"Server={server};Database={database};User ID={user};Password={password};";
        }

        // Test database connection
        public bool TestConnection()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
                return false;
            }
        }

        // Execute a SELECT query and return results
        public List<Dictionary<string, object>> ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Add parameters if provided
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                cmd.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> row = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row[reader.GetName(i)] = reader.GetValue(i);
                                }
                                results.Add(row);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database query error:");
                Console.WriteLine(ex.Message);
            }

            return results;
        }
    }
}