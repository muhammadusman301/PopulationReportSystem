using System;
using System.Collections.Generic;
using PopulationReport.Database;

namespace PopulationReport.Reports
{
    public class CityReports
    {
        private readonly Database.DatabaseManager dbManager;

        public CityReports(Database.DatabaseManager manager)
        {
            dbManager = manager;
        }

        // Display the report data in a tabular format
        private void DisplayCityReport(List<Dictionary<string, object>> cities, string title)
        {
            Console.Clear();
            Console.WriteLine($"\n{title}");
            Console.WriteLine(new string('=', title.Length));

            if (cities == null || cities.Count == 0)
            {
                Console.WriteLine("No data to display.");
                return;
            }

            // Define column widths
            int nameWidth = 35;
            int countryWidth = 30;
            int districtWidth = 25;
            int populationWidth = 15;

            // Print header
            Console.WriteLine($"{"Name".PadRight(nameWidth)} " +
                             $"{"Country".PadRight(countryWidth)} " +
                             $"{"District".PadRight(districtWidth)} " +
                             $"{"Population".PadRight(populationWidth)}");

            Console.WriteLine(new string('-', nameWidth + countryWidth + districtWidth + populationWidth + 3));

            // Print each row
            foreach (var city in cities)
            {
                string name = city["Name"]?.ToString() ?? "N/A";
                string country = city["Country"]?.ToString() ?? "N/A";
                string district = city["District"]?.ToString() ?? "N/A";
                string population = city["Population"]?.ToString() ?? "0";

                // Format population with thousand separators
                if (long.TryParse(population, out long popValue))
                {
                    population = popValue.ToString("#,##0");
                }

                // Truncate long strings to fit column width
                if (name.Length > nameWidth - 3) name = name.Substring(0, nameWidth - 3) + "...";
                if (country.Length > countryWidth - 3) country = country.Substring(0, countryWidth - 3) + "...";
                if (district.Length > districtWidth - 3) district = district.Substring(0, districtWidth - 3) + "...";

                Console.WriteLine($"{name.PadRight(nameWidth)} " +
                                 $"{country.PadRight(countryWidth)} " +
                                 $"{district.PadRight(districtWidth)} " +
                                 $"{population.PadLeft(populationWidth - 2)}  ");
            }
            Console.WriteLine($"\nTotal: {cities.Count} cities");
        }

        public void GenerateReport()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("City Reports");
                Console.WriteLine("===========");
                Console.WriteLine("1. All cities in the world by population");
                Console.WriteLine("2. All cities in a continent by population");
                Console.WriteLine("3. All cities in a region by population");
                Console.WriteLine("4. All cities in a country by population");
                Console.WriteLine("5. All cities in a district by population");
                Console.WriteLine("6. Top N populated cities in the world");
                Console.WriteLine("7. Top N populated cities in a continent");
                Console.WriteLine("8. Top N populated cities in a region");
                Console.WriteLine("9. Top N populated cities in a country");
                Console.WriteLine("10. Top N populated cities in a district");
                Console.WriteLine("11. Return to main menu");
                Console.Write("\nEnter your choice (1-11): ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        DisplayCityReport(
                            dbManager.GetAllCitiesByPopulation(),
                            "All Cities in the World Organized by Population");
                        break;

                    case "2":
                        GenerateCitiesByContinentReport();
                        break;

                    case "3":
                        GenerateCitiesByRegionReport();
                        break;

                    case "4":
                        GenerateCitiesByCountryReport();
                        break;

                    case "5":
                        GenerateCitiesByDistrictReport();
                        break;

                    case "6":
                        GenerateTopNCitiesWorldReport();
                        break;

                    case "7":
                        GenerateTopNCitiesContinentReport();
                        break;

                    case "8":
                        GenerateTopNCitiesRegionReport();
                        break;

                    case "9":
                        GenerateTopNCitiesCountryReport();
                        break;

                    case "10":
                        GenerateTopNCitiesDistrictReport();
                        break;

