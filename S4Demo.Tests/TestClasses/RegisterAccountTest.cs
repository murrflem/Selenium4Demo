using NUnit.Framework;
using S4Demo.Framework.Helpers;
using S4Demo.Tests.PageObjects;
using S4Demo.Tests.TestModels;
using Shouldly;
using System;

namespace S4Demo.Tests.TestClasses
{
  [TestFixture]
  [Parallelizable(ParallelScope.All)]
  public class RegisterAccountTest : BaseTest
  {
    private LoginPage _loginPage;
    private CreateAccountPage _createAccountPage;

    [SetUp]
    public void Init()
    {
      _loginPage = new LoginPage(WebDriver);
      _createAccountPage = new CreateAccountPage(WebDriver);
    }

    [Test]
    public void RegisterAccountShouldSucceed()
    {
      var successMessage = "Your Account Has Been Created!";
      var user = new RegistrationData
      {
        FirstName = "Jane",
        LastName = "Tester",
        Email = CommonHelper.RandomEmail(),
        Address = "123 Main St",
        City = "Vancouver",
        Country = "Canada",
        Region = "British Columbia",
        ZipCode = "V6B 1A1",
        LoginName = $"jtester_{Guid.NewGuid():N}".Substring(0, 15),
        Password = "P@ssw0rd123!"
      };

      _loginPage.NavigationLink.Click();
      _loginPage.ContinueButton.Click();
      _createAccountPage.FillOutRegistrationForm(user);
      _createAccountPage.RegistrationSuccessful(successMessage).ShouldBeTrue();
    }
  }
}