using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace S4Demo.Framework.Helpers
{
  public static class TestDataHelper
  {
    /// <summary>
    /// Loads a CSV file and maps each row to a strong-typed model.
    /// Adds basic validation for missing or empty files.
    /// </summary>
    public static List<T> LoadCsv<T>(string filePath)
    {
      if (!File.Exists(filePath))
      {
        throw new FileNotFoundException(
            $"CSV file not found at: {filePath}. " +
            $"Make sure the file exists and is set to 'Copy if newer'.");
      }

      using var reader = new StreamReader(filePath);
      using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

      var records = csv.GetRecords<T>().ToList();

      if (!records.Any())
      {
        throw new InvalidDataException(
            $"CSV file '{filePath}' is empty or contains no data rows.");
      }

      return records;
    }

    // Loads a CSV file and returns a single, randomly selected row
    public static T LoadRandomCsvRow<T>(string filePath)
    {
      var allRows = LoadCsv<T>(filePath);

      var random = new Random();
      var index = random.Next(allRows.Count);

      return allRows[index];
    }

  }
}