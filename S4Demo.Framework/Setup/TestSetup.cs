using OpenQA.Selenium;
using S4Demo.Framework.Helpers;
using System;
using System.IO;

namespace S4Demo.Framework.Setup
{
  public class TestSetup
  {
    public string BaseUrl = "";
    public string CurrentBrowser = "";

    // Folders
    public static string BaseFolder { get; private set; }
    public static string ScreenshotsMainFolder = "";
    public static string ExceptionScreenshotsFolder = "";
    public static string PageScreenshotsFolder = "";

    public IWebDriver SetupTest()
    {
      var runOnGrid = ConfigManager.RunOnGrid;
      try
      {
        var browserUnderTest = GetDesiredBrowser();
        CreateScreenshotFolders();
        var webDriver = CreateWebDriver(browserUnderTest, runOnGrid);
        SetBrowserWindowSize(webDriver);
        SetWaitTime(webDriver);
        CurrentBrowser = browserUnderTest;
        return webDriver;
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError(ex.Message);
      }
      return null;
    }

    public void QuitDriver(IWebDriver browser)
    {
      if (browser == null)
      {
        return;
      }

      try
      {
        browser.Quit();
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError(ex.Message);
      }
    }

    private IWebDriver CreateWebDriver(string browserUnderTest, bool runOnGrid)
    {
      var uri = "http://" + ConfigManager.GridHubIp + ":" + ConfigManager.GridPort + "/wd/hub/";
      var gridUri = new Uri(uri);
      return new DriverFactory().GetBrowserOptions(browserUnderTest, runOnGrid, gridUri);
    }

    private string GetDesiredBrowser()
    {
      return ConfigManager.TestedBrowser;
    }

    private void CreateScreenshotFolders()
    {
      // Base folder for all screenshots
      BaseFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Screenshots");
      ScreenshotsMainFolder = BaseFolder; // legacy
      ExceptionScreenshotsFolder = Path.Combine(ScreenshotsMainFolder, "ErrorScreenshots");
      PageScreenshotsFolder = Path.Combine(ScreenshotsMainFolder, "PageScreenshots");

      try
      {
        Directory.CreateDirectory(ScreenshotsMainFolder);
        Directory.CreateDirectory(ExceptionScreenshotsFolder);
        Directory.CreateDirectory(PageScreenshotsFolder);
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError(ex.Message);
      }
    }

    private void SetBrowserWindowSize(IWebDriver browser)
    {
      browser.Manage().Window.Maximize();
    }

    private void SetWaitTime(IWebDriver browser)
    {
      browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
      browser.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(2);
    }
  }
}