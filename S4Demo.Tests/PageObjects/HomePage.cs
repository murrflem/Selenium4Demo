using S4Demo.Tests.Pages;
using OpenQA.Selenium;

namespace S4Demo.Tests.PageObjects
{
  public class HomePage : BasePage
  {
    public override IWebElement NavigationLink => WebDriver.FindElement(By.CssSelector(".logo.p-1.rounded"));
    public HomePage(IWebDriver webdriver) : base(webdriver)
    {

    }

  }
}