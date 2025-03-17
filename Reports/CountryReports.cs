using System;
using System.Collections.Generic;
using PopulationReport.Database;

namespace PopulationReport.Reports
{
    public class CountryReports
    {
        private readonly DatabaseManager dbManager;

        public CountryReports(DatabaseManager manager)
        {
            dbManager = manager;
        }

        // Display the report data in a tabular format
        private void DisplayCountryReport(List<Dictionary<string, object>> countries, string title)
        {
            Console.Clear();
            Console.WriteLine($"\n{title}");
            Console.WriteLine(new string('=', title.Length));

            if (countries == null || countries.Count == 0)
            {
                Console.WriteLine("No data to display.");
                return;
            }

            // Define column widths
            int codeWidth = 6;
            int nameWidth = 35;
            int continentWidth = 15;
            int regionWidth = 25;
            int populationWidth = 15;
            int capitalWidth = 25;

            // Print header
            Console.WriteLine($"{"Code".PadRight(codeWidth)} " +
                             $"{"Name".PadRight(nameWidth)} " +
                             $"{"Continent".PadRight(continentWidth)} " +
                             $"{"Region".PadRight(regionWidth)} " +
                             $"{"Population".PadRight(populationWidth)} " +
                             $"{"Capital".PadRight(capitalWidth)}");

            Console.WriteLine(new string('-', codeWidth + nameWidth + continentWidth + regionWidth + populationWidth + capitalWidth + 5));

            // Print each row
            foreach (var country in countries)
            {
                string code = country["Code"]?.ToString() ?? "N/A";
                string name = country["Name"]?.ToString() ?? "N/A";
                string continent = country["Continent"]?.ToString() ?? "N/A";
                string region = country["Region"]?.ToString() ?? "N/A";
                string population = country["Population"]?.ToString() ?? "0";
                string capital = country["Capital"]?.ToString() ?? "N/A";

                // Format population with thousand separators
                if (long.TryParse(population, out long popValue))
                {
                    population = popValue.ToString("#,##0");
                }

                // Truncate long strings to fit column width
                if (name.Length > nameWidth - 3) name = name.Substring(0, nameWidth - 3) + "...";
                if (continent.Length > continentWidth - 3) continent = continent.Substring(0, continentWidth - 3) + "...";
                if (region.Length > regionWidth - 3) region = region.Substring(0, regionWidth - 3) + "...";
                if (capital.Length > capitalWidth - 3) capital = capital.Substring(0, capitalWidth - 3) + "...";

                Console.WriteLine($"{code.PadRight(codeWidth)} " +
                                 $"{name.PadRight(nameWidth)} " +
                                 $"{continent.PadRight(continentWidth)} " +
                                 $"{region.PadRight(regionWidth)} " +
                                 $"{population.PadLeft(populationWidth - 2)}  " +
                                 $"{capital.PadRight(capitalWidth)}");
            }
            Console.WriteLine($"\nTotal: {countries.Count} countries");
        }

        public void GenerateReport()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Country Reports");
                Console.WriteLine("==============");
                Console.WriteLine("1. All countries in the world by population");
                Console.WriteLine("2. All countries in a continent by population");
                Console.WriteLine("3. All countries in a region by population");
                Console.WriteLine("4. Top N populated countries in the world");
                Console.WriteLine("5. Top N populated countries in a continent");
                Console.WriteLine("6. Top N populated countries in a region");
                Console.WriteLine("7. Return to main menu");
                Console.Write("\nEnter your choice (1-7): ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        DisplayCountryReport(
                            dbManager.GetAllCountriesByPopulation(),
                            "All Countries in the World Organized by Population");
                        break;

                    case "2":
                        GenerateCountriesByContinentReport();
                        break;

                    case "3":
                        GenerateCountriesByRegionReport();
                        break;

                    case "4":
                        GenerateTopNCountriesWorldReport();
                        break;

                    case "5":
                        GenerateTopNCountriesContinentReport();
                        break;

                    case "6":
                        GenerateTopNCountriesRegionReport();
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
                    Console.WriteLine("\nPress any key to return to the Country Reports menu...");
                    Console.ReadKey();
                }
            }
        }

        private void GenerateCountriesByContinentReport()
        {
            Console.Clear();
            Console.WriteLine("Countries by Continent");
            Console.WriteLine("=====================");

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
                var countries = dbManager.GetCountriesByContinent(selectedContinent);
                DisplayCountryReport(countries, $"Countries in {selectedContinent} Organized by Population");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void GenerateCountriesByRegionReport()
        {
            Console.Clear();
            Console.WriteLine("Countries by Region");
            Console.WriteLine("==================");

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
                var countries = dbManager.GetCountriesByRegion(selectedRegion);
                DisplayCountryReport(countries, $"Countries in {selectedRegion} Organized by Population");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void GenerateTopNCountriesWorldReport()
        {
            Console.Clear();
            Console.WriteLine("Top N Countries in the World");
            Console.WriteLine("==========================");

            // Get number of countries to display
            Console.Write("Enter the number of countries to display: ");
            if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
            {
                var countries = dbManager.GetTopNCountriesWorld(n);
                DisplayCountryReport(countries, $"Top {n} Countries in the World by Population");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a positive number.");
            }
        }

        private void GenerateTopNCountriesContinentReport()
        {
            Console.Clear();
            Console.WriteLine("Top N Countries in a Continent");
            Console.WriteLine("============================");

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

                Console.Write("Enter the number of countries to display: ");
                if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
                {
                    var countries = dbManager.GetTopNCountriesContinent(n, selectedContinent);
                    DisplayCountryReport(countries, $"Top {n} Countries in {selectedContinent} by Population");
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

        private void GenerateTopNCountriesRegionReport()
        {
            Console.Clear();
            Console.WriteLine("Top N Countries in a Region");
            Console.WriteLine("=========================");

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

                Console.Write("Enter the number of countries to display: ");
                if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
                {
                    var countries = dbManager.GetTopNCountriesRegion(n, selectedRegion);
                    DisplayCountryReport(countries, $"Top {n} Countries in {selectedRegion} by Population");
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