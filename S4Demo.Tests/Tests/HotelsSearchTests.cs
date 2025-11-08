using NUnit.Framework;
using S4Demo.Tests.PageObjects;
using Shouldly;
using System.Collections.Generic;

namespace S4Demo.Tests.Tests
{
  [TestFixture, Parallelizable]
  public class HotelsSearchTests : BaseTest
  {
    private HotelSearchComponent _hotelSearchComponent;
    private SearchResultsPage _searchResultsPage;

    [SetUp]
    public void Init()
    {
      _hotelSearchComponent = new HotelSearchComponent(WebDriver);
      _searchResultsPage = new SearchResultsPage(WebDriver);
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
      //resultsReturned.ShouldContain(destination);
      
    }
  }
}