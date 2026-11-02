using S4Demo.Tests.Pages;
using OpenQA.Selenium;

namespace S4Demo.Tests.PageObjects
{
  public class HomePage : BasePage
  {
    public override IWebElement NavigationLink => WebDriver.FindElement(By.CssSelector(".header-logo > a"));
    public By LoginOrRegisterLink =>  By.CssSelector("#customer_menu_top > li > a");
    public HomePage(IWebDriver webdriver) : base(webdriver)
    {

    }

  }
}