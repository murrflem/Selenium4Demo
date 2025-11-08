using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using S4Demo.Framework.Setup;
using SeleniumExtras.WaitHelpers;
using System;

namespace S4Demo.Framework.Helpers
{
  public class WaitHelper
  {
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public WaitHelper(IWebDriver driver)
    {
      _driver = driver ?? throw new ArgumentNullException(nameof(driver));
      _wait = new WebDriverWait(
          new SystemClock(),
          _driver,
          TimeSpan.FromSeconds(TestSettings.DefaultTimeoutInSeconds),
          TimeSpan.FromMilliseconds(TestSettings.PollingIntervalMs)
      );
    }

    // ===================================================================
    // WAIT FOR ELEMENT ENABLED / CLICKABLE
    // ===================================================================

    public IWebElement WaitUntilClickable(By locator)
    {
      return _wait.Until(ExpectedConditions.ElementToBeClickable(locator));
    }

    public IWebElement WaitUntilClickable(IWebElement element)
    {
      return _wait.Until(driver =>
      {
        try
        {
          return (element.Displayed && element.Enabled) ? element : null;
        }
        catch (StaleElementReferenceException)
        {
          return null;
        }
        catch (NoSuchElementException)
        {
          return null;
        }
      });
    }

    public void WaitForElementEnabled(IWebElement element)
    {
      _wait.Until(d => element.Enabled);
    }

    public void WaitForElementEnabled(By by)
    {
      var element = _driver.FindElement(by);
      _wait.Until(d => element.Enabled);
    }

    // ===================================================================
    // WAIT FOR ELEMENT VISIBILE / DISPLAYED
    // ===================================================================

    public IWebElement WaitUntilVisible(By locator)
    {
      return _wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }

    public IWebElement WaitUntilVisible(IWebElement element)
    {
      return _wait.Until(driver =>
      {
        try
        {
          return element.Displayed ? element : null;
        }
        catch (StaleElementReferenceException)
        {
          return null;
        }
        catch (NoSuchElementException)
        {
          return null;
        }
      });
    }

    public void WaitForElementToBeDisplayed(IWebElement element)
    {
      _wait.Until(d => element.Displayed);
    }

    public void WaitForElementToBeDisplayed(By by)
    {
      var element = _driver.FindElement(by);
      _wait.Until(d => element.Displayed);
    }

    // ===================================================================
    // WAIT FOR ELEMENT INVISIBLE
    // ===================================================================

    public bool WaitUntilInvisible(By locator)
    {
      return _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
    }

    public bool WaitUntilInvisible(IWebElement element)
    {
      return _wait.Until(driver =>
      {
        try
        {
          return !element.Displayed;
        }
        catch (StaleElementReferenceException)
        {
          return true; // Consider it gone
        }
        catch (NoSuchElementException)
        {
          return true;
        }
      });
    }


    // ===================================================================
    // WAIT FOR TEXT PRESENCE
    // ===================================================================

    public bool WaitUntilTextPresent(By locator, string text)
    {
      return _wait.Until(ExpectedConditions.TextToBePresentInElementLocated(locator, text));
    }

    public bool WaitUntilTextPresent(IWebElement element, string text)
    {
      if (element == null)
        throw new ArgumentNullException(nameof(element));

      return _wait.Until(driver =>
      {
        try
        {
          return element.Text.Contains(text, StringComparison.OrdinalIgnoreCase);
        }
        catch (StaleElementReferenceException)
        {
          return false;
        }
        catch (NoSuchElementException)
        {
          return false;
        }
      });
    }

    // ===================================================================
    // WAIT FOR PAGE LOAD AND TITLE CONTAINS
    // ===================================================================

    public void WaitForPageToLoad()
    {
      _wait.Until(driver =>
      {
        try
        {
          return ((IJavaScriptExecutor)driver)
              .ExecuteScript("return document.readyState")
              .ToString()
              .Equals("complete", StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
          return false;
        }
      });
    }

    public void WaitForAjaxAndPageLoad()
    {
      _wait.Until(driver =>
      {
        try
        {
          var js = (IJavaScriptExecutor)driver;
          var ready = js.ExecuteScript("return document.readyState").ToString() == "complete";
          var ajaxComplete = (bool)(js.ExecuteScript("return window.jQuery != undefined && jQuery.active == 0") ?? true);
          return ready && ajaxComplete;
        }
        catch
        {
          return false;
        }
      });
    }

    public bool WaitUntilTitleContains(string text)
    {
      return _wait.Until(ExpectedConditions.TitleContains(text));
    }

    // ===================================================================
    // GENERIC CUSTOM WAITS
    // ===================================================================

    public bool WaitUntil(Func<IWebDriver, bool> condition)
    {
      return _wait.Until(condition);
    }
  }
}