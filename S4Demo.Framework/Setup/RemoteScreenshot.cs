using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;

namespace S4Demo.Framework.Setup
{
  public class RemoteScreenshot : RemoteWebDriver, ITakesScreenshot
  {
    public RemoteScreenshot(Uri remoteAddress, ICapabilities capabilities) : base(remoteAddress, capabilities)
    {

    }

    public new Screenshot GetScreenshot()
    {
      var screenshotResponse = this.Execute(DriverCommand.Screenshot, null);
      var base64 = screenshotResponse.Value.ToString();
      return new Screenshot(base64);
    }
  }
}