using System;
using System.Collections.Generic;
using PopulationReport.Database;

namespace PopulationReport.Reports
{
    public class PopulationReports
    {
        private readonly DatabaseManager dbManager;

        public PopulationReports(DatabaseManager manager)
        {
            dbManager = manager;
        }

        // Display population data for a single entity
        private void DisplaySinglePopulationReport(Dictionary<string, object> data, string title)
        {
            Console.Clear();
            Console.WriteLine($"\n{title}");
            Console.WriteLine(new string('=', title.Length));

            if (data == null)
            {
                Console.WriteLine("No data to display.");
                return;
            }

            // Extract and format the population data
            string name = data.ContainsKey("Name") ? data["Name"]?.ToString() :
                         (data.ContainsKey("Continent") ? data["Continent"]?.ToString() :
                         (data.ContainsKey("Region") ? data["Region"]?.ToString() :
                         (data.ContainsKey("District") ? data["District"]?.ToString() : "Unknown")));

            string population = data.ContainsKey("Population") ? data["Population"]?.ToString() :
                               (data.ContainsKey("TotalPopulation") ? data["TotalPopulation"]?.ToString() : "0");

            // Format with thousand separators
            if (long.TryParse(population, out long popValue))
            {
                population = popValue.ToString("#,##0");
            }

            Console.WriteLine($"Name: {name}");
            Console.WriteLine($"Population: {population}");
        }

        // Display urban vs rural population breakdown
        private void DisplayUrbanRuralReport(List<Dictionary<string, object>> data, string title)
        {
            Console.Clear();
            Console.WriteLine($"\n{title}");
            Console.WriteLine(new string('=', title.Length));

            if (data == null || data.Count == 0)
            {
                Console.WriteLine("No data to display.");
                return;
            }

            // Define column widths
            int nameWidth = 35;
            int totalWidth = 20;
            int urbanWidth = 25;
            int ruralWidth = 25;

            // Print header
            Console.WriteLine($"{"Name".PadRight(nameWidth)} " +
                             $"{"Total Population".PadRight(totalWidth)} " +
                             $"{"Urban Population (%)".PadRight(urbanWidth)} " +
                             $"{"Rural Population (%)".PadRight(ruralWidth)}");

            Console.WriteLine(new string('-', nameWidth + totalWidth + urbanWidth + ruralWidth + 3));

            // Print each row
            foreach (var entry in data)
            {
                string name = entry.ContainsKey("Continent") ? entry["Continent"]?.ToString() :
                             (entry.ContainsKey("Region") ? entry["Region"]?.ToString() :
                             (entry.ContainsKey("Country") ? entry["Country"]?.ToString() : "Unknown"));

                long totalPop = Convert.ToInt64(entry["TotalPopulation"]);
                long urbanPop = Convert.ToInt64(entry["UrbanPopulation"]);
                long ruralPop = Convert.ToInt64(entry["RuralPopulation"]);

                double urbanPerc = totalPop > 0 ? (double)urbanPop / totalPop * 100 : 0;
                double ruralPerc = totalPop > 0 ? (double)ruralPop / totalPop * 100 : 0;

                string totalPopStr = totalPop.ToString("#,##0");
                string urbanPopStr = $"{urbanPop.ToString("#,##0")} ({urbanPerc:F2}%)";
                string ruralPopStr = $"{ruralPop.ToString("#,##0")} ({ruralPerc:F2}%)";

                // Truncate long names to fit column width
                if (name.Length > nameWidth - 3) name = name.Substring(0, nameWidth - 3) + "...";

                Console.WriteLine($"{name.PadRight(nameWidth)} " +
                                 $"{totalPopStr.PadLeft(totalWidth - 2)}  " +
                                 $"{urbanPopStr.PadRight(urbanWidth)} " +
                                 $"{ruralPopStr.PadRight(ruralWidth)}");
            }
        }

