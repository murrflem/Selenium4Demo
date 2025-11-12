using OpenQA.Selenium;
using S4Demo.Framework.Helpers;

namespace S4Demo.Tests.PageObjects.SearchComponent
{
  public class FlightSearchComponent : BaseSearchComponent
  {
    private WaitHelper _waitHelper;
    public IWebElement FlightsLink => WebDriver.FindElement(By.CssSelector("#tab > li:nth-child(1) > button > span"));
    public IWebElement FromTextbox => WebDriver.FindElement(By.Name("from"));
    public IWebElement ToTextbox => WebDriver.FindElement(By.Name("to"));
    public IWebElement FromSelectInput => WebDriver.FindElement(
      By.CssSelector("#onereturn > div.col-12.col-md-3.show.active > div > input"));
    public IWebElement ToSelectInput => WebDriver.FindElement(
      By.CssSelector("#onereturn > div:nth-child(2) > div.form-floating.flight_search > input"));
    public IWebElement FromDropDownSelectResult => WebDriver.FindElement(
      By.CssSelector("#onereturn > div.col-12.col-md-3.show.active > div > div > div > button"));
    public IWebElement ToDropDownSelectResult => WebDriver.FindElement(
      By.CssSelector("#onereturn > div:nth-child(2) > div.form-floating.flight_search > div > div:nth-child(1) > button"));
    public IWebElement DepartDateInput => WebDriver.FindElement(By.Id("departure"));
    public override IWebElement SearchButton => WebDriver.FindElement(By.XPath("(//button[@id='flights-search'])[1]"));
    
    public FlightSearchComponent(IWebDriver driver) : base(driver)
    {
      _waitHelper = new WaitHelper(driver);
    }

    public void SearchForFlights(string departureCode, string arrivalCode)
    {
      var x = CommonHelper.RandomString(14);
      Selenium.Click(FlightsLink);
      EnterDepartureAiport(departureCode);
      _waitHelper.WaitForAjaxAndPageLoad();
      EnterArrivalAirport(arrivalCode);
      _waitHelper.WaitForAjaxAndPageLoad();
      EnterDepartureDate();
      _waitHelper.WaitForAjaxAndPageLoad();
      SearchButton.Click();
      _waitHelper.WaitForAjaxAndPageLoad();
    }

    public void EnterDepartureAiport(string departureAirport)
    {
      FromTextbox.Click();
      FromSelectInput.SendKeys(departureAirport);
      FromDropDownSelectResult.Click();
    }

    public void EnterArrivalAirport(string arrivalAirport)
    {
      ToTextbox.Click();
      ToSelectInput.SendKeys(arrivalAirport);
      ToDropDownSelectResult.Click();
    }

    public void EnterDepartureDate()
    {
      DepartDateInput.Click();
      WebDriver.FindElement(By.CssSelector("body > div:nth-child(16) > div:nth-child(1) > table:nth-child(1) > tbody:nth-child(2) > tr:nth-child(4) > td:nth-child(4)")).Click();
    }
  }
}