using System;
using System.Collections.Generic;

namespace PopulationReport.Database
{
    public partial class DatabaseManager
    {
        #region Population Queries

        // Get world population
        public long GetWorldPopulation()
        {
            string query = "SELECT SUM(Population) AS TotalPopulation FROM country";
            var result = ExecuteQuery(query);
            return Convert.ToInt64(result[0]["TotalPopulation"]);
        }

        // Get continent population
        public Dictionary<string, object> GetContinentPopulation(string continent)
        {
            string query = @"
                SELECT Continent, SUM(Population) AS TotalPopulation
                FROM country
                WHERE Continent = @continent
                GROUP BY Continent";
            var parameters = new Dictionary<string, object> { { "@continent", continent } };
            var result = ExecuteQuery(query, parameters);
            return result.Count > 0 ? result[0] : null;
        }

        // Get region population
        public Dictionary<string, object> GetRegionPopulation(string region)
        {
            string query = @"
                SELECT Region, SUM(Population) AS TotalPopulation
                FROM country
                WHERE Region = @region
                GROUP BY Region";
            var parameters = new Dictionary<string, object> { { "@region", region } };
            var result = ExecuteQuery(query, parameters);
            return result.Count > 0 ? result[0] : null;
        }

        // Get country population
        public Dictionary<string, object> GetCountryPopulation(string countryName)
        {
            string query = @"
                SELECT Name, Population
                FROM country
                WHERE Name = @countryName";
            var parameters = new Dictionary<string, object> { { "@countryName", countryName } };
            var result = ExecuteQuery(query, parameters);
            return result.Count > 0 ? result[0] : null;
        }

        // Get district population
        public Dictionary<string, object> GetDistrictPopulation(string district)
        {
            string query = @"
                SELECT District, SUM(Population) AS TotalPopulation
                FROM city
                WHERE District = @district
                GROUP BY District";
            var parameters = new Dictionary<string, object> { { "@district", district } };
            var result = ExecuteQuery(query, parameters);
            return result.Count > 0 ? result[0] : null;
        }

        // Get city population
        public Dictionary<string, object> GetCityPopulation(string cityName)
        {
            string query = @"
                SELECT Name, Population
                FROM city
                WHERE Name = @cityName";
            var parameters = new Dictionary<string, object> { { "@cityName", cityName } };
            var result = ExecuteQuery(query, parameters);
            return result.Count > 0 ? result[0] : null;
        }

        // Population living in cities vs not living in cities by continent
        public List<Dictionary<string, object>> GetCityVsRuralPopulationByContinent()
        {
            string query = @"
                SELECT 
                    co.Continent,
                    SUM(co.Population) AS TotalPopulation,
                    SUM(IFNULL(ci.CityPopulation, 0)) AS UrbanPopulation,
                    SUM(co.Population) - SUM(IFNULL(ci.CityPopulation, 0)) AS RuralPopulation
                FROM country co
                LEFT JOIN (
                    SELECT CountryCode, SUM(Population) AS CityPopulation 
                    FROM city 
                    GROUP BY CountryCode
                ) ci ON co.Code = ci.CountryCode
                GROUP BY co.Continent
                ORDER BY co.Continent";
            return ExecuteQuery(query);
        }

        // Population living in cities vs not living in cities by region
        public List<Dictionary<string, object>> GetCityVsRuralPopulationByRegion()
        {
            string query = @"
                SELECT 
                    co.Region,
                    SUM(co.Population) AS TotalPopulation,
                    SUM(IFNULL(ci.CityPopulation, 0)) AS UrbanPopulation,
                    SUM(co.Population) - SUM(IFNULL(ci.CityPopulation, 0)) AS RuralPopulation
                FROM country co
                LEFT JOIN (
                    SELECT CountryCode, SUM(Population) AS CityPopulation 
                    FROM city 
                    GROUP BY CountryCode
                ) ci ON co.Code = ci.CountryCode
                GROUP BY co.Region
                ORDER BY co.Region";
            return ExecuteQuery(query);
        }

        // Population living in cities vs not living in cities by country
        public List<Dictionary<string, object>> GetCityVsRuralPopulationByCountry()
        {
            string query = @"
                SELECT 
                    co.Name AS Country,
                    co.Population AS TotalPopulation,
                    IFNULL(ci.CityPopulation, 0) AS UrbanPopulation,
                    co.Population - IFNULL(ci.CityPopulation, 0) AS RuralPopulation
                FROM country co
                LEFT JOIN (
                    SELECT CountryCode, SUM(Population) AS CityPopulation 
                    FROM city 
                    GROUP BY CountryCode
                ) ci ON co.Code = ci.CountryCode
                ORDER BY co.Name";
            return ExecuteQuery(query);
        }

        #endregion
    }
}