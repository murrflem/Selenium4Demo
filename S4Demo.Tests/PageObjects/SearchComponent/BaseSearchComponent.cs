using S4Demo.Tests.Pages;
using OpenQA.Selenium;
using System;

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
    public IWebElement ChildsAgeSelect => WebDriver.FindElement(
      By.CssSelector("#ages1"));


    public BaseSearchComponent(IWebDriver driver) : base(driver)
    {

    }

    public void EnterNumberOfAdults(int numberOfAdults)
    {
      Selenium.Click(TravellersDropDown);
      var numberOfDisplayedAdults = Convert.ToInt32(Selenium.GetElementValueAttribute(AdultsTextbox));
      if (numberOfDisplayedAdults == Convert.ToInt32(numberOfAdults))
      {
        return;
      }
      if (numberOfDisplayedAdults > numberOfAdults)
      {
        var diff = numberOfDisplayedAdults - numberOfAdults;
        for (int i = 0; i < diff; i++)
        {
          Selenium.Click(MinusOneAdultButton);
        }
      }
      if (numberOfDisplayedAdults < numberOfAdults)
      {
        var diff = numberOfAdults - numberOfDisplayedAdults;
        for (int i = 0; i < diff; i++)
        {
          Selenium.Click(PlusOneAdultButton);
        }
      }
    }

    public void EnterNumberOfChildren(int numberOfChildren)
    {
      var numberOfDisplayedChildren = Convert.ToInt32(Selenium.GetElementValueAttribute(ChildTextbox));
      if (numberOfDisplayedChildren == Convert.ToInt32(numberOfChildren))
      {
        return;
      }
      if (numberOfDisplayedChildren > numberOfChildren)
      {
        var diff = numberOfDisplayedChildren - numberOfChildren;
        for (int i = 0; i < diff; i++)
        {
          Selenium.Click(MinusOneChildButton);
        }
      }
      if (numberOfDisplayedChildren < numberOfChildren)
      {
        var diff = numberOfChildren - numberOfDisplayedChildren;
        for (int i = 0; i < diff; i++)
        {
          Selenium.Click(PlusOneChildButton);
        }
      }
      EnterChildsAge(numberOfChildren);
    }

    public void EnterChildsAge(int numOfChildren)
    {
      //Can add logic for handling multiple children later
      Selenium.SelectItemByValue(ChildsAgeSelect, "10");
    }
  }
}