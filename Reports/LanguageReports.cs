using System;
using System.Collections.Generic;
using PopulationReport.Database;

namespace PopulationReport.Reports
{
    public class LanguageReports
    {
        private readonly DatabaseManager dbManager;

        public LanguageReports(DatabaseManager manager)
        {
            dbManager = manager;
        }

        // Display language statistics in a tabular format
        private void DisplayLanguageStatistics(List<Dictionary<string, object>> languageData)
        {
            Console.Clear();
            Console.WriteLine("\nLanguage Speaker Statistics Report");
            Console.WriteLine("================================");

            if (languageData == null || languageData.Count == 0)
            {
                Console.WriteLine("No language data to display.");
                return;
            }

            // Define column widths
            int languageWidth = 20;
            int speakersWidth = 25;
            int percentageWidth = 20;

            // Print header
            Console.WriteLine($"{"Language".PadRight(languageWidth)} " +
                             $"{"Total Speakers".PadRight(speakersWidth)} " +
                             $"{"World Percentage".PadRight(percentageWidth)}");

            Console.WriteLine(new string('-', languageWidth + speakersWidth + percentageWidth + 2));

            // Print each row
            foreach (var language in languageData)
            {
                string languageName = language["Language"]?.ToString() ?? "Unknown";
                double speakers = Convert.ToDouble(language["TotalSpeakers"]);
                double percentage = Convert.ToDouble(language["WorldPercentage"]);

                // Format with thousand separators and percentages
                string speakersStr = speakers.ToString("#,##0");
                string percentageStr = $"{percentage:F2}%";

                Console.WriteLine($"{languageName.PadRight(languageWidth)} " +
                                 $"{speakersStr.PadLeft(speakersWidth - 2)}  " +
                                 $"{percentageStr.PadLeft(percentageWidth - 2)}  ");
            }
        }

        public void GenerateReport()
        {
            Console.WriteLine("\nGenerating Language Speaker Statistics Report...");

            var languageData = dbManager.GetLanguageStatistics();
            DisplayLanguageStatistics(languageData);

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }
    }
}