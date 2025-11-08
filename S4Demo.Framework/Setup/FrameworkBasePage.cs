using OpenQA.Selenium;
using S4Demo.Framework.Helpers;

namespace S4Demo.Framework.Setup
{
  public class FrameworkBasePage
  {
    public IWebDriver WebDriver;
    public SeleniumHelper Selenium;
    public JavaScriptHelper JavaScriptHelper;
    public CommonHelper Common;
    public ScreenshotHelper ScreenshotHelper;
    public TestSetup Setup;

    protected FrameworkBasePage(IWebDriver webdriver)
    {
      WebDriver = webdriver;
      Selenium = new SeleniumHelper(WebDriver);
      JavaScriptHelper = new JavaScriptHelper(WebDriver);
      Common = new CommonHelper();
      ScreenshotHelper = new ScreenshotHelper(WebDriver);
      Setup = new TestSetup();
    }

    public virtual void NavigateToUrl(string url)
    {
      new WaitHelper(WebDriver).WaitForPageToLoad();
      WebDriver.Navigate().GoToUrl(Setup.BaseUrl + url);
    }
  }
}