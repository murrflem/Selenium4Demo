using S4Demo.Tests.PageObjects.SearchComponent;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using S4Demo.Framework.Helpers;

namespace S4Demo.Tests.PageObjects
{
  public class HotelSearchComponent : BaseSearchComponent
  {
    private WaitHelper _waitHelper;
    public IWebElement DestinationSearchbox => WebDriver.FindElement(By.CssSelector("input[role='searchbox']"));
    public IWebElement CheckInTextbox => WebDriver.FindElement(By.Id("checkin"));
    public IWebElement CheckOutTextbox => WebDriver.FindElement(By.Id("checkout"));
    public override IWebElement AdultsTextbox => WebDriver.FindElement(By.Id("hotels_adults"));
    public override IWebElement PlusOneAdultButton => WebDriver.FindElement(
      By.CssSelector("form[id='hotels-search'] div[class='dropdown-item'] div[class='qtyBtn d-flex align-items-center'] div[class='qtyInc'] svg"));
    public override IWebElement MinusOneAdultButton => WebDriver.FindElement(
    By.CssSelector("form[id='hotels-search'] div[class='dropdown-item'] div[class='qtyBtn d-flex align-items-center'] div[class='qtyDec'] svg"));
    public override IWebElement PlusOneChildButton => WebDriver.FindElement(
      By.CssSelector("div[class='qtyBtn d-flex align-items-center child_ages'] div[class='qtyInc'] svg"));
    public override IWebElement MinusOneChildButton => WebDriver.FindElement(
      By.CssSelector("div[class='qtyBtn d-flex align-items-center child_ages'] div[class='qtyDec'] svg"));
    public override IWebElement ChildTextbox => WebDriver.FindElement(By.Id("hotels_childs"));
    public override IWebElement SearchButton => WebDriver.FindElement(
      By.CssSelector("form[id='hotels-search'] button[type='submit']"));


    public HotelSearchComponent(IWebDriver driver) : base(driver)
    {
      _waitHelper = new WaitHelper(driver);
    }

    public void SearchForDefaultPackage()
    {
      Selenium.Click(HotelsLink);
      Selenium.Click(By.CssSelector("#select2-hotels_city-container"));
      Selenium.Click(By.CssSelector("#select2-hotels_city-results > div > div:nth-child(2) > div > strong"));
      SearchButton.Click();
    }

    public void SearchForHotelPackage(List<string> criteria)
    {
      Selenium.Click(HotelsLink);
      SearchByDestination(criteria[0]);
      EnterDates(criteria[1], criteria[2]);
      EnterNumberOfAdults(Convert.ToInt32(criteria[3]));
      EnterNumberOfChildren(Convert.ToInt32(criteria[4]));
      Selenium.Click(TravellersDropDown);
      Selenium.Submit(SearchButton);
      _waitHelper.WaitForAjaxAndPageLoad();
    }

    private void SearchByDestination(string destination)
    {
      Selenium.Click(By.Id("select2-hotels_city-container"));
      Selenium.EnterText(DestinationSearchbox, destination);
      _waitHelper.WaitForAjaxAndPageLoad();
      Selenium.Click(By.CssSelector("li[role='option']"));
    }

    private void EnterDates(string checkInDate, string checkOutDate)
    {
      Selenium.EnterText(CheckInTextbox, checkInDate);
      Selenium.Click(CheckInTextbox);
      Selenium.EnterText(CheckOutTextbox, checkOutDate);
      Selenium.Click(CheckOutTextbox);
    }
  }
}