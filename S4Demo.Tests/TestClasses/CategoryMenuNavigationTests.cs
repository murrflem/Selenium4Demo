using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using S4Demo.Tests.PageObjects;
using Shouldly;
using System;

namespace S4Demo.Tests.TestClasses
{
  [TestFixture]
  [Parallelizable(ParallelScope.Self)]
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

      var wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(10));

      wait.Until(driver =>
          driver.Title.Contains(expectedTitle, StringComparison.OrdinalIgnoreCase));

      WebDriver.Title.ShouldContain(expectedTitle);
    }
  }
}