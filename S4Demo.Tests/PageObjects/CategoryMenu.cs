using OpenQA.Selenium;
using S4Demo.Tests.Pages;
using System.Collections.Generic;
using System.Linq;

namespace S4Demo.Tests.PageObjects
{
  public class CategoryMenu : BasePage
  {
    private By MenuLocator => By.CssSelector("#categorymenu > nav > ul");
    private By CategoryLinksLocator => By.CssSelector("#categorymenu > nav > ul > li > a");

    public CategoryMenu(IWebDriver webdriver) : base(webdriver)
    {
    }

    public IReadOnlyCollection<IWebElement> CategoryLinks =>
      WebDriver.FindElements(CategoryLinksLocator);

    public void SelectCategory(string categoryName)
    {
      var category = CategoryLinks
          .First(link => link.Text.Trim().Equals(categoryName, System.StringComparison.OrdinalIgnoreCase));

      Selenium.Click(category);
    }

    public bool IsDisplayed()
    {
      return WebDriver.FindElement(MenuLocator).Displayed;
    }
  }
}