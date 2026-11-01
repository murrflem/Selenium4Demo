using NUnit.Framework;
using OpenQA.Selenium;
using S4Demo.Framework.Helpers;
using S4Demo.Tests.TestModels;
using Shouldly;
using System.Linq;

namespace S4Demo.Tests.TestClasses
{
  [TestFixture]
  [Parallelizable(ParallelScope.All)]
  public class SearchTests : BaseTest
  {
    [SetUp]
    public void Init()
    {
      
    }

    [Test]
    public void SearchShouldReturnExpectedResults()
    {

      // ================ LOAD TEST DATA FROM CSV FILE ==============================
      //Pick a specific row (i.e. , the first row) from the CSV file
      //var testData = TestDataHelper.LoadCsv<SearchData>("TestData/SearchData.csv");
      //var row = testData.First();
      //Pick a random row from the CSV file
      var row = TestDataHelper.LoadRandomCsvRow<SearchData>("TestData/SearchData.csv");
      // ==============================================================================

      var searchBox = WebDriver.FindElement(By.Id("filter_keyword"));
      var searchTerm = row.SearchTerm; // Use the search term from the CSV file
      var expectedText = row.ExpectedText; // Use the expected text from the CSV file

      searchBox.SendKeys(searchTerm);
      searchBox.SendKeys(Keys.Enter);

      string title = WebDriver.Title;
      if (title.Contains("Search"))
      {
        WebDriver.FindElement(By.Id("keyword")).GetDomProperty("value").ShouldBe(expectedText);
      }
      else
      Assert.That(title.Contains(expectedText),
          $"Expected title '{title}' to contain '{expectedText}'.");
    }
  }
}