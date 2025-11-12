using NUnit.Framework;
using S4Demo.Framework.Helpers;
using S4Demo.Tests.PageObjects;
using S4Demo.Tests.PageObjects.SearchComponent;
using Shouldly;

namespace S4Demo.Tests.Tests
{
  [TestFixture, Parallelizable]
  public class FlightSearchTests : BaseTest
  {
    private HomePage _homePage;
    private FlightSearchComponent _flightSearchComponent;
    private SearchResultsPage _searchResultsPage;
    private ScreenshotHelper _screenshotHelper;

    [SetUp]
    public void Init()
    {
      _homePage = new HomePage(WebDriver);
      _flightSearchComponent = new FlightSearchComponent(WebDriver);
      _searchResultsPage = new SearchResultsPage(WebDriver);
      _screenshotHelper = new ScreenshotHelper(WebDriver);
    }

    [Test]
    public void SearchForFlightsTest()
    {
      var departureCode = "YVR";
      var arrivalCode = "YYZ";
      _flightSearchComponent.SearchForFlights(departureCode, arrivalCode);
      _searchResultsPage.SearchesDropdown.Displayed.ShouldBeTrue();
      _searchResultsPage.ClearSearches();
      _homePage.NavigateToPage();
      _screenshotHelper.ScreenshotPage();
    }
  }
}