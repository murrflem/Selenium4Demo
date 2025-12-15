using ClosedXML.Excel;
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

    /// <summary>
    /// Loads all rows from an Excel worksheet and maps each row
    /// to a strong-typed model using the header row for mapping.
    /// </summary>
    public static List<T> LoadExcel<T>(string filePath, string worksheetName)
        where T : new()
    {
      if (!File.Exists(filePath))
      {
        throw new FileNotFoundException(
            $"Excel file not found at: {filePath}");
      }

      using var workbook = new XLWorkbook(filePath);
      var worksheet = workbook.Worksheet(worksheetName);

      var rows = worksheet.RowsUsed().ToList();

      if (rows.Count < 2)
      {
        throw new InvalidDataException(
            $"Worksheet '{worksheetName}' in file '{filePath}' contains no data rows.");
      }

      // Read header row
      var headerRow = rows.First();
      var headers = headerRow.Cells()
          .Select((cell, index) => new
          {
            Name = cell.GetString(),
            Index = index + 1
          })
          .ToDictionary(x => x.Name, x => x.Index);

      var results = new List<T>();

      // Read data rows
      foreach (var row in rows.Skip(1))
      {
        var item = new T();

        foreach (var prop in typeof(T).GetProperties())
        {
          if (!headers.TryGetValue(prop.Name, out int columnIndex))
            continue;

          var cellValue = row.Cell(columnIndex).GetString();

          if (string.IsNullOrWhiteSpace(cellValue))
            continue;

          prop.SetValue(item, Convert.ChangeType(cellValue, prop.PropertyType));
        }

        results.Add(item);
      }

      return results;
    }

    /// <summary>
    /// Loads an Excel worksheet and returns a single, randomly selected row
    /// mapped to the specified strong-typed model.
    /// </summary>
    public static T LoadRandomExcelRow<T>(string filePath, string worksheetName)
        where T : new()
    {
      var rows = LoadExcel<T>(filePath, worksheetName);

      var random = new Random();
      var index = random.Next(rows.Count);

      return rows[index];
    }
  }
}