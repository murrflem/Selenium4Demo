using NUnit.Framework;
using OpenQA.Selenium;
using S4Demo.Framework.Helpers;
using System;

namespace S4Demo.Framework.Setup
{
    [TestFixture]
    public abstract class FrameworkBaseTest
    {
        private readonly TestSetup setup;
        protected IWebDriver WebDriver;

        protected FrameworkBaseTest()
        {
            setup = new TestSetup();
        }

        [SetUp]
        public void SetupTest()
        {
          FrameworkLogger.LogInfo($"TEST SETUP - {TestContext.CurrentContext.Test.Name}");
          // Verify ConfigManager is reading from appsettings.json
          FrameworkLogger.LogInfo($"Loaded Base URL from config: {ConfigManager.TestSiteUrl}");
          try
          {
            WebDriver = setup.SetupTest();
            LoadInitialPage();
          }
          catch (Exception ex)
          {
            FrameworkLogger.LogError($"FAILURE INITIALIZING TEST: {ex.Message}");
            throw; // Important: bubble up to fail the test cleanly
          }
        }

        [TearDown]
        public void TestCleanUp()
        {
            try
            {
                var testName = TestContext.CurrentContext.Test.Name;
                FrameworkLogger.LogInfo($"TEST TEARDOWN - {testName}");
                var testFailed = TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed;

                if (testFailed && WebDriver != null)
                {
                    ScreenshotOnError(testName);
                    FrameworkLogger.LogError(TestContext.CurrentContext.Result.Message);
                }
            }
            catch (Exception ex)
            {
                FrameworkLogger.LogError($"Error during teardown: {ex.Message}");
            }
            finally
            {
                setup.QuitDriver(WebDriver);
                WebDriver = null;
            }
        }

        private void ScreenshotOnError(string testName)
        {
            if (WebDriver == null) return;

            try
            {
                var browser = setup.CurrentBrowser;
                var screenshot = ((ITakesScreenshot)WebDriver).GetScreenshot();
                var fName = S4Demo.Framework.Setup.TestSetup.ExceptionScreenshotsFolder + "\\" + "Error_" + testName + "_" + browser + ".png";
                screenshot.SaveAsFile(fName);
                FrameworkLogger.LogInfo($"Saved screenshot: {fName}");
            }
            catch (Exception ex)
            {
                FrameworkLogger.LogWarning($"Unable to capture screenshot: {ex.Message}");
            }
        }

        protected abstract void LoadInitialPage();
    }
}