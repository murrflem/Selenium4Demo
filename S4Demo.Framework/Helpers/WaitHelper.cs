using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using S4Demo.Framework.Setup;
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
          TimeSpan.FromSeconds(ConfigManager.DefaultTimeoutInSeconds),
          TimeSpan.FromMilliseconds(ConfigManager.PollingIntervalMs)
      );
    }

    // ===================================================================
    // WAIT FOR ELEMENT ENABLED / CLICKABLE
    // ===================================================================

    /// <summary>
    /// Waits until the element located by the specified <see cref="By"/> is both visible and enabled,
    /// making it ready for clicking or interaction. Returns the element once clickable.
    /// </summary>
    public IWebElement WaitUntilClickable(By locator)
    {
      return _wait.Until(driver =>
      {
        try
        {
          var element = driver.FindElement(locator);
          // Element must be both visible and enabled to be considered clickable
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

    /// <summary>
    /// Waits until the provided element is both visible and enabled.
    /// Returns the element when clickable, or null if the wait times out.
    /// </summary>
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

    /// <summary>
    /// Waits until the provided element reference is enabled for interaction.
    /// </summary>
    public void WaitForElementEnabled(IWebElement element)
    {
      _wait.Until(driver =>
      {
        try
        {
          return element != null && element.Enabled;
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

    /// <summary>
    /// Waits until the element located by the specified <see cref="By"/> is present and enabled for interaction.
    /// </summary>
    public void WaitForElementEnabled(By by)
    {
      _wait.Until(driver =>
      {
        try
        {
          var element = driver.FindElement(by);
          return element.Enabled;
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
    // WAIT FOR ELEMENT VISIBILITY / DISPLAYED
    // ===================================================================

    /// <summary>
    /// Waits until the element located by the specified <see cref="By"/> is visible on the page.
    /// Returns the element once visible.
    /// </summary>
    public IWebElement WaitUntilVisible(By locator)
    {
      return _wait.Until(driver =>
      {
        try
        {
          var element = driver.FindElement(locator);
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

    /// <summary>
    /// Waits until the provided element is visible on the page.
    /// Returns the element when displayed, or null if the wait times out.
    /// </summary>
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

    /// <summary>
    /// Waits until the provided element becomes invisible or is removed from the DOM.
    /// Returns true once the element is no longer displayed.
    /// </summary>
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

    /// <summary>
    /// Waits until the element located by the specified <see cref="By"/> is no longer visible in the DOM.
    /// Returns true once the element becomes invisible or is removed.
    /// </summary>
    public bool WaitUntilInvisible(By locator)
    {
      return _wait.Until(driver =>
      {
        try
        {
          var element = driver.FindElement(locator);
          return !element.Displayed;
        }
        catch (StaleElementReferenceException)
        {
          return true;
        }
        catch (NoSuchElementException)
        {
          return true;
        }
      });
    }

    /// <summary>
    /// Waits until the provided element reference becomes visible on the page.
    /// </summary>
    public void WaitForElementToBeDisplayed(IWebElement element)
    {
      _wait.Until(driver =>
      {
        try
        {
          return element != null && element.Displayed;
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

    /// <summary>
    /// Waits until the element located by the specified <see cref="By"/> is visible on the page.
    /// </summary>
    public void WaitForElementToBeDisplayed(By by)
    {
      _wait.Until(driver =>
      {
        try
        {
          var element = driver.FindElement(by);
          return element.Displayed;
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
    // WAIT FOR ELEMENT EXISTS
    // ===================================================================

    /// <summary>
    /// Waits until the element identified by the given locator exists in the DOM,
    /// regardless of whether it is visible or interactable.
    /// Returns the element once found, or null if the wait times out.
    /// </summary>
    public IWebElement WaitForElementExists(By locator)
    {
      return _wait.Until(driver =>
      {
        try
        {
          // Return element once it exists in the DOM
          return driver.FindElement(locator);
        }
        catch (NoSuchElementException)
        {
          return null;
        }
        catch (StaleElementReferenceException)
        {
          return null;
        }
      });
    }

    /// <summary>
    /// Waits until the provided element reference still exists in the DOM.
    /// Returns true if the element is valid and attached, false if it becomes stale or not found.
    /// </summary>
    public bool WaitForElementExists(IWebElement element)
    {
      return _wait.Until(driver =>
      {
        try
        {
          // If no exception is thrown, element still exists in DOM
          var _ = element.Displayed;
          return true;
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
    // WAIT FOR TEXT PRESENCE
    // ===================================================================

    /// <summary>
    /// Waits until the element located by the specified <see cref="By"/> contains the specified text.
    /// Returns true if the text is found within the element before timeout.
    /// </summary>
    public bool WaitUntilTextPresent(By locator, string text)
    {
      return _wait.Until(driver =>
      {
        try
        {
          var element = driver.FindElement(locator);
          return element.Text.Contains(text, StringComparison.OrdinalIgnoreCase);
        }
        catch (NoSuchElementException)
        {
          return false;
        }
        catch (StaleElementReferenceException)
        {
          return false;
        }
      });
    }

    /// <summary>
    /// Waits until the provided element contains the specified text.
    /// Returns true if the text is found within the element before timeout.
    /// </summary>
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
    // WAIT FOR TITLE/URL CONTAINS AND PAGE LOAD
    // ===================================================================

    /// <summary>
    /// Waits until the browser title contains the specified text.
    /// Returns true once the condition is met.
    /// </summary>
    public bool WaitUntilTitleContains(string text)
    {
      return _wait.Until(driver =>
      {
        try
        {
          return driver.Title.Contains(text, StringComparison.OrdinalIgnoreCase);
        }
        catch (WebDriverException)
        {
          return false;
        }
      });
    }

    /// <summary>
    /// Waits until the browser URL contains the specified partial URL.
    /// Returns true once the condition is met.
    /// </summary>
    public bool WaitForUrlContains(string partialUrl)
    {
      return _wait.Until(driver =>
      {
        try
        {
          return driver.Url.Contains(partialUrl, StringComparison.OrdinalIgnoreCase);
        }
        catch (WebDriverException)
        {
          return false;
        }
      });
    }

    /// <summary>
    /// Waits for the browser to fully load the page, determined by the document.readyState property reaching "complete".
    /// </summary>
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

    /// <summary>
    /// Waits until both the page load is complete and all jQuery AJAX requests (if jQuery is present) are finished.
    /// </summary>
    public void WaitForAjaxAndPageLoad()
    {
      _wait.Until(driver =>
      {
        try
        {
          var js = (IJavaScriptExecutor)driver;
          var ready = js.ExecuteScript("return document.readyState").ToString() == "complete";
          var ajaxComplete = (bool)(js.ExecuteScript("return (window.jQuery && jQuery.active == 0) || !window.jQuery;") ?? true);
          return ready && ajaxComplete;
        }
        catch
        {
          return false;
        }
      });
    }

    // ===================================================================
    // GENERIC CUSTOM WAITS
    // ===================================================================

    /// <summary>
    /// Waits for a custom condition to be true. Useful for project-specific or dynamic conditions
    /// that are not covered by built-in wait helpers.
    /// </summary>
    public bool WaitUntil(Func<IWebDriver, bool> condition)
    {
      return _wait.Until(condition);
    }
  }
}