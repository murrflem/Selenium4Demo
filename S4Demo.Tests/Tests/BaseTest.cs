using NUnit.Framework;
using NUnit.Framework.Interfaces;
using S4Demo.Framework.Helpers;
using S4Demo.Framework.Setup;
using S4Demo.Tests.Pages;
using System;
using System.Diagnostics;
using System.Net.Http;

namespace S4Demo.Tests.Tests
{
  public class BaseTest : FrameworkBaseTest
  {
    protected BasePage CurrentPage;
    private Stopwatch _testStopwatch;

    [SetUp]
    public void BaseSetUp()
    {
      _testStopwatch = Stopwatch.StartNew();

      var testName = TestContext.CurrentContext.Test.Name;
      var browser = WebDriver?.GetType().Name ?? "Unknown Browser";
      var environment = ConfigManager.TestSiteUrl ?? "Unknown";

      FrameworkLogger.LogTestStart(testName, browser, environment);
    }

    [TearDown]
    public void BaseTearDown()
    {
      _testStopwatch.Stop();

      var result = TestContext.CurrentContext.Result.Outcome.Status;
      var testName = TestContext.CurrentContext.Test.Name;
      var duration = _testStopwatch.Elapsed;

      switch (result)
      {
        case TestStatus.Passed:
          FrameworkLogger.LogTestPass(testName, duration);
          break;

        case TestStatus.Failed:
          var message = TestContext.CurrentContext.Result.Message;
          var stackTrace = TestContext.CurrentContext.Result.StackTrace;
          var ex = new Exception(message + Environment.NewLine + stackTrace);
          FrameworkLogger.LogTestFail(testName, ex, duration);
          break;

        default:
          FrameworkLogger.LogWarning($"[TEST {result}] {testName} finished with status {result}");
          break;
      }
    }

    protected override void LoadInitialPage()
    {
      var testEnvironment = ConfigManager.TestSiteUrl;
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
        FrameworkLogger.LogError($"EXCEPTION LOADING INITIAL PAGE: {url}", ex);
      }
    }
  }
}