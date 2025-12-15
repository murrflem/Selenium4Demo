using NUnit.Framework;
using S4Demo.Framework.Helpers;
using S4Demo.Tests.PageObjects;
using S4Demo.Tests.PageObjects.SearchComponent;
using Shouldly;
using System.Collections.Generic;

namespace S4Demo.Tests.Tests
{
  [TestFixture, Parallelizable]
  public class HotelsSearchTests : BaseTest
  {
    private HotelSearchComponent _hotelSearchComponent;
    private SearchResultsPage _searchResultsPage;
    private ScreenshotHelper _screenshotHelper;

    [SetUp]
    public void Init()
    {
      _hotelSearchComponent = new HotelSearchComponent(WebDriver);
      _searchResultsPage = new SearchResultsPage(WebDriver);
      _screenshotHelper = new ScreenshotHelper(WebDriver);
    }


    [Test]
    [Ignore("Enable When Needed")]
    public void CheckForBrokenImages()
    {
      _hotelSearchComponent.VerifyPageImages(); // Check all images on the current page
    }

    [Test]
    public void SearchForHotelPackages()
    {
      var destination = "Miami";
      List<string> searchCriteria = new List<string>();
      searchCriteria.Add(destination);
      searchCriteria.Add(@"01/12/2026");
      searchCriteria.Add(@"07/12/2026");
      searchCriteria.Add("1");
      searchCriteria.Add("1");
      //_hotelSearchComponent.SearchForHotelPackage(searchCriteria);

      _hotelSearchComponent.SearchForDefaultPackage();
      var resultsReturned = _searchResultsPage.TravelInfoDisplayed();
      resultsReturned.ShouldNotBeEmpty();

      // ================== Visual comparison of the results page ===================
      // Uses global default tolerance from appsettings.json
      _screenshotHelper.ComparePage("HotelSearchResults");
      // Overrides global tolerance setting for this specific comparison
      //_screenshotHelper.ComparePage("HotelSearchResults", tolerance: 10);
      //============================================================================

      //resultsReturned.ShouldContain(destination);

    }
  }
}