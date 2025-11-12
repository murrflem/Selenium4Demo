using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;
using S4Demo.Framework.Helpers;
using System;

namespace S4Demo.Framework.Setup
{
  public class DriverFactory
  {
    public IWebDriver GetBrowserOptions(string browserType, bool gridFlag, Uri gridUri)
    {
      try
      {
        var browser = (browserType ?? string.Empty).ToLowerInvariant();

        if (gridFlag)
        {
          return CreateRemoteDriver(browser, gridUri);
        }

        return CreateLocalDriver(browser);
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError("Error initializing browser driver", ex);
        throw;
      }
    }
    private IWebDriver CreateRemoteDriver(string browser, Uri gridUri)
    {
      switch (browser)
      {
        case "chrome-headless":
          var chromeOptionsHead = new ChromeOptions();
          chromeOptionsHead.AddArguments("--headless=new", "--disable-gpu");
          return new RemoteWebDriver(gridUri, chromeOptionsHead.ToCapabilities(), TimeSpan.FromSeconds(120));

        case "edge":
          var edgeOptionsGrid = new EdgeOptions();
          return new RemoteWebDriver(gridUri, edgeOptionsGrid.ToCapabilities(), TimeSpan.FromSeconds(120));

        default:
          var chromeOptionsDefault = new ChromeOptions();
          chromeOptionsDefault.AddArgument("--disable-notifications");
          return new RemoteWebDriver(gridUri, chromeOptionsDefault.ToCapabilities(), TimeSpan.FromSeconds(120));
      }
    }

    private IWebDriver CreateLocalDriver(string browser)
    {
      switch (browser)
      {
        case "chrome-headless":
          var chromeHeadless = new ChromeOptions();
          chromeHeadless.AddArguments("--headless=new", "--disable-gpu");
          return new ChromeDriver(chromeHeadless);

        case "edge":
          var edgeLocal = new EdgeOptions();
          return new EdgeDriver(edgeLocal);

        default:
          var chromeLocal = new ChromeOptions();
          chromeLocal.AddArgument("--disable-notifications");
          return new ChromeDriver(chromeLocal);
      }
    }
  }
}