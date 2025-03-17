using System;
using System.Collections.Generic;

namespace PopulationReport.Database
{
    public partial class DatabaseManager
    {
        #region City Queries

        // Get all cities in the world sorted by population
        public List<Dictionary<string, object>> GetAllCitiesByPopulation()
        {
            string query = @"
                SELECT city.Name, country.Name AS Country, city.District, city.Population
                FROM city
                JOIN country ON city.CountryCode = country.Code
                ORDER BY city.Population DESC";
            return ExecuteQuery(query);
        }

        // Get all cities in a continent sorted by population
        public List<Dictionary<string, object>> GetCitiesByContinent(string continent)
        {
            string query = @"
                SELECT city.Name, country.Name AS Country, city.District, city.Population
                FROM city
                JOIN country ON city.CountryCode = country.Code
                WHERE country.Continent = @continent
                ORDER BY city.Population DESC";
            var parameters = new Dictionary<string, object> { { "@continent", continent } };
            return ExecuteQuery(query, parameters);
        }

        // Get all cities in a region sorted by population
        public List<Dictionary<string, object>> GetCitiesByRegion(string region)
        {
            string query = @"
                SELECT city.Name, country.Name AS Country, city.District, city.Population
                FROM city
                JOIN country ON city.CountryCode = country.Code
                WHERE country.Region = @region
                ORDER BY city.Population DESC";
            var parameters = new Dictionary<string, object> { { "@region", region } };
            return ExecuteQuery(query, parameters);
        }

        // Get all cities in a country sorted by population
        public List<Dictionary<string, object>> GetCitiesByCountry(string countryName)
        {
            string query = @"
                SELECT city.Name, country.Name AS Country, city.District, city.Population
                FROM city
                JOIN country ON city.CountryCode = country.Code
                WHERE country.Name = @countryName
                ORDER BY city.Population DESC";
            var parameters = new Dictionary<string, object> { { "@countryName", countryName } };
            return ExecuteQuery(query, parameters);
        }

        // Get all cities in a district sorted by population
        public List<Dictionary<string, object>> GetCitiesByDistrict(string district)
        {
            string query = @"
                SELECT city.Name, country.Name AS Country, city.District, city.Population
                FROM city
                JOIN country ON city.CountryCode = country.Code
                WHERE city.District = @district
                ORDER BY city.Population DESC";
            var parameters = new Dictionary<string, object> { { "@district", district } };
            return ExecuteQuery(query, parameters);
        }

        // Get top N cities in the world by population
        public List<Dictionary<string, object>> GetTopNCitiesWorld(int n)
        {
            string query = @"
                SELECT city.Name, country.Name AS Country, city.District, city.Population
                FROM city
                JOIN country ON city.CountryCode = country.Code
                ORDER BY city.Population DESC
                LIMIT @n";
            var parameters = new Dictionary<string, object> { { "@n", n } };
            return ExecuteQuery(query, parameters);
        }

        // Get top N cities in a continent by population
        public List<Dictionary<string, object>> GetTopNCitiesContinent(int n, string continent)
        {
            string query = @"
                SELECT city.Name, country.Name AS Country, city.District, city.Population
                FROM city
                JOIN country ON city.CountryCode = country.Code
                WHERE country.Continent = @continent
                ORDER BY city.Population DESC
                LIMIT @n";
            var parameters = new Dictionary<string, object> {
                { "@n", n },
                { "@continent", continent }
            };
            return ExecuteQuery(query, parameters);
        }

        // Get top N cities in a region by population
        public List<Dictionary<string, object>> GetTopNCitiesRegion(int n, string region)
        {
            string query = @"
                SELECT city.Name, country.Name AS Country, city.District, city.Population
                FROM city
                JOIN country ON city.CountryCode = country.Code
                WHERE country.Region = @region
                ORDER BY city.Population DESC
                LIMIT @n";
            var parameters = new Dictionary<string, object> {
                { "@n", n },
                { "@region", region }
            };
            return ExecuteQuery(query, parameters);
        }

        // Get top N cities in a country by population
        public List<Dictionary<string, object>> GetTopNCitiesCountry(int n, string countryName)
        {
            string query = @"
                SELECT city.Name, country.Name AS Country, city.District, city.Population
                FROM city
                JOIN country ON city.CountryCode = country.Code
                WHERE country.Name = @countryName
                ORDER BY city.Population DESC
                LIMIT @n";
            var parameters = new Dictionary<string, object> {
                { "@n", n },
                { "@countryName", countryName }
            };
            return ExecuteQuery(query, parameters);
        }

        // Get top N cities in a district by population
        public List<Dictionary<string, object>> GetTopNCitiesDistrict(int n, string district)
        {
            string query = @"
                SELECT city.Name, country.Name AS Country, city.District, city.Population
                FROM city
                JOIN country ON city.CountryCode = country.Code
                WHERE city.District = @district
                ORDER BY city.Population DESC
                LIMIT @n";
            var parameters = new Dictionary<string, object> {
                { "@n", n },
                { "@district", district }
            };
            return ExecuteQuery(query, parameters);
        }

        // Get list of all districts
        public List<Dictionary<string, object>> GetAllDistricts()
        {
            string query = "SELECT DISTINCT District FROM city WHERE District != '' ORDER BY District";
            return ExecuteQuery(query);
        }

        // Get list of all cities
        public List<Dictionary<string, object>> GetAllCityNames()
        {
            string query = "SELECT DISTINCT Name FROM city ORDER BY Name";
            return ExecuteQuery(query);
        }

        #endregion
    }
}