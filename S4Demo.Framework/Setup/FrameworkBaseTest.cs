using NLog;
using NUnit.Framework;
using OpenQA.Selenium;
using System;

namespace S4Demo.Framework.Setup
{
    [TestFixture]
    public abstract class FrameworkBaseTest
    {
        private readonly TestSetup setup;
        protected IWebDriver WebDriver;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected FrameworkBaseTest()
        {
            setup = new TestSetup();
        }

        [SetUp]
        public void TestSetup()
        {
            Logger.Info($"TEST SETUP - {TestContext.CurrentContext.Test.Name}");
            try
            {
                WebDriver = setup.SetupTest();
                LoadInitialPage();
            }
            catch (Exception ex)
            {
                Logger.Error($"FAILURE INITIALIZING TEST: {ex.Message}");
                throw; // Important: bubble up to fail the test cleanly
            }
        }

        [TearDown]
        public void TestCleanUp()
        {
            try
            {
                var testName = TestContext.CurrentContext.Test.Name;
                Logger.Info($"TEST TEARDOWN - {testName}");

                var testFailed = TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed;

                if (testFailed && WebDriver != null)
                {
                    ScreenshotOnError(testName);
                    Logger.Error(TestContext.CurrentContext.Result.Message);
                }
            }
            catch (Exception ex)
            {
                Logger.Warn($"Error during teardown: {ex.Message}");
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
                Logger.Info($"Saved screenshot: {fName}");
            }
            catch (Exception ex)
            {
                Logger.Warn($"Unable to capture screenshot: {ex.Message}");
            }
        }

        protected abstract void LoadInitialPage();
    }
}