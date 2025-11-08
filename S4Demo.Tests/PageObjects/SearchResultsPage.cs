using S4Demo.Tests.Pages;
using OpenQA.Selenium;
using S4Demo.Framework.Helpers;

namespace S4Demo.Tests.PageObjects
{
  public class SearchResultsPage : BasePage
  {
    private WaitHelper _waitHelper;
    public IWebElement DestinationAndTravellersHeader => WebDriver.FindElement(By.CssSelector(".text-capitalize.mx-1"));
    public IWebElement SearchesDropdown => WebDriver.FindElement(By.CssSelector("strong[class='h6 m-0 header_options text-dark']"));
    public IWebElement ClearSearchesButton => WebDriver.FindElement(By.XPath("(//strong[normalize-space()='Clear Searches'])[1]"));

    public SearchResultsPage(IWebDriver webdriver) : base(webdriver)
    {
      _waitHelper = new WaitHelper(webdriver);
    }

    public void ClearSearches()
    {
      Selenium.Click(SearchesDropdown);
      Selenium.Click(ClearSearchesButton);
    }

    public string TravelInfoDisplayed()
    {
      _waitHelper.WaitForElementToBeDisplayed(DestinationAndTravellersHeader);
      return Selenium.GetTextFromElement(DestinationAndTravellersHeader);
    }
  }
}