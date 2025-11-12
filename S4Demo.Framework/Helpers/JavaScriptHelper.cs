using OpenQA.Selenium;
using S4Demo.Framework.Setup;
using System;

namespace S4Demo.Framework.Helpers
{
  /// <summary>
  /// Utilities that execute JavaScript in the browser via <see cref="IJavaScriptExecutor"/>.
  /// Centralizes JavaScript interactions (click, scroll, value manipulation, page utilities).
  /// </summary>
  public class JavaScriptHelper
  {
    private readonly IWebDriver _webDriver;
    private readonly IJavaScriptExecutor _js;
    private readonly WaitHelper _waitHelper;
    protected readonly SeleniumHelper Selenium;

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

        FrameworkLogger.LogInfo("Clicked element using JavaScript.");
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError("Error clicking element via JavaScript.", ex);
      }
    }

    public void ScrollToAndClick(IWebElement element)
    {
      try
      {
        _waitHelper.WaitUntilVisible(element);
        _js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        ExecuteHighlight(element);
        _js.ExecuteScript("arguments[0].click();", element);
        _waitHelper.WaitForPageToLoad();

        FrameworkLogger.LogInfo("Scrolled to and clicked element via JavaScript.");
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError("Error during ScrollToAndClick.", ex);
      }
    }

    // ===================================================================
    // TEXT ENTRY / RETRIEVAL
    // ===================================================================

    public void EnterText(IWebElement element, string text)
    {
      try
      {
        _waitHelper.WaitUntilVisible(element);
        ExecuteHighlight(element);
        _js.ExecuteScript("arguments[0].value = arguments[1];", element, text ?? string.Empty);
        FrameworkLogger.LogInfo($"Entered text '{text}' using JavaScript.");
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError("Error entering text via JavaScript.", ex);
      }
    }

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
        FrameworkLogger.LogError("Error retrieving element value via JavaScript.", ex);
        return string.Empty;
      }
    }

    public void SetInnerHtml(IWebElement element, string text)
    {
      try
      {
        _waitHelper.WaitUntilVisible(element);
        _js.ExecuteScript("arguments[0].innerHTML = arguments[1];", element, text ?? string.Empty);
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError("Error setting innerHTML via JavaScript.", ex);
      }
    }

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
        FrameworkLogger.LogError("Error retrieving innerHTML via JavaScript.", ex);
        return string.Empty;
      }
    }

    // ===================================================================
    // PAGE-LEVEL UTILITIES
    // ===================================================================

    public string GetPageTitleViaJS()
    {
      try
      {
        return _js.ExecuteScript("return document.title;")?.ToString() ?? string.Empty;
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError("Error retrieving page title via JavaScript.", ex);
        return string.Empty;
      }
    }

    public string GetPageInnerText()
    {
      try
      {
        return _js.ExecuteScript("return document.documentElement.innerText;")?.ToString() ?? string.Empty;
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError("Error retrieving page inner text via JavaScript.", ex);
        return string.Empty;
      }
    }

    public void RefreshPageViaJS()
    {
      try
      {
        _js.ExecuteScript("location.reload();");
        _waitHelper.WaitForPageToLoad();
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError("Error refreshing page via JavaScript.", ex);
      }
    }

    public object ExecuteAsyncScript(string script, params object[] args)
    {
      try
      {
        return _js.ExecuteAsyncScript(script, args);
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError("Error executing async JavaScript.", ex);
        return null;
      }
    }

    // ===================================================================
    // SCROLL AND VISUAL UTILITIES
    // ===================================================================

    public void ScrollToElement(IWebElement element)
    {
      try
      {
        _waitHelper.WaitUntilVisible(element);
        _js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError("Error scrolling to element via JavaScript.", ex);
      }
    }

    public void ScrollToTop()
    {
      try
      {
        _js.ExecuteScript("window.scrollTo(0, 0);");
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError("Error scrolling to top via JavaScript.", ex);
      }
    }

    public void ScrollToBottom()
    {
      try
      {
        _js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError("Error scrolling to bottom via JavaScript.", ex);
      }
    }

    public void ScrollToAndClickElement(IWebElement element)
    {
      ScrollToAndClick(element);
    }

    // ===================================================================
    // HIGHLIGHTING
    // ===================================================================

    public void HighlightElement(IWebElement element)
    {
      try
      {
        _waitHelper.WaitForElementToBeDisplayed(element);
        ExecuteHighlight(element);
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogWarning($"Error highlighting element (IWebElement): {ex.Message}");
      }
    }

    public void HighlightElement(By by)
    {
      try
      {
        var element = _waitHelper.WaitUntilVisible(by);
        ExecuteHighlight(element);
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogWarning($"Error highlighting element (By locator): {ex.Message}");
      }
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
        FrameworkLogger.LogWarning($"Error executing highlight JavaScript: {ex.Message}");
      }
    }
  }
}