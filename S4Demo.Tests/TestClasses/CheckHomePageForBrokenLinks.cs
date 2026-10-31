using NUnit.Framework;
using S4Demo.Tests.PageObjects;

namespace S4Demo.Tests.Tests
{
  [TestFixture, Parallelizable]
  public class CheckHomePageForBrokenLinks : BaseTest
  {
    private HomePage _homePage;

    [SetUp]
    public void Init()
    {
      _homePage = new HomePage(WebDriver);
    }

    [Test]
    public void CheckForBrokenLinks()
    {
      _homePage.VerifyPageLinks(); // Check all links on the current page
    }
  }
}
