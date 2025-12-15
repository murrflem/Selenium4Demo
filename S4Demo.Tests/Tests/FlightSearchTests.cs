using NUnit.Framework;
using S4Demo.Framework.Helpers;
using S4Demo.Framework.Setup;
using S4Demo.Tests.Models;
using S4Demo.Tests.PageObjects;
using S4Demo.Tests.PageObjects.SearchComponent;
using Shouldly;
using System;
using System.Linq;
using System.Net;

namespace S4Demo.Tests.Tests
{
  [TestFixture, Parallelizable]
  public class FlightSearchTests : BaseTest
  {
    private HomePage _homePage;
    private FlightSearchComponent _flightSearchComponent;
    private SearchResultsPage _searchResultsPage;

    [SetUp]
    public void Init()
    {
      _homePage = new HomePage(WebDriver);
      _flightSearchComponent = new FlightSearchComponent(WebDriver);
      _searchResultsPage = new SearchResultsPage(WebDriver);
    }

    [Test]
    //[Ignore("Enable When Needed")]
    public void CheckForBrokenLinks()
    {
      _flightSearchComponent.VerifyPageLinks(); // Check all links on the current page
    }

    [Test]
    public void SearchForFlightsTest()
    {
      // ================ LOAD TEST DATA FROM CSV FILE ==============================
      //Pick the first row
      //var testData = TestDataHelper.LoadCsv<FlightData>("TestData/FlightData.csv");
      //var row = testData.First();

      //Pick a random row from the CSV file
      //var row = TestDataHelper.LoadRandomCsvRow<FlightData>("TestData/FlightData.csv");
      // ==============================================================================

      // ================ LOAD TEST DATA FROM EXCEL FILE ==============================
      // Pick a random row 
      var row = TestDataHelper.LoadRandomExcelRow<FlightData>("TestData/FlightData.xlsx", "FlightData");
      
      //Pick a specific row (e.g., 1st row)
      //var row = TestDataHelper.LoadExcel<FlightData>("TestData/FlightData.xlsx","FlightData").First();
      // ==============================================================================

      var departureCode = row.DepartureCode;
      var arrivalCode = row.ArrivalCode;

      _flightSearchComponent.SearchForFlights(departureCode, arrivalCode);
      _searchResultsPage.SearchesDropdown.Displayed.ShouldBeTrue();
      _searchResultsPage.ClearSearches();
      _homePage.NavigateToPage();

      // Check page status code response time
      var statusResult = HttpHelper.CheckUrlStatus(WebDriver.Url).GetAwaiter().GetResult();
      Console.WriteLine($"Status: {statusResult.StatusCode}, Time: {statusResult.ResponseTimeMs} ms");
      var responseTime = statusResult.ResponseTimeMs;
      statusResult.StatusCode.ShouldBe(HttpStatusCode.OK);
      statusResult.ResponseTimeMs.ShouldBeLessThanOrEqualTo(ConfigManager.MaxExpectedResponseTimeMs);
    }
  } 
}