using OpenQA.Selenium;
using S4Demo.Framework.Helpers;
using S4Demo.Framework.Setup;

namespace S4Demo.Tests.Pages
{
  public  class BasePage : FrameworkBasePage
  {
    public virtual IWebElement NavigationLink { get; set; }

    public BasePage(IWebDriver webdriver) : base(webdriver)
    {

    }

    public virtual void NavigateToPage()
    {
      if(NavigationLink == null)
      {
        return;
      }
      new WaitHelper(WebDriver).WaitUntilClickable(NavigationLink);
      Selenium.Click(NavigationLink);
    }
  }
}