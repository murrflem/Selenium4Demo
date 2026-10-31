using NUnit.Framework;
using S4Demo.Tests.PageObjects;
using S4Demo.Tests.Tests;
using Shouldly;

namespace S4Demo.Tests.TestClasses
{
  [TestFixture, Parallelizable]
  public class CategoryMenuNavigationTests : BaseTest
  {
    private CategoryMenu _categoryMenu;

    [SetUp]
    public void Init()
    {
      _categoryMenu = new CategoryMenu(WebDriver);
    }

    [TestCase("Apparel & Accessories", "Apparel")]
    [TestCase("Makeup", "Makeup")]
    [TestCase("Skincare", "Skincare")]
    [TestCase("Fragrance", "Fragrance")]
    [TestCase("Men", "Men")]
    [TestCase("Hair Care", "Hair Care")]
    [TestCase("Books", "Books")]
    public void CategoryMenuShouldNavigateToExpectedPage(
      string categoryName,
      string expectedTitle)
    {
      _categoryMenu.IsDisplayed().ShouldBeTrue();

      _categoryMenu.SelectCategory(categoryName);

      WebDriver.Title.ShouldContain(expectedTitle);
    }
  }
}