using NLog;
using OpenQA.Selenium;
using S4Demo.Framework.Setup;
using System;

namespace S4Demo.Framework.Helpers
{
  /// <summary>
  /// Utilities that execute JavaScript in the browser via <see cref="IJavaScriptExecutor"/>.
  /// This class centralizes common JS interactions (click, scroll, read/write values, page utilities).
  /// All calls use a cached IJavaScriptExecutor instance (_js) for consistency.
  /// </summary>
  public class JavaScriptHelper
  {
    private readonly IWebDriver _webDriver;
    private readonly IJavaScriptExecutor _js;
    private readonly WaitHelper _waitHelper;
    protected readonly SeleniumHelper Selenium;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private const string HighlightInGreen = "arguments[0].style.border='3px solid lime'";

    public JavaScriptHelper(IWebDriver webDriver)
    {
      _webDriver = webDriver ?? throw new ArgumentNullException(nameof(webDriver));
      _js = (IJavaScriptExecutor)webDriver;
      _waitHelper = new WaitHelper(webDriver);
      Selenium = new SeleniumHelper(webDriver);
    }

    // ===================================================================
    // CLICK / INTERACTION
    // ===================================================================

    /// <summary>
    /// Clicks the specified element using JavaScript and highlights it before clicking.
    /// Waits for navigation to complete if the click triggers a new page.
    /// </summary>
    /// <param name="element">The web element to click.</param>
    public void ClickElement(IWebElement element)
    {
      try
      {
        _waitHelper.WaitUntilClickable(element);
        ExecuteHighlight(element);

        string currentUrl = _webDriver.Url;
        _js.ExecuteScript("arguments[0].click();", element);

        if (_webDriver.Url != currentUrl)
          _waitHelper.WaitForPageToLoad();
      }
      catch (Exception ex)
      {
        LogError(nameof(ClickElement), ex);
      }
    }

    /// <summary>
    /// Scrolls to an element and clicks it using JavaScript.
    /// Waits for page load after click.
    /// </summary>
    /// <param name="element">The web element to scroll to and click.</param>
    public void ScrollToAndClick(IWebElement element)
    {
      try
      {
        _waitHelper.WaitUntilVisible(element);
        _js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        ExecuteHighlight(element);
        _js.ExecuteScript("arguments[0].click();", element);
        _waitHelper.WaitForPageToLoad();
      }
      catch (Exception ex)
      {
        LogError(nameof(ScrollToAndClick), ex);
      }
    }

    // ===================================================================
    // TEXT ENTRY / RETRIEVAL
    // ===================================================================

    /// <summary>
    /// Enters text into the specified element using JavaScript by setting its 'value' property.
    /// </summary>
    /// <param name="element">The target web element.</param>
    /// <param name="text">The text to enter.</param>
    public void EnterText(IWebElement element, string text)
    {
      try
      {
        _waitHelper.WaitUntilVisible(element);
        ExecuteHighlight(element);
        _js.ExecuteScript("arguments[0].value = arguments[1];", element, text ?? string.Empty);
      }
      catch (Exception ex)
      {
        LogError(nameof(EnterText), ex);
      }
    }

    /// <summary>
    /// Retrieves the current 'value' property of the specified web element using JavaScript.
    /// </summary>
    /// <param name="element">The target web element.</param>
    /// <returns>The element's value as a string, or an empty string if not available.</returns>
    public string GetElementValue(IWebElement element)
    {
      try
      {
        _waitHelper.WaitUntilVisible(element);
        var elementValue = _js.ExecuteScript("return arguments[0].value;", element);
        return elementValue?.ToString() ?? string.Empty;
      }
      catch (Exception ex)
      {
        LogError(nameof(GetElementValue), ex);
        return string.Empty;
      }
    }

    /// <summary>
    /// Sets the innerHTML of the specified element using JavaScript.
    /// </summary>
    /// <param name="element">The target web element.</param>
    /// <param name="text">The HTML/text to set.</param>
    public void SetInnerHtml(IWebElement element, string text)
    {
      try
      {
        _waitHelper.WaitUntilVisible(element);
        _js.ExecuteScript("arguments[0].innerHTML = arguments[1];", element, text ?? string.Empty);
      }
      catch (Exception ex)
      {
        LogError(nameof(SetInnerHtml), ex);
      }
    }

    /// <summary>
    /// Retrieves the innerHTML of the specified element using JavaScript.
    /// </summary>
    /// <param name="element">The target web element.</param>
    /// <returns>The element's innerHTML as a string, or an empty string if not available.</returns>
    public string GetInnerHtml(IWebElement element)
    {
      try
      {
        _waitHelper.WaitUntilVisible(element);
        var innerHtml = _js.ExecuteScript("return arguments[0].innerHTML;", element);
        return innerHtml?.ToString() ?? string.Empty;
      }
      catch (Exception ex)
      {
        LogError(nameof(GetInnerHtml), ex);
        return string.Empty;
      }
    }

