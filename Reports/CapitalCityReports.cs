using System;
using System.Collections.Generic;
using PopulationReport.Database;

namespace PopulationReport.Reports
{
    public class CapitalCityReports
    {
        private readonly DatabaseManager dbManager;

        public CapitalCityReports(DatabaseManager manager)
        {
            dbManager = manager;
        }

        // Display the report data in a tabular format
        private void DisplayCapitalCityReport(List<Dictionary<string, object>> capitalCities, string title)
        {
            Console.Clear();
            Console.WriteLine($"\n{title}");
            Console.WriteLine(new string('=', title.Length));

            if (capitalCities == null || capitalCities.Count == 0)
            {
                Console.WriteLine("No data to display.");
                return;
            }

            // Define column widths
            int nameWidth = 35;
            int countryWidth = 35;
            int populationWidth = 15;

            // Print header
            Console.WriteLine($"{"Name".PadRight(nameWidth)} " +
                             $"{"Country".PadRight(countryWidth)} " +
                             $"{"Population".PadRight(populationWidth)}");

            Console.WriteLine(new string('-', nameWidth + countryWidth + populationWidth + 2));

            // Print each row
            foreach (var city in capitalCities)
            {
                string name = city["Name"]?.ToString() ?? "N/A";
                string country = city["Country"]?.ToString() ?? "N/A";
                string population = city["Population"]?.ToString() ?? "0";

                // Format population with thousand separators
                if (long.TryParse(population, out long popValue))
                {
                    population = popValue.ToString("#,##0");
                }

                // Truncate long strings to fit column width
                if (name.Length > nameWidth - 3) name = name.Substring(0, nameWidth - 3) + "...";
                if (country.Length > countryWidth - 3) country = country.Substring(0, countryWidth - 3) + "...";

                Console.WriteLine($"{name.PadRight(nameWidth)} " +
                                 $"{country.PadRight(countryWidth)} " +
                                 $"{population.PadLeft(populationWidth - 2)}  ");
            }
            Console.WriteLine($"\nTotal: {capitalCities.Count} capital cities");
        }

        public void GenerateReport()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Capital City Reports");
                Console.WriteLine("===================");
                Console.WriteLine("1. All capital cities in the world by population");
                Console.WriteLine("2. All capital cities in a continent by population");
                Console.WriteLine("3. All capital cities in a region by population");
                Console.WriteLine("4. Top N populated capital cities in the world");
                Console.WriteLine("5. Top N populated capital cities in a continent");
                Console.WriteLine("6. Top N populated capital cities in a region");
                Console.WriteLine("7. Return to main menu");
                Console.Write("\nEnter your choice (1-7): ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        DisplayCapitalCityReport(
                            dbManager.GetAllCapitalCitiesByPopulation(),
                            "All Capital Cities in the World Organized by Population");
                        break;

                    case "2":
                        GenerateCapitalCitiesByContinentReport();
                        break;

                    case "3":
                        GenerateCapitalCitiesByRegionReport();
                        break;

                    case "4":
                        GenerateTopNCapitalCitiesWorldReport();
                        break;

                    case "5":
                        GenerateTopNCapitalCitiesContinentReport();
                        break;

                    case "6":
                        GenerateTopNCapitalCitiesRegionReport();
                        break;

                    case "7":
                        exit = true;
                        continue;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nPress any key to return to the Capital City Reports menu...");
                    Console.ReadKey();
                }
            }
        }

        private void GenerateCapitalCitiesByContinentReport()
        {
            Console.Clear();
            Console.WriteLine("Capital Cities by Continent");
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
                var capitalCities = dbManager.GetCapitalCitiesByContinent(selectedContinent);
                DisplayCapitalCityReport(capitalCities, $"Capital Cities in {selectedContinent} Organized by Population");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void GenerateCapitalCitiesByRegionReport()
        {
            Console.Clear();
            Console.WriteLine("Capital Cities by Region");
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
                var capitalCities = dbManager.GetCapitalCitiesByRegion(selectedRegion);
                DisplayCapitalCityReport(capitalCities, $"Capital Cities in {selectedRegion} Organized by Population");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void GenerateTopNCapitalCitiesWorldReport()
        {
            Console.Clear();
            Console.WriteLine("Top N Capital Cities in the World");
            Console.WriteLine("===============================");

            // Get number of capital cities to display
            Console.Write("Enter the number of capital cities to display: ");
            if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
            {
                var capitalCities = dbManager.GetTopNCapitalCitiesWorld(n);
                DisplayCapitalCityReport(capitalCities, $"Top {n} Capital Cities in the World by Population");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a positive number.");
            }
        }

        private void GenerateTopNCapitalCitiesContinentReport()
        {
            Console.Clear();
            Console.WriteLine("Top N Capital Cities in a Continent");
            Console.WriteLine("=================================");

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

                Console.Write("Enter the number of capital cities to display: ");
                if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
                {
                    var capitalCities = dbManager.GetTopNCapitalCitiesContinent(n, selectedContinent);
                    DisplayCapitalCityReport(capitalCities, $"Top {n} Capital Cities in {selectedContinent} by Population");
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a positive number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void GenerateTopNCapitalCitiesRegionReport()
        {
            Console.Clear();
            Console.WriteLine("Top N Capital Cities in a Region");
            Console.WriteLine("==============================");

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

                Console.Write("Enter the number of capital cities to display: ");
                if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
                {
                    var capitalCities = dbManager.GetTopNCapitalCitiesRegion(n, selectedRegion);
                    DisplayCapitalCityReport(capitalCities, $"Top {n} Capital Cities in {selectedRegion} by Population");
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a positive number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }
    }
}