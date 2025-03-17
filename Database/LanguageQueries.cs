using System;
using System.Collections.Generic;

namespace PopulationReport.Database
{
    public partial class DatabaseManager
    {
        #region Language Queries

        // Get language statistics for specified languages
        public List<Dictionary<string, object>> GetLanguageStatistics()
        {
            string query = @"
                SELECT 
                    cl.Language,
                    SUM(co.Population * cl.Percentage / 100.0) AS TotalSpeakers,
                    (SUM(co.Population * cl.Percentage / 100.0) / (SELECT SUM(Population) FROM country)) * 100 AS WorldPercentage
                FROM countrylanguage cl
                JOIN country co ON cl.CountryCode = co.Code
                WHERE cl.Language IN ('Chinese', 'English', 'Hindi', 'Spanish', 'Arabic')
                GROUP BY cl.Language
                ORDER BY TotalSpeakers DESC";
            return ExecuteQuery(query);
        }

        #endregion
    }
}