namespace S4Demo.Framework.Setup
{
  public static class TestSettings
  {
    public static bool EnableScreenshots { get; set; } = true;
    public const bool HighlightElements = true;
    public const bool RunOnGrid = false;
    public const string TestSiteUrl = "https://www.phptravels.net/";
    public const string TestedBrowser = "chrome";
    public const string GridHubIp = "192.168.1.30";
    public const string GridPort = "4444";
    public static int DefaultTimeoutInSeconds => 30;
    public static int PollingIntervalMs => 500;

  }
}