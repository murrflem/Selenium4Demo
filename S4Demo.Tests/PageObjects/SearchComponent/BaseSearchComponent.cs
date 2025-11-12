using S4Demo.Tests.Pages;
using OpenQA.Selenium;
using System;
using S4Demo.Framework.Helpers;

namespace S4Demo.Tests.PageObjects.SearchComponent
{
  public class BaseSearchComponent : BasePage
  {
    public virtual IWebElement AdultsTextbox { get; set; }
    public virtual IWebElement ChildTextbox { get; set; }
    public virtual IWebElement PlusOneAdultButton { get; set; }
    public virtual IWebElement MinusOneAdultButton { get; set; }
    public virtual IWebElement PlusOneChildButton { get; set; }
    public virtual IWebElement MinusOneChildButton { get; set; }
    public virtual IWebElement SearchButton { get; set; }
    public IWebElement HotelsLink => WebDriver.FindElement(
        By.CssSelector("button[data-bs-target='#tab-hotels'] span"));
    public IWebElement TravellersDropDown => WebDriver.FindElement(
        By.CssSelector(".dropdown-toggle.dropdown-btn.travellers.d-flex.align-items-center.waves-effect"));
    public IWebElement ChildsAgeSelect => WebDriver.FindElement(By.CssSelector("#ages1"));

    public BaseSearchComponent(IWebDriver driver) : base(driver)
    {
    }

    // --------------------------------------------------------------------
    // 🔹 Enter number of adults
    // --------------------------------------------------------------------
    public void EnterNumberOfAdults(int numberOfAdults)
    {
      FrameworkLogger.LogStep($"Setting number of adults to {numberOfAdults}");
      Selenium.Click(TravellersDropDown);

      var displayedAdults = Convert.ToInt32(Selenium.GetElementValueAttribute(AdultsTextbox));
      if (displayedAdults == numberOfAdults)
      {
        FrameworkLogger.LogStep("Adults count already correct — no changes needed.");
        return;
      }

      if (displayedAdults > numberOfAdults)
      {
        var diff = displayedAdults - numberOfAdults;
        FrameworkLogger.LogStep($"Decreasing adults count by {diff}.");
        for (int i = 0; i < diff; i++)
        {
          Selenium.Click(MinusOneAdultButton);
        }
      }
      else
      {
        var diff = numberOfAdults - displayedAdults;
        FrameworkLogger.LogStep($"Increasing adults count by {diff}.");
        for (int i = 0; i < diff; i++)
        {
          Selenium.Click(PlusOneAdultButton);
        }
      }
    }

    // --------------------------------------------------------------------
    // 🔹 Enter number of children
    // --------------------------------------------------------------------
    public void EnterNumberOfChildren(int numberOfChildren)
    {
      FrameworkLogger.LogStep($"Setting number of children to {numberOfChildren}");

      var displayedChildren = Convert.ToInt32(Selenium.GetElementValueAttribute(ChildTextbox));
      if (displayedChildren == numberOfChildren)
      {
        FrameworkLogger.LogStep("Children count already correct — no changes needed.");
        return;
      }

      if (displayedChildren > numberOfChildren)
      {
        var diff = displayedChildren - numberOfChildren;
        FrameworkLogger.LogStep($"Decreasing children count by {diff}.");
        for (int i = 0; i < diff; i++)
        {
          Selenium.Click(MinusOneChildButton);
        }
      }
      else
      {
        var diff = numberOfChildren - displayedChildren;
        FrameworkLogger.LogStep($"Increasing children count by {diff}.");
        for (int i = 0; i < diff; i++)
        {
          Selenium.Click(PlusOneChildButton);
        }
      }

      EnterChildsAge(numberOfChildren);
    }

    // --------------------------------------------------------------------
    // 🔹 Enter child age (can be expanded later)
    // --------------------------------------------------------------------
    public void EnterChildsAge(int numOfChildren)
    {
      FrameworkLogger.LogStep($"Selecting child age for {numOfChildren} child(ren).");
      // Placeholder for multi-child handling later
      Selenium.SelectItemByValue(ChildsAgeSelect, "10");
    }
  }
}