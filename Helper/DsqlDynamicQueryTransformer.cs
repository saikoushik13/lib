using System;
using System.Text.RegularExpressions;

namespace Helpers
{
    public static class DsqlDynamicQueryTransformer
    {
        public static string Transform(string dsql)
        {
            if (string.IsNullOrWhiteSpace(dsql))
                return "true"; // No filtering if dsql is empty

            // Replace curly quotes with standard quotes.
            string query = dsql.Replace("“", "\"").Replace("”", "\"");

            // Trim extra whitespace.
            query = query.Trim();

            // Convert logical operators to lowercase for Dynamic LINQ.
            query = Regex.Replace(query, @"\bAND\b", "and", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"\bOR\b", "or", RegexOptions.IgnoreCase);

            // Transform the 'in' operator.
            query = Regex.Replace(query, @"(\w+)\s+in\s+(\[[^\]]+\])", @"new []$2.Contains($1)", RegexOptions.IgnoreCase);

            // First: Replace single '=' with '==' for all remaining cases.
            query = Regex.Replace(query, @"(?<![!><])=(?!=)", "==", RegexOptions.IgnoreCase);

            // Then, custom conversion for genre:
            // Match genre== "value" (case-insensitive) and replace it with Genres.Any(Name=="value")
            query = Regex.Replace(query, @"\bgenre\s*==\s*(\""[^\""]+\"")", "Genres.Any(Name==$1)", RegexOptions.IgnoreCase);

            // Custom conversion for rating:
            // Match rating==<number> and replace it with Reviews.Any(Rating==<number>)
            query = Regex.Replace(query, @"\brating\s*==\s*(\d+)", "Reviews.Any(Rating==$1)", RegexOptions.IgnoreCase);

            var transformedQuery = query.Trim();
            Console.WriteLine("Transformed Query: " + transformedQuery);
            return transformedQuery;
        }
    }
}
