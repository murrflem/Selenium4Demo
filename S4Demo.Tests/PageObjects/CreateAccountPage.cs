using OpenQA.Selenium;
using S4Demo.Tests.Pages;
using S4Demo.Tests.TestModels;


namespace S4Demo.Tests.PageObjects
{
  public class CreateAccountPage : BasePage
  {
    public override IWebElement NavigationLink => WebDriver.FindElement(By.CssSelector("#customer_menu_top > li > a"));
    public IWebElement FirstNameInput => WebDriver.FindElement(By.Id("AccountFrm_firstname"));
    public IWebElement LastNameInput => WebDriver.FindElement(By.Id("AccountFrm_lastname"));
    public IWebElement EmailInput => WebDriver.FindElement(By.Id("AccountFrm_email"));
    public IWebElement AddressInput => WebDriver.FindElement(By.Id("AccountFrm_address_1"));
    public IWebElement CityInput => WebDriver.FindElement(By.Id("AccountFrm_city"));
    public IWebElement Region => WebDriver.FindElement(By.Id("AccountFrm_zone_id"));
    public IWebElement Country => WebDriver.FindElement(By.Id("AccountFrm_country_id"));
    public IWebElement ZipCode => WebDriver.FindElement(By.Id("AccountFrm_postcode"));
    public IWebElement LoginName => WebDriver.FindElement(By.Id("AccountFrm_loginname"));
    public IWebElement PasswordInput => WebDriver.FindElement(By.Id("AccountFrm_password"));
    public IWebElement SubscribeNoRadioBtn => WebDriver.FindElement(By.Id("AccountFrm_newsletter0"));
    public IWebElement ConfirmPasswordInput => WebDriver.FindElement(By.Id("AccountFrm_confirm"));
    public IWebElement TermsAndConditionsCheckbox => WebDriver.FindElement(By.Id("AccountFrm_agree"));
    public IWebElement RegisterButton => WebDriver.FindElement(By.CssSelector("#AccountFrm > div.form-group > div > div > button"));
    public IWebElement ContinueButton => WebDriver.FindElement(By.CssSelector("#maincontainer > div > div.col-md-9.col-xs-12.mt20 > div > div > section > a"));
    public IWebElement SuccessHeading => WebDriver.FindElement(By.CssSelector("#maincontainer > div > div.col-md-9.col-xs-12.mt20 > div > h1"));
    public CreateAccountPage(IWebDriver webdriver) : base(webdriver)
    {

    }

    public void FillOutRegistrationForm(RegistrationData data)
    {
      Selenium.EnterText(FirstNameInput, data.FirstName);
      Selenium.EnterText(LastNameInput, data.LastName);
      Selenium.EnterText(EmailInput, data.Email);
      Selenium.EnterText(AddressInput, data.Address);
      Selenium.EnterText(CityInput, data.City);
      Selenium.SelectItemByText(Country, data.Country);
      Selenium.EnterText(ZipCode, data.ZipCode);
      Selenium.SelectItemByText(Region, data.Region);
      Selenium.EnterText(LoginName, data.LoginName);
      Selenium.EnterText(PasswordInput, data.Password);
      Selenium.EnterText(ConfirmPasswordInput, data.Password);
      Selenium.Click(SubscribeNoRadioBtn);
      Selenium.Click(TermsAndConditionsCheckbox);
      Selenium.Click(RegisterButton);
    }

    public bool RegistrationSuccessful(string expectedMessage)
    {
      return Selenium.IsTextPresentInElement(SuccessHeading, expectedMessage);
    }
  }
}