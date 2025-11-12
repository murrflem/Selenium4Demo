using S4Demo.Tests.PageObjects.SearchComponent;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using S4Demo.Framework.Helpers;

namespace S4Demo.Tests.PageObjects
{
  public class HotelSearchComponent : BaseSearchComponent
  {
    private readonly WaitHelper _waitHelper;

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

    // --------------------------------------------------------------------
    // 🔹 Search for the default hotel package
    // --------------------------------------------------------------------
    public void SearchForDefaultPackage()
    {
      FrameworkLogger.LogStep("Starting default hotel package search.");
      Selenium.Click(HotelsLink);
      FrameworkLogger.LogStep("Clicked on Hotels tab.");

      FrameworkLogger.LogStep("Selecting default destination from dropdown.");
      Selenium.Click(By.CssSelector("#select2-hotels_city-container"));
      Selenium.Click(By.CssSelector("#select2-hotels_city-results > div > div:nth-child(2) > div > strong"));
      FrameworkLogger.LogStep("Destination selected.");

      FrameworkLogger.LogStep("Clicking Search button.");
      SearchButton.Click();
      _waitHelper.WaitForAjaxAndPageLoad();
      FrameworkLogger.LogWait("Waiting for search results to load after clicking Search.");
    }

    // --------------------------------------------------------------------
    // 🔹 Search for a hotel package using criteria
    // --------------------------------------------------------------------
    public void SearchForHotelPackage(List<string> criteria)
    {
      FrameworkLogger.LogStep("Starting hotel package search with custom criteria.");
      Selenium.Click(HotelsLink);
      FrameworkLogger.LogStep("Clicked on Hotels tab.");

      SearchByDestination(criteria[0]);
      EnterDates(criteria[1], criteria[2]);
      EnterNumberOfAdults(Convert.ToInt32(criteria[3]));
      EnterNumberOfChildren(Convert.ToInt32(criteria[4]));

      FrameworkLogger.LogStep("Expanding travellers dropdown before search.");
      Selenium.Click(TravellersDropDown);

      FrameworkLogger.LogStep("Submitting search form.");
      Selenium.Submit(SearchButton);
      _waitHelper.WaitForAjaxAndPageLoad();
      FrameworkLogger.LogWait("Waiting for search results to load after submitting form.");
    }

    // --------------------------------------------------------------------
    // 🔹 Supporting private methods with logging
    // --------------------------------------------------------------------
    private void SearchByDestination(string destination)
    {
      FrameworkLogger.LogStep($"Searching for destination: {destination}");
      Selenium.Click(By.Id("select2-hotels_city-container"));
      Selenium.EnterText(DestinationSearchbox, destination);
      _waitHelper.WaitForAjaxAndPageLoad();
      FrameworkLogger.LogWait("Waiting for search suggestions to load.");
      Selenium.Click(By.CssSelector("li[role='option']"));
      FrameworkLogger.LogStep("Destination selected from search results.");
    }

    private void EnterDates(string checkInDate, string checkOutDate)
    {
      FrameworkLogger.LogStep($"Entering check-in date: {checkInDate}");
      Selenium.EnterText(CheckInTextbox, checkInDate);
      Selenium.Click(CheckInTextbox);

      FrameworkLogger.LogStep($"Entering check-out date: {checkOutDate}");
      Selenium.EnterText(CheckOutTextbox, checkOutDate);
      Selenium.Click(CheckOutTextbox);
    }
  }
}