    // ===================================================================
    // PAGE-LEVEL UTILITIES
    // ===================================================================

    /// <summary>
    /// Retrieves the document title using JavaScript.
    /// </summary>
    public string GetPageTitleViaJS()
    {
      try
      {
        return _js.ExecuteScript("return document.title;")?.ToString() ?? string.Empty;
      }
      catch (Exception ex)
      {
        LogError(nameof(GetPageTitleViaJS), ex);
        return string.Empty;
      }
    }

    /// <summary>
    /// Retrieves the visible inner text of the page using JavaScript.
    /// </summary>
    public string GetPageInnerText()
    {
      try
      {
        return _js.ExecuteScript("return document.documentElement.innerText;")?.ToString() ?? string.Empty;
      }
      catch (Exception ex)
      {
        LogError(nameof(GetPageInnerText), ex);
        return string.Empty;
      }
    }

    /// <summary>
    /// Refreshes the page using JavaScript and waits for it to load.
    /// </summary>
    public void RefreshPageViaJS()
    {
      try
      {
        _js.ExecuteScript("location.reload();");
        _waitHelper.WaitForPageToLoad();
      }
      catch (Exception ex)
      {
        LogError(nameof(RefreshPageViaJS), ex);
      }
    }

    /// <summary>
    /// Executes an asynchronous JavaScript script and returns the result.
    /// The provided script is expected to call the callback when complete.
    /// </summary>
    /// <param name="script">JavaScript async script that takes a callback as the last argument.</param>
    /// <param name="args">Optional arguments passed to the script.</param>
    /// <returns>Result of the executed script, or null on error.</returns>
    public object ExecuteAsyncScript(string script, params object[] args)
    {
      try
      {
        return _js.ExecuteAsyncScript(script, args);
      }
      catch (Exception ex)
      {
        LogError(nameof(ExecuteAsyncScript), ex);
        return null;
      }
    }

    // ===================================================================
    // SCROLL AND VISUAL UTILITIES
    // ===================================================================

    /// <summary>
    /// Scrolls the page until the specified element is in view using JavaScript.
    /// </summary>
    /// <param name="element">The target web element to scroll into view.</param>
    public void ScrollToElement(IWebElement element)
    {
      try
      {
        _waitHelper.WaitUntilVisible(element);
        _js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
      }
      catch (Exception ex)
      {
        LogError(nameof(ScrollToElement), ex);
      }
    }

    /// <summary>
    /// Scrolls to the top of the document.
    /// </summary>
    public void ScrollToTop()
    {
      try
      {
        _js.ExecuteScript("window.scrollTo(0, 0);");
      }
      catch (Exception ex)
      {
        LogError(nameof(ScrollToTop), ex);
      }
    }

    /// <summary>
    /// Scrolls to the bottom of the document.
    /// </summary>
    public void ScrollToBottom()
    {
      try
      {
        _js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
      }
      catch (Exception ex)
      {
        LogError(nameof(ScrollToBottom), ex);
      }
    }

    /// <summary>
    /// Scrolls to the element and clicks it using JavaScript (alias for ScrollToAndClick).
    /// </summary>
    /// <param name="element">The target web element.</param>
    public void ScrollToAndClickElement(IWebElement element)
    {
      ScrollToAndClick(element);
    }

    // ===================================================================
    // HIGHLIGHTING
    // ===================================================================

    /// <summary>
    /// Highlights the specified element by applying a green border using JavaScript.
    /// The action only runs if element highlighting is enabled in TestSettings.
    /// </summary>
    /// <param name="element">The target web element to highlight.</param>
    public void HighlightElement(IWebElement element)
    {
      try
      {
        _waitHelper.WaitForElementToBeDisplayed(element);
        ExecuteHighlight(element);
      }
      catch (Exception ex)
      {
        Logger.Warn(ex, "Error highlighting element (IWebElement).");
      }
    }

    /// <summary>
    /// Highlights an element located by the specified selector using JavaScript.
    /// The action only runs if element highlighting is enabled in TestSettings.
    /// </summary>
    /// <param name="by">The locator used to find the element.</param>
    public void HighlightElement(By by)
    {
      try
      {
        var element = _waitHelper.WaitUntilVisible(by);
        ExecuteHighlight(element);
      }
      catch (Exception ex)
      {
        Logger.Warn(ex, "Error highlighting element (By locator).");
      }
    }

    // ===================================================================
    // CENTRALIZED ERROR HANDLING
    // ===================================================================
    private void LogError(string action, Exception ex)
    {
      Logger.Error(ex, $"JavaScriptHelper: Error during {action} - {ex.Message}");
    }

    // ===================================================================
    // PRIVATE UTILITIES
    // ===================================================================
    private void ExecuteHighlight(IWebElement element)
    {
      if (!ConfigManager.HighlightElements || element == null) return;

      try
      {
        _js.ExecuteScript(HighlightInGreen, element);
      }
      catch (Exception ex)
      {
        Logger.Warn(ex, "Error executing element highlight.");
      }
    }

  }
}
