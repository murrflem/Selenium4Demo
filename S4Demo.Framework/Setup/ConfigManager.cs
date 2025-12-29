using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace S4Demo.Framework.Setup
{
  public static class ConfigManager
  {
    private static readonly IConfigurationRoot _config;

    static ConfigManager()
    {
      // Look directly inside the Framework\Setup folder relative to the solution root
      var basePath = Path.Combine(
          AppDomain.CurrentDomain.BaseDirectory,
          @"..\..\..\..\S4Demo.Framework\Setup"
      );

      _config = new ConfigurationBuilder()
          .SetBasePath(basePath)
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .Build();
    }

    // Environment ======================================================================
    public static string TestSiteUrl => _config["Environment:TestSiteUrl"];
    public static bool RunOnGrid => bool.Parse(_config["Environment:RunOnGrid"]);
    public static string GridHubIp => _config["Environment:GridHubIp"];
    public static string GridPort => _config["Environment:GridPort"];

    // Browser ==========================================================================
    public static string TestedBrowser => _config["Browser:TestedBrowser"];
    public static bool HighlightElements => bool.Parse(_config["Browser:HighlightElements"]);

    // Screenshots ======================================================================
    public static bool EnableScreenshots => bool.Parse(_config["Screenshots:EnableScreenshots"]);

    public static int VisualComparisonTolerance
    {
      get
      {
        if (int.TryParse(_config["VisualComparison:PixelTolerance"], out int value))
          return value;
        return 10;
      }
    }

    public static bool FailOnVisualDifference
    {
      get
      {
        if (bool.TryParse(_config["VisualComparison:FailOnDifference"], out bool value))
          return value;
        return false;
      }
    }

    // Framework Logging ================================================================
    public static string LoggingMode => _config["FrameworkLogging:Mode"] ?? "AllTests";
    public static int MaxLogsPerTest
    {
      get
      {
        if (int.TryParse(_config["FrameworkLogging:MaxLogsPerTest"], out int value))
          return value;
        return 5; // default fallback
      }
    }

    // Timeouts =========================================================================
    public static int DefaultTimeoutInSeconds => int.Parse(_config["Timeouts:DefaultTimeoutInSeconds"]);
    public static int PollingIntervalMs => int.Parse(_config["Timeouts:PollingIntervalMs"]);

    public static int HttpTimeoutSeconds
    {
      get
      {
        if (int.TryParse(_config["HttpSettings:TimeoutSeconds"], out int value))
          return value;

        return 10; // fallback default
      }
    }

    public static int MaxExpectedResponseTimeMs
    {
      get
      {
        if (int.TryParse(_config["HttpSettings:MaxExpectedResponseTimeMs"], out int value))
          return value;

        return 2000; // fallback default
      }
    }

    public static bool HttpFilterByDomain
    {
      get
      {
        if (bool.TryParse(_config["HttpSettings:FilterByDomain"], out bool value))
          return value;
        return true; // fallback default
      }
    }

    // Retry ============================================================================
    public static int HttpRetryCount
    {
      get
      {
        if (int.TryParse(_config["HttpSettings:RetryCount"], out int value))
          return value;
        return 0;
      }
    }

    public static int HttpRetryDelayMs
    {
      get
      {
        if (int.TryParse(_config["HttpSettings:RetryDelayMs"], out int value))
          return value;
        return 0;
      }
    }
  }
}