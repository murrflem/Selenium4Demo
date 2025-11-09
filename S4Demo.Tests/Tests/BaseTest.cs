using NLog;
using S4Demo.Framework.Setup;
using S4Demo.Tests.Pages;
using System.Net.Http;

namespace S4Demo.Tests.Tests
{
  public class BaseTest : FrameworkBaseTest
  {
    protected BasePage CurrentPage;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public BaseTest()
    {

    }

    protected override void LoadInitialPage()
    {
      var testEnvironment = TestSettings.TestSiteUrl;
      new TestSetup().BaseUrl = testEnvironment;
      WebDriver.Url = testEnvironment;
      CurrentPage = new BasePage(WebDriver);
      CheckPageStatus(WebDriver.Url);
    }

    private void CheckPageStatus(string url)
    {
      try
      {
        using var httpClient = new HttpClient();
        var response = httpClient.GetAsync(url).GetAwaiter().GetResult();
        response.EnsureSuccessStatusCode();
      }
      catch (HttpRequestException ex)
      {
        Logger.Fatal(ex, $"EXCEPTION LOADING THE INITIAL PAGE: {url}");
      }
    }
  }
}