                    case "11":
                        exit = true;
                        continue;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nPress any key to return to the City Reports menu...");
                    Console.ReadKey();
                }
            }
        }

        private void GenerateCitiesByContinentReport()
        {
            Console.Clear();
            Console.WriteLine("Cities by Continent");
            Console.WriteLine("==================");

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
                var cities = dbManager.GetCitiesByContinent(selectedContinent);
                DisplayCityReport(cities, $"Cities in {selectedContinent} Organized by Population");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void GenerateCitiesByRegionReport()
        {
            Console.Clear();
            Console.WriteLine("Cities by Region");
            Console.WriteLine("===============");

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
                var cities = dbManager.GetCitiesByRegion(selectedRegion);
                DisplayCityReport(cities, $"Cities in {selectedRegion} Organized by Population");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void GenerateCitiesByCountryReport()
        {
            Console.Clear();
            Console.WriteLine("Cities by Country");
            Console.WriteLine("================");

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

            var cities = dbManager.GetCitiesByCountry(selectedCountry);

            if (cities.Count > 0)
            {
                DisplayCityReport(cities, $"Cities in {selectedCountry} Organized by Population");
            }
            else
            {
                Console.WriteLine($"No cities found for country '{selectedCountry}' or country does not exist.");
            }
        }

        private void GenerateCitiesByDistrictReport()
        {
            Console.Clear();
            Console.WriteLine("Cities by District");
            Console.WriteLine("=================");

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

            var cities = dbManager.GetCitiesByDistrict(selectedDistrict);

            if (cities.Count > 0)
            {
                DisplayCityReport(cities, $"Cities in {selectedDistrict} District Organized by Population");
            }
            else
            {
                Console.WriteLine($"No cities found for district '{selectedDistrict}' or district does not exist.");
            }
        }

        private void GenerateTopNCitiesWorldReport()
        {
            Console.Clear();
            Console.WriteLine("Top N Cities in the World");
            Console.WriteLine("=======================");

            // Get number of cities to display
            Console.Write("Enter the number of cities to display: ");
            if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
            {
                var cities = dbManager.GetTopNCitiesWorld(n);
                DisplayCityReport(cities, $"Top {n} Cities in the World by Population");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a positive number.");
            }
        }

        private void GenerateTopNCitiesContinentReport()
        {
            Console.Clear();
            Console.WriteLine("Top N Cities in a Continent");
            Console.WriteLine("=========================");

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

                Console.Write("Enter the number of cities to display: ");
                if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
                {
                    var cities = dbManager.GetTopNCitiesContinent(n, selectedContinent);
                    DisplayCityReport(cities, $"Top {n} Cities in {selectedContinent} by Population");
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

        private void GenerateTopNCitiesRegionReport()
        {
            Console.Clear();
            Console.WriteLine("Top N Cities in a Region");
            Console.WriteLine("======================");

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

                Console.Write("Enter the number of cities to display: ");
                if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
                {
                    var cities = dbManager.GetTopNCitiesRegion(n, selectedRegion);
                    DisplayCityReport(cities, $"Top {n} Cities in {selectedRegion} by Population");
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

        private void GenerateTopNCitiesCountryReport()
        {
            Console.Clear();
            Console.WriteLine("Top N Cities in a Country");
            Console.WriteLine("=======================");

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

            Console.Write("Enter the number of cities to display: ");
            if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
            {
                var cities = dbManager.GetTopNCitiesCountry(n, selectedCountry);

                if (cities.Count > 0)
                {
                    DisplayCityReport(cities, $"Top {n} Cities in {selectedCountry} by Population");
                }
                else
                {
                    Console.WriteLine($"No cities found for country '{selectedCountry}' or country does not exist.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a positive number.");
            }
        }

        private void GenerateTopNCitiesDistrictReport()
        {
            Console.Clear();
            Console.WriteLine("Top N Cities in a District");
            Console.WriteLine("========================");

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

            Console.Write("Enter the number of cities to display: ");
            if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
            {
                var cities = dbManager.GetTopNCitiesDistrict(n, selectedDistrict);

                if (cities.Count > 0)
                {
                    DisplayCityReport(cities, $"Top {n} Cities in {selectedDistrict} District by Population");
                }
                else
                {
                    Console.WriteLine($"No cities found for district '{selectedDistrict}' or district does not exist.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a positive number.");
            }
        }
    }
}