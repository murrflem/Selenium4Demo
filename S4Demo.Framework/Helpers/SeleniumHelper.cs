using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace S4Demo.Framework.Helpers
{
  public class SeleniumHelper
  {
    private readonly IWebDriver _webDriver;
    private readonly WaitHelper _waitHelper;

    public SeleniumHelper(IWebDriver webDriver)
    {
      _webDriver = webDriver ?? throw new ArgumentNullException(nameof(webDriver));
      _waitHelper = new WaitHelper(webDriver);
    }

    #region click/submit

    /// <summary>
    /// Clicks the specified <see cref="IWebElement"/> safely, 
    /// ensuring it is clickable, highlighted, and waiting for page load or navigation.
    /// </summary>
    /// <param name="element">The target web element to click.</param>
    public void Click(IWebElement element) => PerformElementAction(element, e => e.Click(), "Click");

    /// <summary>
    /// Clicks the element identified by the specified <see cref="By"/> locator, 
    /// ensuring it is clickable, highlighted, and waiting for page load or navigation.
    /// </summary>
    /// <param name="by">Locator used to find the element to click.</param>
    public void Click(By by)
    {
      var element = _waitHelper.WaitUntilClickable(by);
      PerformElementAction(element, e => e.Click(), "Click");
    }

    /// <summary>
    /// Submits the specified <see cref="IWebElement"/> safely, 
    /// ensuring it is ready, highlighted, and waiting for page load or navigation.
    /// </summary>
    /// <param name="element">The target web element to submit.</param>
    public void Submit(IWebElement element) => PerformElementAction(element, e => e.Submit(), "Submit");

    /// <summary>
    /// Submits the element identified by the specified <see cref="By"/> locator, 
    /// ensuring it is ready, highlighted, and waiting for page load or navigation.
    /// </summary>
    /// <param name="by">Locator used to find the element to submit.</param>
    public void Submit(By by)
    {
      var element = _waitHelper.WaitUntilClickable(by);
      PerformElementAction(element, e => e.Submit(), "Submit");
    }

    /// <summary>
    /// Performs a safe element action such as Click or Submit with built-in wait, highlight, and error handling.
    /// Automatically waits for AJAX and page load after the action completes.
    /// </summary>
    /// <param name="element">The target web element to interact with.</param>
    /// <param name="action">The action delegate (e.g., e => e.Click()).</param>
    /// <param name="actionName">A descriptive name for logging, e.g., "Click" or "Submit".</param>
    private void PerformElementAction(IWebElement element, Action<IWebElement> action, string actionName)
    {
      if (element == null)
      {
        FrameworkLogger.LogWarning($"{actionName} skipped — target element was null.");
        return;
      }

      try
      {
        _waitHelper.WaitUntilClickable(element);
        new JavaScriptHelper(_webDriver).HighlightElement(element);

        string currentUrl = _webDriver.Url;
        action(element);

        // Wait for potential AJAX or navigation
        _waitHelper.WaitForAjaxAndPageLoad();

        if (_webDriver.Url != currentUrl)
          _waitHelper.WaitForAjaxAndPageLoad();

        FrameworkLogger.LogInfo($"{actionName} action performed successfully.");
      }
      catch (StaleElementReferenceException ex)
      {
        FrameworkLogger.LogWarning($"{actionName} failed — element became stale. {ex.Message}");
      }
      catch (NoSuchElementException ex)
      {
        FrameworkLogger.LogWarning($"{actionName} failed — element not found. {ex.Message}");
      }
      catch (WebDriverException ex)
      {
        FrameworkLogger.LogError($"{actionName} failed due to WebDriver issue.", ex);
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError($"Unexpected error during {actionName} action.", ex); 
      }
    }

    #endregion

    #region enter/clear/get text

    /// <summary>
    /// Safely enters text into an input element, clearing it first and waiting for it to be visible.
    /// </summary>
    /// <param name="element">The input element where text will be entered.</param>
    /// <param name="textToType">The text to type into the field.</param>
    public void EnterText(IWebElement element, string textToType)
    {
      if (element == null)
      {
        FrameworkLogger.LogWarning("EnterText skipped — target element was null.");
        return;
      }

      try
      {
        _waitHelper.WaitUntilVisible(element);
        new JavaScriptHelper(_webDriver).HighlightElement(element);

        ClearTextbox(element);
        element.SendKeys(textToType ?? string.Empty);

        FrameworkLogger.LogInfo($"Text '{textToType}' entered successfully.");
      }
      catch (StaleElementReferenceException ex)
      {
        FrameworkLogger.LogWarning($"Element became stale during EnterText. {ex.Message}");
      }
      catch (NoSuchElementException ex)
      {
        FrameworkLogger.LogWarning($"Element not found during EnterText. {ex.Message}");
      }
      catch (WebDriverException ex)
      {
        FrameworkLogger.LogError("WebDriver error while entering text.", ex);
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError("Unexpected error while entering text.", ex);
      }
    }

    /// <summary>
    /// Clears an input element safely, waiting until it is visible and ready before clearing.
    /// </summary>
    /// <param name="textBox">The textbox or input field to clear.</param>
    public void ClearTextbox(IWebElement textBox)
    {
      if (textBox == null)
      {
        FrameworkLogger.LogWarning("ClearTextbox skipped — target element was null.");
        return;
      }

      try
      {
        _waitHelper.WaitUntilVisible(textBox);
        new JavaScriptHelper(_webDriver).HighlightElement(textBox);
        textBox.Clear();
        FrameworkLogger.LogInfo("Textbox cleared successfully.");
      }
      catch (StaleElementReferenceException ex)
      {
        FrameworkLogger.LogWarning($"Textbox became stale during ClearTextbox. {ex.Message}");
      }
      catch (NoSuchElementException ex)
      {
        FrameworkLogger.LogWarning($"Textbox not found during ClearTextbox. {ex.Message}");
      }
      catch (WebDriverException ex)
      {
        FrameworkLogger.LogError("WebDriver error while clearing textbox.", ex);
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError("Unexpected error while clearing textbox.", ex);
      }
    }

    public string GetTextFromElement(IWebElement element)
    {
      return SafeGetText(() => element);
    }

    public string GetTextFromElement(By by)
    {
      return SafeGetText(() => _waitHelper.WaitUntilVisible(by));
    }

    // Internal shared implementation
    private string SafeGetText(Func<IWebElement> getElement)
    {
      try
      {
        var element = getElement();

        if (element == null)
          return string.Empty;

        _waitHelper.WaitUntilVisible(element);
        new JavaScriptHelper(_webDriver).HighlightElement(element);

        var text = element.Text?.Trim();
        return string.IsNullOrEmpty(text) ? string.Empty : text;
      }
      catch (StaleElementReferenceException)
      {
        FrameworkLogger.LogWarning("Element became stale while trying to retrieve text.");
        return string.Empty;
      }
      catch (NoSuchElementException)
      {
        FrameworkLogger.LogWarning("Element not found while trying to retrieve text.");
        return string.Empty;
      }
      catch (Exception e)
      {
        FrameworkLogger.LogError("Error while checking element state.", e);
        return string.Empty;
      }
    }

    #endregion

    #region get elements / links/ attributes
    public string GetLinkHref(IWebElement element)
    {
      var href = element.GetAttribute("href");
      return href;
    }

    public string GetElementValueAttribute(IWebElement element)
    {
      var value = element.GetAttribute("value");
      return value;
    }

    public string GetElementId(IWebElement element)
    {
      var id = element.GetAttribute("id");
      return id;
    }

    public IEnumerable<IWebElement> GetAllPageLinks()
    {
      var list = _webDriver.FindElements(By.TagName("a"));
      return list;
    }

    public IEnumerable<IWebElement> GetAllPageImages()
    {
      var list = _webDriver.FindElements(By.TagName("img"));
      return list;
    }

    #endregion

    #region select elements / drop-downs

    /// <summary>
    /// Selects an option in a <see cref="SelectElement"/> by its visible text.
    /// </summary>
    /// <remarks>
    /// This method waits for the dropdown to be visible and interactable, highlights it for debugging,
    /// and then selects the matching option text.
    /// </remarks>
    /// <param name="element">The dropdown <see cref="IWebElement"/> element.</param>
    /// <param name="comboItem">The visible text of the option to select.</param>
    /// <exception cref="NoSuchElementException">Thrown if the specified option is not found.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="element"/> is null.</exception>
    public void SelectItemByText(IWebElement element, string comboItem)
    {
      try
      {
        var selectElement = GetReadySelectElement(element);
        selectElement.SelectByText(comboItem);
      }
      catch (NoSuchElementException)
      {
        FrameworkLogger.LogWarning($"Option with text '{comboItem}' not found in dropdown.");
      }
      catch (StaleElementReferenceException)
      {
        FrameworkLogger.LogWarning("Dropdown element became stale while selecting by text.");
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError($"Unexpected error selecting dropdown option by text: '{comboItem}'.", ex);
      }
    }

    /// <summary>
    /// Selects an option in a <see cref="SelectElement"/> by its <c>value</c> attribute.
    /// </summary>
    /// <remarks>
    /// This method waits until the dropdown is visible, highlights it in the browser,
    /// and selects the option whose value matches the provided string.
    /// </remarks>
    /// <param name="element">The dropdown <see cref="IWebElement"/> element.</param>
    /// <param name="comboItem">The <c>value</c> attribute of the option to select.</param>
    /// <exception cref="NoSuchElementException">Thrown if the specified value is not found in the dropdown.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="element"/> is null.</exception>
    public void SelectItemByValue(IWebElement element, string comboItem)
    {
      try
      {
        var selectElement = GetReadySelectElement(element);
        selectElement.SelectByValue(comboItem);
      }
      catch (NoSuchElementException)
      {
        FrameworkLogger.LogWarning($"Option with value '{comboItem}' not found in dropdown.");
      }
      catch (StaleElementReferenceException)
      {
        FrameworkLogger.LogWarning("Dropdown element became stale while selecting by value.");
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError($"Unexpected error selecting dropdown option by value: '{comboItem}'.", ex);
      }
    }

    /// <summary>
    /// Retrieves the total number of options available in a <see cref="SelectElement"/>.
    /// </summary>
    /// <remarks>
    /// Waits until the dropdown is visible before counting, and handles common Selenium exceptions gracefully.
    /// </remarks>
    /// <param name="element">The dropdown <see cref="IWebElement"/> to inspect.</param>
    /// <returns>The number of available options, or <c>0</c> if the element is missing or stale.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="element"/> is null.</exception>
    public int GetNumberOfOptionsInSelect(IWebElement element)
    {
      try
      {
        var selectElement = GetReadySelectElement(element);
        return selectElement.Options?.Count ?? 0;
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError("Error retrieving number of dropdown options.", ex);
        return 0;
      }
    }

    /// <summary>
    /// Retrieves the visible text of the currently selected option in a <see cref="SelectElement"/>.
    /// </summary>
    /// <remarks>
    /// Waits until the dropdown is visible and returns the active option text.  
    /// Returns an empty string if the dropdown or selected option cannot be accessed.
    /// </remarks>
    /// <param name="element">The dropdown <see cref="IWebElement"/> element.</param>
    /// <returns>The visible text of the selected option, or an empty string if unavailable.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="element"/> is null.</exception>
    public string GetCurrentValueInSelect(IWebElement element)
    {
      try
      {
        var selectElement = GetReadySelectElement(element);
        var selectedOption = selectElement.SelectedOption;
        return selectedOption?.Text ?? string.Empty;
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError("Error retrieving currently selected dropdown value.", ex);
        return string.Empty;
      }
    }

    /// <summary>
    /// Creates and returns a ready-to-use <see cref="SelectElement"/> from the given <see cref="IWebElement"/>.
    /// </summary>
    /// <remarks>
    /// This helper method ensures that the dropdown element is:
    /// <list type="bullet">
    ///   <item><description>Non-null (throws <see cref="ArgumentNullException"/> if null)</description></item>
    ///   <item><description>Visible and interactable on the page</description></item>
    ///   <item><description>Highlighted in the browser for debugging visibility</description></item>
    ///   <item><description>Populated with options (waits until at least one option exists)</description></item>
    /// </list>
    /// 
    /// It improves stability and reduces duplication in dropdown-related operations like selecting values,
    /// counting options, or retrieving the currently selected item.
    /// </remarks>
    /// <param name="element">The dropdown <see cref="IWebElement"/> to wrap in a <see cref="SelectElement"/>.</param>
    /// <returns>A ready <see cref="SelectElement"/> that can be safely used for selection or data retrieval.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="element"/> is null.</exception>
    private SelectElement GetReadySelectElement(IWebElement element)
    {
      if (element == null)
        throw new ArgumentNullException(nameof(element));

      _waitHelper.WaitUntilVisible(element);
      new JavaScriptHelper(_webDriver).HighlightElement(element);

      var selectElement = new SelectElement(element);

      // Ensure options are loaded (useful for dynamic dropdowns)
      _waitHelper.WaitUntil(driver =>
      {
        try
        {
          return selectElement.Options.Count > 0;
        }
        catch (StaleElementReferenceException)
        {
          return false;
        }
      });

      return selectElement;
    }

    #endregion

    #region verifications

    // Text Present Verification ----------------------------------------------
    public bool IsTextPresentInElement(IWebElement element, string expectedText)
    {
      return SafeIsTextPresent(() => element, expectedText);
    }

    public bool IsTextPresentInElement(By by, string expectedText)
    {
      return SafeIsTextPresent(() => _waitHelper.WaitUntilVisible(by), expectedText);
    }

    // Internal shared implementation
    private bool SafeIsTextPresent(Func<IWebElement> getElement, string expectedText)
    {
      try
      {
        var element = getElement();

        if (element == null || string.IsNullOrWhiteSpace(expectedText))
          return false;

        // Wait until text appears
        _waitHelper.WaitUntilTextPresent(element, expectedText);

        // Check text presence with trimming and case-insensitive match
        var elementText = element.Text?.Trim() ?? string.Empty;
        return elementText.Contains(expectedText, StringComparison.OrdinalIgnoreCase);
      }
      catch (StaleElementReferenceException)
      {
        FrameworkLogger.LogWarning("Element became stale while checking for text presence.");
        return false;
      }
      catch (NoSuchElementException)
      {
        FrameworkLogger.LogWarning("Element not found while checking for text presence.");
        return false;
      }
      catch (WebDriverTimeoutException)
      {
        FrameworkLogger.LogWarning($"Timeout waiting for text '{expectedText}' to appear.");
        return false;
      }
      catch (Exception e)
      {
        FrameworkLogger.LogError("Error while checking element state.", e);
        return false;
      }
    }

    public bool IsTextPresentOnPage(string expectedText)
    {
      return _webDriver.PageSource.Contains(expectedText);
    }

    // Element Visible Verification --------------------------------------------
    public bool IsElementVisible(IWebElement element)
    {
      return SafeIsElementVisible(() => element);
    }

    public bool IsElementVisible(By by)
    {
      return SafeIsElementVisible(() => _waitHelper.WaitUntilVisible(by));
    }

    // Internal shared implementation
    private bool SafeIsElementVisible(Func<IWebElement> getElement)
    {
      try
      {
        var element = getElement();

        if (element == null)
          return false;

        _waitHelper.WaitUntilVisible(element);
        new JavaScriptHelper(_webDriver).HighlightElement(element);

        return element.Displayed;
      }
      catch (StaleElementReferenceException)
      {
        FrameworkLogger.LogWarning("Element became stale while checking visibility.");
        return false;
      }
      catch (NoSuchElementException)
      {
        FrameworkLogger.LogWarning("Element not found while checking visibility.");
        return false;
      }
      catch (WebDriverTimeoutException)
      {
        FrameworkLogger.LogWarning("Timed out waiting for element to become visible.");
        return false;
      }
      catch (Exception e)
      {
        FrameworkLogger.LogError("Error while checking element state.", e);
        return false;
      }
    }

    // Element Enabled Verification --------------------------------------------
    public bool IsElementEnabled(IWebElement element)
    {
      return SafeIsElementEnabled(() => element);
    }

    public bool IsElementEnabled(By by)
    {
      return SafeIsElementEnabled(() => _waitHelper.WaitForElementExists(by));
    }

    // Shared internal implementation
    private bool SafeIsElementEnabled(Func<IWebElement> getElement)
    {
      try
      {
        var element = getElement();

        if (element == null)
          return false;

        _waitHelper.WaitForElementEnabled(element);
        return element.Enabled;
      }
      catch (StaleElementReferenceException)
      {
        FrameworkLogger.LogWarning("Element became stale while checking if enabled.");
        return false;
      }
      catch (NoSuchElementException)
      {
        FrameworkLogger.LogWarning("Element not found while checking if enabled.");
        return false;
      }
      catch (WebDriverTimeoutException)
      {
        FrameworkLogger.LogWarning("Timed out waiting for element to become enabled.");
        return false;
      }
      catch (Exception e)
      {
        FrameworkLogger.LogError("Error while checking element state.", e);
        return false;
      }
    }

    // Element Selected Verification -------------------------------------------
    public bool IsElementSelected(IWebElement element)
    {
      return SafeIsElementSelected(() => element);
    }

    public bool IsElementSelected(By by)
    {
      return SafeIsElementSelected(() => _waitHelper.WaitForElementExists(by));
    }

    // Shared internal implementation
    private bool SafeIsElementSelected(Func<IWebElement> getElement)
    {
      try
      {
        var element = getElement();

        if (element == null)
          return false;

        _waitHelper.WaitUntilVisible(element);
        return element.Selected;
      }
      catch (StaleElementReferenceException)
      {
        FrameworkLogger.LogWarning("Element became stale while checking if selected.");
        return false;
      }
      catch (NoSuchElementException)
      {
        FrameworkLogger.LogWarning("Element not found while checking if selected.");
        return false;
      }
      catch (WebDriverTimeoutException)
      {
        FrameworkLogger.LogWarning("Timed out waiting for element to be available for selection check.");
        return false;
      }
      catch (Exception e)
      {
        FrameworkLogger.LogError("Error while checking element state.", e);
        return false;
      }
    }

    // Elements visible on Page Verification ------------------------------------
    // Verifies that all expected elements are visible on the current page.
    // Waits for the page to load, checks each element’s visibility
    // logs warnings or errors if any are missing or not displayed.
    public bool IsPageDisplayingExpectedElements(IReadOnlyList<IWebElement> expectedElements)
    {
      if (expectedElements == null || expectedElements.Count == 0)
      {
        FrameworkLogger.LogWarning("No expected elements were provided for page validation.");
        return false;
      }

      _waitHelper.WaitForPageToLoad();

      try
      {
        // All elements must be visible
        foreach (var element in expectedElements)
        {
          if (!IsElementVisible(element))
          {
            FrameworkLogger.LogWarning("One or more expected elements were not visible on the page.");
            return false;
          }
        }

        return true;
      }
      catch (Exception e)
      {
        FrameworkLogger.LogError($"Error while validating expected elements: {e.Message}\r\n{e.StackTrace}");
        return false;
      }
    }

    #endregion

    #region key presses

    public void PressEnterKey(IWebElement element)
    {
      element.SendKeys(Keys.Enter);
    }

    public void PressEnterKey(By by)
    {
      var element = _waitHelper.WaitUntilVisible(by);
      element.SendKeys(Keys.Enter);
    }

    public void PressTabKey(IWebElement element)
    {
      element.SendKeys(Keys.Tab);
    }

    public void PressTabKey(By by)
    {
      var element = _waitHelper.WaitUntilVisible(by);
      element.SendKeys(Keys.Tab);
    }

    public void PressBackspaceKey(IWebElement element)
    {
      element.SendKeys(Keys.Backspace);
    }

    public void PressBackspaceKey(By by)
    {
      var element = _waitHelper.WaitUntilVisible(by);
      element.SendKeys(Keys.Backspace);
    }

    #endregion
  }
}