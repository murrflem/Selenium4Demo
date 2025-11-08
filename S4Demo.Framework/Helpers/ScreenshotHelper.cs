using NLog;
using OpenQA.Selenium;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using S4Demo.Framework.Setup;

namespace S4Demo.Framework.Helpers
{
  public class ScreenshotHelper
  {
    private readonly IWebDriver _webDriver;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public ScreenshotHelper(IWebDriver webDriver)
    {
      _webDriver = webDriver;
    }

    public void ScreenshotPage()
    {
      var enabled = TestSettings.EnableScreenshots;
      if (!enabled.Equals(true)) return;
      try
      {
        var page = _webDriver.Title;
        var ss = ((ITakesScreenshot)_webDriver).GetScreenshot();
        var fName = TestSetup.PageScreenshotsFolder + "\\" + page + ".png";
        ss.SaveAsFile(fName);
      }
      catch(Exception ex)
      {
        Logger.Error(ex.Message);
      }
    }

    public void ScreenshotPage(string nameOfScreenshot)
    {
      var enabled = TestSettings.EnableScreenshots;
      if (!enabled.Equals(true)) return;
      try
      {
        var ss = ((ITakesScreenshot)_webDriver).GetScreenshot();
        nameOfScreenshot = TestSetup.PageScreenshotsFolder + "\\" + nameOfScreenshot + ".png";
        ss.SaveAsFile(nameOfScreenshot);
      }
      catch (Exception ex)
      {
        Logger.Error(ex.Message);
      }
    }

    public void ScreenshotElement(IWebElement element, string nameOfScreenshot)
    {
      try
      {
        new WaitHelper(_webDriver).WaitForElementToBeDisplayed(element);
        var byteArray = ((ITakesScreenshot)_webDriver).GetScreenshot().AsByteArray;
        var screenshot = new Bitmap(new MemoryStream(byteArray));
        var croppedImage = new Rectangle(element.Location.X, element.Location.Y, element.Size.Width, element.Size.Height);
        screenshot = screenshot.Clone(croppedImage, screenshot.PixelFormat);
        nameOfScreenshot = TestSetup.PageScreenshotsFolder + "\\" + nameOfScreenshot + ".png";
        screenshot.Save(nameOfScreenshot, ImageFormat.Png);
      }
      catch(Exception ex)
      {
        Logger.Error(ex.StackTrace + ' ' + ex.Message);
      }     
    }
  }
}