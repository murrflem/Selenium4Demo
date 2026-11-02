using OpenQA.Selenium;
using S4Demo.Tests.Pages;

namespace S4Demo.Tests.PageObjects
{
  public class LoginPage : BasePage
  {
    public override IWebElement NavigationLink => WebDriver.FindElement(By.CssSelector("#customer_menu_top > li > a"));
    public IWebElement LoginButton => WebDriver.FindElement(By.CssSelector("#loginFrm > fieldset > button"));
    public IWebElement ContinueButton => WebDriver.FindElement(By.CssSelector("#accountFrm > fieldset > button"));

    public LoginPage(IWebDriver webdriver) : base(webdriver)
    {

    }
  }
}