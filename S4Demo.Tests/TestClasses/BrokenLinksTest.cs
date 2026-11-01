using NUnit.Framework;
using S4Demo.Tests.PageObjects;

namespace S4Demo.Tests.TestClasses
{
  [TestFixture]
  [Parallelizable(ParallelScope.Self)]
  public class BrokenLinksTest : BaseTest
  {
    private HomePage _homePage;

    [SetUp]
    public void Init()
    {
      _homePage = new HomePage(WebDriver);
    }

    [Test]
    public void CheckForBrokenLinksOnHomePage()
    {
      _homePage.VerifyPageLinks(); // Check all links on the current page
    }
  }
}