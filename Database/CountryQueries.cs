using System;
using System.Collections.Generic;

namespace PopulationReport.Database
{
    public partial class DatabaseManager
    {
        #region Country Queries

        // Get all countries sorted by population
        public List<Dictionary<string, object>> GetAllCountriesByPopulation()
        {
            string query = @"
                SELECT Code, Name, Continent, Region, Population, 
                       (SELECT city.Name FROM city WHERE city.ID = country.Capital) AS Capital 
                FROM country 
                ORDER BY Population DESC";
            return ExecuteQuery(query);
        }

        // Get countries by continent sorted by population
        public List<Dictionary<string, object>> GetCountriesByContinent(string continent)
        {
            string query = @"
                SELECT Code, Name, Continent, Region, Population, 
                       (SELECT city.Name FROM city WHERE city.ID = country.Capital) AS Capital 
                FROM country 
                WHERE Continent = @continent
                ORDER BY Population DESC";
            var parameters = new Dictionary<string, object> { { "@continent", continent } };
            return ExecuteQuery(query, parameters);
        }

        // Get countries by region sorted by population
        public List<Dictionary<string, object>> GetCountriesByRegion(string region)
        {
            string query = @"
                SELECT Code, Name, Continent, Region, Population, 
                       (SELECT city.Name FROM city WHERE city.ID = country.Capital) AS Capital 
                FROM country 
                WHERE Region = @region
                ORDER BY Population DESC";
            var parameters = new Dictionary<string, object> { { "@region", region } };
            return ExecuteQuery(query, parameters);
        }

        // Get top N countries in the world by population
        public List<Dictionary<string, object>> GetTopNCountriesWorld(int n)
        {
            string query = @"
                SELECT Code, Name, Continent, Region, Population, 
                       (SELECT city.Name FROM city WHERE city.ID = country.Capital) AS Capital 
                FROM country 
                ORDER BY Population DESC
                LIMIT @n";
            var parameters = new Dictionary<string, object> { { "@n", n } };
            return ExecuteQuery(query, parameters);
        }

        // Get top N countries in a continent by population
        public List<Dictionary<string, object>> GetTopNCountriesContinent(int n, string continent)
        {
            string query = @"
                SELECT Code, Name, Continent, Region, Population, 
                       (SELECT city.Name FROM city WHERE city.ID = country.Capital) AS Capital 
                FROM country 
                WHERE Continent = @continent
                ORDER BY Population DESC
                LIMIT @n";
            var parameters = new Dictionary<string, object> {
                { "@n", n },
                { "@continent", continent }
            };
            return ExecuteQuery(query, parameters);
        }

        // Get top N countries in a region by population
        public List<Dictionary<string, object>> GetTopNCountriesRegion(int n, string region)
        {
            string query = @"
                SELECT Code, Name, Continent, Region, Population, 
                       (SELECT city.Name FROM city WHERE city.ID = country.Capital) AS Capital 
                FROM country 
                WHERE Region = @region
                ORDER BY Population DESC
                LIMIT @n";
            var parameters = new Dictionary<string, object> {
                { "@n", n },
                { "@region", region }
            };
            return ExecuteQuery(query, parameters);
        }

        // Get list of all continents
        public List<Dictionary<string, object>> GetAllContinents()
        {
            string query = "SELECT DISTINCT Continent FROM country ORDER BY Continent";
            return ExecuteQuery(query);
        }

        // Get list of all regions
        public List<Dictionary<string, object>> GetAllRegions()
        {
            string query = "SELECT DISTINCT Region FROM country ORDER BY Region";
            return ExecuteQuery(query);
        }

        // Get list of all countries
        public List<Dictionary<string, object>> GetAllCountries()
        {
            string query = "SELECT DISTINCT Name FROM country ORDER BY Name";
            return ExecuteQuery(query);
        }

        #endregion
    }
}