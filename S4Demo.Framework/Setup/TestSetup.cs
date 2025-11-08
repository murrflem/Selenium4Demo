using NLog;
using OpenQA.Selenium;
using System;
using System.IO;

namespace S4Demo.Framework.Setup
{
  public class TestSetup
  {
    public string BaseUrl = "";
    public string CurrentBrowser = "";
    public static string ScreenshotsMainFolder = "";
    public static string ExceptionScreenshotsFolder = "";
    public static string PageScreenshotsFolder = "";
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public IWebDriver SetupTest()
    {
      var runOnGrid = TestSettings.RunOnGrid;
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
      catch(Exception ex)
      {
        Logger.Error(ex.Message);
      }
      return null;
    }

    public void QuitDriver(IWebDriver browser)
    {
      try
      {
        browser.Quit();
      }
      catch(Exception ex)
      {
        Logger.Error(ex.Message);
      }
    }

    private IWebDriver CreateWebDriver(string browserUnderTest, bool runOnGrid)
    {
      var uri = "http://" + TestSettings.GridHubIp + ":" + TestSettings.GridPort + "/wd/hub/";
      var gridUri = new Uri(uri);
      return new DriverFactory().GetBrowserOptions(browserUnderTest, runOnGrid, gridUri);
    }

    private string GetDesiredBrowser()
    {
      return TestSettings.TestedBrowser;
    }

    private void CreateScreenshotFolders()
    {
      ScreenshotsMainFolder = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\Screenshots\";
      ExceptionScreenshotsFolder = ScreenshotsMainFolder + "\\ErrorScreenshots";
      PageScreenshotsFolder = ScreenshotsMainFolder + "\\PageScreenshots";

      try
      {
        Directory.CreateDirectory(ScreenshotsMainFolder);
        Directory.CreateDirectory(ExceptionScreenshotsFolder);
        Directory.CreateDirectory(PageScreenshotsFolder);
      }
      catch(Exception ex)
      {
        Logger.Error(ex.Message);
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