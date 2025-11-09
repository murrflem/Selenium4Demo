using System;
using System.Linq;

namespace S4Demo.Framework.Helpers
{
  /// <summary>
  /// Provides utility methods for generating random strings, numbers, and common test data.
  /// </summary>
  public static class CommonHelper
  {
    private static readonly Random Random = new();

    /// <summary>
    /// Generates a random string of lowercase or mixed-case letters, optionally with a prefix or suffix.
    /// </summary>
    public static string RandomString(int length, bool includeUppercase = false, string prefix = "", string suffix = "")
    {
      var chars = "abcdefghijklmnopqrstuvwxyz";
      if (includeUppercase)
        chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

      var randomPart = new string(Enumerable.Repeat(chars, length)
        .Select(s => s[Random.Next(s.Length)]).ToArray());

      return $"{prefix}{randomPart}{suffix}";
    }

    /// <summary>
    /// Generates a random numeric string of the specified length, optionally with a prefix or suffix.
    /// </summary>
    public static string RandomNumericString(int length, string prefix = "", string suffix = "")
    {
      const string numbers = "0123456789";
      var randomPart = new string(Enumerable.Repeat(numbers, length)
        .Select(s => s[Random.Next(s.Length)]).ToArray());

      return $"{prefix}{randomPart}{suffix}";
    }

    /// <summary>
    /// Generates a random alphanumeric string (letters and digits), optionally with a prefix or suffix.
    /// </summary>
    public static string AlphaNumericString(int length, bool includeUppercase = false, string prefix = "", string suffix = "")
    {
      var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
      if (includeUppercase)
        chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

      var randomPart = new string(Enumerable.Repeat(chars, length)
        .Select(s => s[Random.Next(s.Length)]).ToArray());

      return $"{prefix}{randomPart}{suffix}";
    }

    /// <summary>
    /// Generates a random integer within the specified range (inclusive lower, exclusive upper bound).
    /// </summary>
    public static int RandomInt(int minValue, int maxValue)
    {
      return Random.Next(minValue, maxValue);
    }

    /// <summary>
    /// Generates a string with a timestamp appended (format: yyyyMMdd_HHmmss).
    /// Useful for creating unique identifiers in test data.
    /// </summary>
    public static string TimestampedString(string baseText, bool includeUppercase = false)
    {
      var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
      var randomSuffix = AlphaNumericString(4, includeUppercase);
      return $"{baseText}_{timestamp}_{randomSuffix}";
    }

    /// <summary>
    /// Generates a random email address suitable for test data.
    /// Example output: "x7tb3q@demoqa.com"
    /// </summary>
    /// <param name="domain">Optional domain name (default: "example.com").</param>
    public static string RandomEmail(string domain = "example.com")
    {
      // Use small random components to avoid overly long addresses
      var localPart = AlphaNumericString(8, true);
      var hostPart = AlphaNumericString(5, false);

      // If the user specifies a domain, respect it. Otherwise, generate one dynamically.
      var selectedDomain = string.IsNullOrWhiteSpace(domain)
        ? $"{hostPart}.com"
        : domain;

      return $"{localPart}@{selectedDomain}";
    }
  }
}