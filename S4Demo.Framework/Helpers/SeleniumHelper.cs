using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace S4Demo.Framework.Helpers
{
  public class SeleniumHelper
  {
    private readonly IWebDriver _webDriver;
    private WaitHelper _waitHelper;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public SeleniumHelper(IWebDriver webDriver)
    {
      _webDriver = webDriver ?? throw new ArgumentNullException(nameof(webDriver));
      _waitHelper = new WaitHelper(webDriver);
    }

    // ===================================================================
    // CLICK / SUBMIT
    // ===================================================================

    public void Click(IWebElement element)
    {
      try
      {
        _waitHelper.WaitUntilClickable(element);
        new JavaScriptHelper(_webDriver).HighlightElement(element);
        string currentUrl = _webDriver.Url;
        element.Click();
        // Wait for a new page if navigation occurred
        if (_webDriver.Url != currentUrl)
          _waitHelper.WaitForPageToLoad();
      }
      catch (Exception e)
      {
        Logger.Error($"{e.Message}\r\n{e.StackTrace}\r\n");
      }
    }

    public void Click(By by)
    {
      try
      {
        var element = _waitHelper.WaitUntilClickable(by);
        _waitHelper.WaitUntilClickable(element);
        new JavaScriptHelper(_webDriver).HighlightElement(element);
        string currentUrl = _webDriver.Url;
        element.Click();
        // Wait for a new page if navigation occurred
        if (_webDriver.Url != currentUrl)
          _waitHelper.WaitForPageToLoad();
      }
      catch (Exception e)
      {
        Logger.Error($"{e.Message}\r\n{e.StackTrace}\r\n");
      }
    }

    public void Submit(IWebElement element)
    {
      try
      {
        _waitHelper.WaitUntilClickable(element);
        new JavaScriptHelper(_webDriver).HighlightElement(element);
        string currentUrl = _webDriver.Url;
        element.Submit();
        // Wait for a new page if navigation occurred
        if (_webDriver.Url != currentUrl)
          _waitHelper.WaitForPageToLoad();
      }
      catch (Exception e)
      {
        Logger.Error(e.Message + "\r\n" + e.StackTrace + "\r\n");
      }
    }

    public void Submit(By by)
    {
      try
      {
        var element = _waitHelper.WaitUntilClickable(by);
        new JavaScriptHelper(_webDriver).HighlightElement(element);
        string currentUrl = _webDriver.Url;
        element.Submit();
        // Wait for a new page if navigation occurred
        if (_webDriver.Url != currentUrl)
          _waitHelper.WaitForPageToLoad();
      }
      catch (Exception e)
      {
        Logger.Error(e.Message + "\r\n" + e.StackTrace + "\r\n");
      }
    }

    // ===================================================================
    // ENTER TEXT, CLEAR TEXT BOX
    // ===================================================================

    public void EnterText(IWebElement element, string textToType)
    {
      try
      {
        _waitHelper.WaitForElementToBeDisplayed(element);
        new JavaScriptHelper(_webDriver).HighlightElement(element);
        ClearTextbox(element);
        element.SendKeys(textToType);
      }
      catch (Exception e)
      {
        Logger.Error(e.Message + "\r\n" + e.StackTrace + "\r\n");
      }
    }

    public void ClearTextbox(IWebElement textBox)
    {
      try
      {
        _waitHelper.WaitForElementToBeDisplayed(textBox);
        textBox.Clear();
      }
      catch (Exception e)
      {
        Logger.Error(e.Message + "\r\n" + e.StackTrace + "\r\n");
      }

    }

    // ===================================================================
    // RETRIEVING TEXT / VALUES FROM ELEMENTS
    // ===================================================================

    public string GetTextFromElement(IWebElement element)
    {
      _waitHelper.WaitUntilVisible(element);
      new JavaScriptHelper(_webDriver).HighlightElement(element);
      var displayedText = element.Text;
      return displayedText;
    }

    public string GetTextFromElement(By by)
    {
      var element = _waitHelper.WaitUntilVisible(by);
      new JavaScriptHelper(_webDriver).HighlightElement(element);
      var displayedText = element.Text;
      return displayedText;
    }

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

    // ===================================================================
    // SELECT ELEMENTS, DROP-DOWNS
    // ===================================================================

    public void SelectItemByText(IWebElement element, string comboItem)
    {
      try
      {
        _waitHelper.WaitUntilVisible(element);
        new JavaScriptHelper(_webDriver).HighlightElement(element);
        var selectElement = new SelectElement(element);
        selectElement.SelectByText(comboItem);
      }
      catch (Exception e)
      {
        Logger.Error(e.Message + "\r\n" + e.StackTrace);
      }
    }

    public void SelectItemByValue(IWebElement element, string comboItem)
    {
      try
      {
        _waitHelper.WaitUntilVisible(element);
        new JavaScriptHelper(_webDriver).HighlightElement(element);
        var selectElement = new SelectElement(element);
        selectElement.SelectByValue(comboItem);
      }
      catch (Exception e)
      {
        Logger.Error(e.Message + "\r\n" + e.StackTrace);
      }
    }

    public int GetNumberOfOptionsInSelect(IWebElement element)
    {
      var selectElement = new SelectElement(element);
      return selectElement.Options.Count;
    }

    public string GetCurrentValueInSelect(IWebElement element)
    {
      var selectedValue = new SelectElement(element);
      return selectedValue.SelectedOption.Text;
    }

    // ===================================================================
    // VERIFICATIONS
    // ===================================================================

    public bool IsElementVisible(IWebElement element)
    {
      if (!element.Displayed) return false;
      new JavaScriptHelper(_webDriver).HighlightElement(element);
      return true;
    }

    public bool IsElementVisible(By by)
    {
      var element = _webDriver.FindElement(by);
      if (!element.Displayed) return false;
      new JavaScriptHelper(_webDriver).HighlightElement(element);
      return true;
    }

    public bool IsElementEnabled(IWebElement element)
    {
      return element.Enabled;
    }

    public bool IsElementEnabled(By by)
    {
      var element = _webDriver.FindElement(by);
      return element.Enabled;
    }

    public bool IsTextPresentOnPage(string expectedText)
    {
      return _webDriver.PageSource.Contains(expectedText);
    }

    public bool IsTextPresentInElement(IWebElement element, string expectedText)
    {
      _waitHelper.WaitUntilTextPresent(element,expectedText);
      return element.Text.Contains(expectedText);
    }

    public bool IsTextPresentInElement(By by, string expectedText)
    {
      var element = _webDriver.FindElement(by);
      _waitHelper.WaitUntilTextPresent(element, expectedText);
      return element.Text.Contains(expectedText);
    }

    public bool IsElementSelected(IWebElement element)
    {
      return element.Selected;
    }

    public bool IsElementSelected(By by)
    {
      var element = _webDriver.FindElement(by);
      return element.Selected;
    }

    public bool IsPageDisplayingExpectedElements(IReadOnlyList<IWebElement> expectedElements)
    {
      _waitHelper.WaitForPageToLoad();
      var expectedElementsAreDisplayed = true;
      var max = expectedElements.Count;
      var counter = 0;
      while (expectedElementsAreDisplayed && counter < max)
      {
        expectedElementsAreDisplayed = IsElementVisible(expectedElements[counter]);
        counter++;
      }
      return expectedElementsAreDisplayed;
    }

    // ===================================================================
    // SIMULATE KEY PRESS
    // ===================================================================

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
  }
}