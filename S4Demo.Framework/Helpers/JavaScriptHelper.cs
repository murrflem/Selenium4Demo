using NLog;
using OpenQA.Selenium;
using S4Demo.Framework.Setup;
using System;

namespace S4Demo.Framework.Helpers
{
  public class JavaScriptHelper
  {
    private readonly IWebDriver _webDriver;
    private WaitHelper _waitHelper;
    protected SeleniumHelper Selenium;
    private const string HighlightInGreen = "arguments[0].style.border='3px solid lime'";
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public JavaScriptHelper(IWebDriver webDriver)
    {
      _webDriver = webDriver;
      _waitHelper = new WaitHelper(webDriver);
      Selenium = new SeleniumHelper(webDriver);
    }

    public void ClickElement(IWebElement element)
    {
      try
      {
        string currentUrl = _webDriver.Url;
        const string highlightInGreen = "arguments[0].style.border='3px solid lime'";
        ((IJavaScriptExecutor)_webDriver).ExecuteScript(highlightInGreen, element);
        ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].click();", element);
        // Wait for a new page if navigation occurred
        if (_webDriver.Url != currentUrl)
          _waitHelper.WaitForPageToLoad();
      }
      catch (Exception e)
      {
        Logger.Error(e.Message + "\r\n" + e.StackTrace + "\r\n");
      }
    }

    public void EnterText(IWebElement element, string text)
    {
      ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].value ='" + text + "'", element);
    }

    public string GetElementValue(IWebElement element)
    {
      var elementValue = ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].value;", element);
      return elementValue?.ToString();
    }

    public void SetInnerHtml(IWebElement element, string text)
    {
      ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].innerHTML ='" + text + "'", element);
    }

    public string GetInnerHtml(IWebElement element)
    {
      var innerHtml = ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].innerHTML;", element);
      return innerHtml?.ToString();
    }

    public void ScrollToElement(IWebElement element)
    {
      try
      {
        ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
      }
      catch (Exception e)
      {
        Logger.Error(e.Message + "\r\n" + e.StackTrace + "\r\n");
      }
    }

    public void ScrollToAndClick(IWebElement element)
    {
      try
      {
        ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].click();", element);
        _waitHelper.WaitForPageToLoad();
      }
      catch (Exception e)
      {
        Logger.Error(e.Message + "\r\n" + e.StackTrace + "\r\n");
      }
    }

    public void HighlightElement(IWebElement element)
    {
      var highlight = TestSettings.HighlightElements;
      _waitHelper.WaitForElementToBeDisplayed(element);

      if (highlight != true) return; 
      const string scriptToExecute = HighlightInGreen;
      ((IJavaScriptExecutor)_webDriver).ExecuteScript(scriptToExecute, element);
    }

    public void HighlightElement(By by)
    {
      var highlight = TestSettings.HighlightElements;
      var element = _webDriver.FindElement(by);
      _waitHelper.WaitForElementToBeDisplayed(element);

      if (highlight != true) return;    
      const string scriptToExecute = HighlightInGreen;
      ((IJavaScriptExecutor)_webDriver).ExecuteScript(scriptToExecute, element);
    }
  }
}