        public void GenerateReport()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Population Reports");
                Console.WriteLine("=================");
                Console.WriteLine("1. World population");
                Console.WriteLine("2. Population of a continent");
                Console.WriteLine("3. Population of a region");
                Console.WriteLine("4. Population of a country");
                Console.WriteLine("5. Population of a district");
                Console.WriteLine("6. Population of a city");
                Console.WriteLine("7. Population living in cities vs rural by continent");
                Console.WriteLine("8. Population living in cities vs rural by region");
                Console.WriteLine("9. Population living in cities vs rural by country");
                Console.WriteLine("10. Return to main menu");
                Console.Write("\nEnter your choice (1-10): ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        GenerateWorldPopulationReport();
                        break;

                    case "2":
                        GenerateContinentPopulationReport();
                        break;

                    case "3":
                        GenerateRegionPopulationReport();
                        break;

                    case "4":
                        GenerateCountryPopulationReport();
                        break;

                    case "5":
                        GenerateDistrictPopulationReport();
                        break;

                    case "6":
                        GenerateCityPopulationReport();
                        break;

                    case "7":
                        DisplayUrbanRuralReport(
                            dbManager.GetCityVsRuralPopulationByContinent(),
                            "Population Living in Cities vs Rural Areas by Continent");
                        break;

                    case "8":
                        DisplayUrbanRuralReport(
                            dbManager.GetCityVsRuralPopulationByRegion(),
                            "Population Living in Cities vs Rural Areas by Region");
                        break;

                    case "9":
                        DisplayUrbanRuralReport(
                            dbManager.GetCityVsRuralPopulationByCountry(),
                            "Population Living in Cities vs Rural Areas by Country");
                        break;

                    case "10":
                        exit = true;
                        continue;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nPress any key to return to the Population Reports menu...");
                    Console.ReadKey();
                }
            }
        }

        private void GenerateWorldPopulationReport()
        {
            Console.Clear();
            long population = dbManager.GetWorldPopulation();

            Console.WriteLine("\nWorld Population Report");
            Console.WriteLine("=====================");
            Console.WriteLine($"Total World Population: {population.ToString("#,##0")}");
        }

        private void GenerateContinentPopulationReport()
        {
            Console.Clear();
            Console.WriteLine("Continent Population Report");
            Console.WriteLine("==========================");

            // Get list of continents
            var continents = dbManager.GetAllContinents();
            if (continents.Count == 0)
            {
                Console.WriteLine("No continents found in the database.");
                return;
            }

            // Display continent options
            Console.WriteLine("Available continents:");
            for (int i = 0; i < continents.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {continents[i]["Continent"]}");
            }

            // Get user selection
            Console.Write("\nSelect a continent (enter number): ");
            if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= continents.Count)
            {
                string selectedContinent = continents[selection - 1]["Continent"].ToString();
                var populationData = dbManager.GetContinentPopulation(selectedContinent);
                DisplaySinglePopulationReport(populationData, $"Population of {selectedContinent}");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void GenerateRegionPopulationReport()
        {
            Console.Clear();
            Console.WriteLine("Region Population Report");
            Console.WriteLine("=======================");

            // Get list of regions
            var regions = dbManager.GetAllRegions();
            if (regions.Count == 0)
            {
                Console.WriteLine("No regions found in the database.");
                return;
            }

            // Display region options
            Console.WriteLine("Available regions:");
            for (int i = 0; i < regions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {regions[i]["Region"]}");
            }

            // Get user selection
            Console.Write("\nSelect a region (enter number): ");
            if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= regions.Count)
            {
                string selectedRegion = regions[selection - 1]["Region"].ToString();
                var populationData = dbManager.GetRegionPopulation(selectedRegion);
                DisplaySinglePopulationReport(populationData, $"Population of {selectedRegion} Region");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void GenerateCountryPopulationReport()
        {
            Console.Clear();
            Console.WriteLine("Country Population Report");
            Console.WriteLine("========================");

            // Get list of countries
            var countries = dbManager.GetAllCountries();
            if (countries.Count == 0)
            {
                Console.WriteLine("No countries found in the database.");
                return;
            }

            // Display country options (first 20 for UI simplicity)
            Console.WriteLine("Available countries (first 20 shown):");
            for (int i = 0; i < Math.Min(countries.Count, 20); i++)
            {
                Console.WriteLine($"{i + 1}. {countries[i]["Name"]}");
            }

            // Allow typing country name for more flexibility
            Console.Write("\nEnter country name or selection number: ");
            string input = Console.ReadLine();

            string selectedCountry = null;

            // Check if input is a number (selection)
            if (int.TryParse(input, out int selection) && selection > 0 && selection <= Math.Min(countries.Count, 20))
            {
                selectedCountry = countries[selection - 1]["Name"].ToString();
            }
            else
            {
                // Input is a country name
                selectedCountry = input;
            }

            var populationData = dbManager.GetCountryPopulation(selectedCountry);

            if (populationData != null)
            {
                DisplaySinglePopulationReport(populationData, $"Population of {selectedCountry}");
            }
            else
            {
                Console.WriteLine($"Country '{selectedCountry}' not found.");
            }
        }

        private void GenerateDistrictPopulationReport()
        {
            Console.Clear();
            Console.WriteLine("District Population Report");
            Console.WriteLine("=========================");

            // Get list of districts
            var districts = dbManager.GetAllDistricts();
            if (districts.Count == 0)
            {
                Console.WriteLine("No districts found in the database.");
                return;
            }

            // Districts can be numerous, allow typing or selecting from first 20
            Console.WriteLine("Available districts (first 20 shown):");
            for (int i = 0; i < Math.Min(districts.Count, 20); i++)
            {
                Console.WriteLine($"{i + 1}. {districts[i]["District"]}");
            }

            // Allow typing district name for more flexibility
            Console.Write("\nEnter district name or selection number: ");
            string input = Console.ReadLine();

            string selectedDistrict = null;

            // Check if input is a number (selection)
            if (int.TryParse(input, out int selection) && selection > 0 && selection <= Math.Min(districts.Count, 20))
            {
                selectedDistrict = districts[selection - 1]["District"].ToString();
            }
            else
            {
                // Input is a district name
                selectedDistrict = input;
            }

            var populationData = dbManager.GetDistrictPopulation(selectedDistrict);

            if (populationData != null)
            {
                DisplaySinglePopulationReport(populationData, $"Population of {selectedDistrict} District");
            }
            else
            {
                Console.WriteLine($"District '{selectedDistrict}' not found.");
            }
        }

        private void GenerateCityPopulationReport()
        {
            Console.Clear();
            Console.WriteLine("City Population Report");
            Console.WriteLine("=====================");

            // Allow user to enter city name directly
            Console.Write("Enter city name: ");
            string cityName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(cityName))
            {
                var populationData = dbManager.GetCityPopulation(cityName);

                if (populationData != null)
                {
                    DisplaySinglePopulationReport(populationData, $"Population of {cityName}");
                }
                else
                {
                    Console.WriteLine($"City '{cityName}' not found.");
                }
            }
            else
            {
                Console.WriteLine("City name cannot be empty.");
            }
        }
    }
}