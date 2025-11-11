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

    public static bool EnableScreenshots =>
        bool.Parse(_config["TestSettings:EnableScreenshots"]);

    public static bool HighlightElements =>
        bool.Parse(_config["TestSettings:HighlightElements"]);

    public static bool RunOnGrid =>
        bool.Parse(_config["TestSettings:RunOnGrid"]);

    public static string TestSiteUrl =>
        _config["TestSettings:TestSiteUrl"];

    public static string TestedBrowser =>
        _config["TestSettings:TestedBrowser"];

    public static string GridHubIp =>
        _config["TestSettings:GridHubIp"];

    public static string GridPort =>
        _config["TestSettings:GridPort"];

    public static int DefaultTimeoutInSeconds =>
        int.Parse(_config["TestSettings:DefaultTimeoutInSeconds"]);

    public static int PollingIntervalMs =>
        int.Parse(_config["TestSettings:PollingIntervalMs"]);
  }
}