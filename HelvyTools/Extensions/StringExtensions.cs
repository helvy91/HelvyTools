using System.Text.RegularExpressions;

namespace HelvyTools.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static string Join(this IEnumerable<string> values, string separator)
        {
            return string.Join(separator, values);
        }

        public static bool EqualsIgnoreCase(this string value, string valueToCompare)
        {
            return value.Equals(valueToCompare, StringComparison.OrdinalIgnoreCase);
        }

        public static bool ContainsIgnoreCase(this string value, string valueToCompare)
        {
            return value.Contains(valueToCompare, StringComparison.OrdinalIgnoreCase);
        }

        public static string RemoveNewLines(this string value)
        {
            return Regex.Replace(value, @"\t|\n|\r", "").Trim();
        }
    }
}
