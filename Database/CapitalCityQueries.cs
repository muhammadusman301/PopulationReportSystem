using System;
using System.Collections.Generic;

namespace PopulationReport.Database
{
    public partial class DatabaseManager
    {
        #region Capital City Queries

        // Get all capital cities in the world sorted by population
        public List<Dictionary<string, object>> GetAllCapitalCitiesByPopulation()
        {
            string query = @"
                SELECT city.Name, country.Name AS Country, city.Population
                FROM city
                JOIN country ON city.ID = country.Capital
                ORDER BY city.Population DESC";
            return ExecuteQuery(query);
        }

        // Get all capital cities in a continent sorted by population
        public List<Dictionary<string, object>> GetCapitalCitiesByContinent(string continent)
        {
            string query = @"
                SELECT city.Name, country.Name AS Country, city.Population
                FROM city
                JOIN country ON city.ID = country.Capital
                WHERE country.Continent = @continent
                ORDER BY city.Population DESC";
            var parameters = new Dictionary<string, object> { { "@continent", continent } };
            return ExecuteQuery(query, parameters);
        }

        // Get all capital cities in a region sorted by population
        public List<Dictionary<string, object>> GetCapitalCitiesByRegion(string region)
        {
            string query = @"
                SELECT city.Name, country.Name AS Country, city.Population
                FROM city
                JOIN country ON city.ID = country.Capital
                WHERE country.Region = @region
                ORDER BY city.Population DESC";
            var parameters = new Dictionary<string, object> { { "@region", region } };
            return ExecuteQuery(query, parameters);
        }

        // Get top N capital cities in the world by population
        public List<Dictionary<string, object>> GetTopNCapitalCitiesWorld(int n)
        {
            string query = @"
                SELECT city.Name, country.Name AS Country, city.Population
                FROM city
                JOIN country ON city.ID = country.Capital
                ORDER BY city.Population DESC
                LIMIT @n";
            var parameters = new Dictionary<string, object> { { "@n", n } };
            return ExecuteQuery(query, parameters);
        }

        // Get top N capital cities in a continent by population
        public List<Dictionary<string, object>> GetTopNCapitalCitiesContinent(int n, string continent)
        {
            string query = @"
                SELECT city.Name, country.Name AS Country, city.Population
                FROM city
                JOIN country ON city.ID = country.Capital
                WHERE country.Continent = @continent
                ORDER BY city.Population DESC
                LIMIT @n";
            var parameters = new Dictionary<string, object> {
                { "@n", n },
                { "@continent", continent }
            };
            return ExecuteQuery(query, parameters);
        }

        // Get top N capital cities in a region by population
        public List<Dictionary<string, object>> GetTopNCapitalCitiesRegion(int n, string region)
        {
            string query = @"
                SELECT city.Name, country.Name AS Country, city.Population
                FROM city
                JOIN country ON city.ID = country.Capital
                WHERE country.Region = @region
                ORDER BY city.Population DESC
                LIMIT @n";
            var parameters = new Dictionary<string, object> {
                { "@n", n },
                { "@region", region }
            };
            return ExecuteQuery(query, parameters);
        }

        #endregion
    }
}