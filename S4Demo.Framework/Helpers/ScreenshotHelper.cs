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
      _webDriver = webDriver ?? throw new ArgumentNullException(nameof(webDriver));
    }

    /// <summary>
    /// Captures a screenshot of the current page using the page title as the file name.
    /// </summary>
    public string ScreenshotPage()
    {
      return ScreenshotPage(_webDriver.Title);
    }

    /// <summary>
    /// Captures a screenshot of the current page using the specified name.
    /// </summary>
    public string ScreenshotPage(string screenshotName)
    {
      if (!TestSettings.EnableScreenshots)
        return string.Empty;

      try
      {
        var safeName = MakeSafeFileName(screenshotName);
        var folder = TestSetup.PageScreenshotsFolder;
        Directory.CreateDirectory(folder);

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var filePath = Path.Combine(folder, $"{safeName}_{timestamp}.png");

        var screenshot = ((ITakesScreenshot)_webDriver).GetScreenshot();
        screenshot.SaveAsFile(filePath);

        Logger.Info($"Screenshot saved: {filePath}");
        return filePath;
      }
      catch (Exception ex)
      {
        Logger.Error(ex, $"Failed to capture page screenshot: {screenshotName}");
        return string.Empty;
      }
    }

    /// <summary>
    /// Captures a screenshot of a specific element.
    /// </summary>
    public string ScreenshotElement(IWebElement element, string screenshotName)
    {
      if (element == null)
        throw new ArgumentNullException(nameof(element));

      try
      {
        new WaitHelper(_webDriver).WaitForElementToBeDisplayed(element);

        var byteArray = ((ITakesScreenshot)_webDriver).GetScreenshot().AsByteArray;
        using var screenshot = new Bitmap(new MemoryStream(byteArray));

        var cropArea = new Rectangle(element.Location.X, element.Location.Y, element.Size.Width, element.Size.Height);
        using var elementImage = screenshot.Clone(cropArea, screenshot.PixelFormat);

        var safeName = MakeSafeFileName(screenshotName);
        var folder = TestSetup.PageScreenshotsFolder;
        Directory.CreateDirectory(folder);

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var filePath = Path.Combine(folder, $"{safeName}_{timestamp}.png");

        elementImage.Save(filePath, ImageFormat.Png);

        Logger.Info($"Element screenshot saved: {filePath}");
        return filePath;
      }
      catch (Exception ex)
      {
        Logger.Error(ex, $"Failed to capture element screenshot: {screenshotName}");
        return string.Empty;
      }
    }

    private static string MakeSafeFileName(string name)
    {
      foreach (var c in Path.GetInvalidFileNameChars())
        name = name.Replace(c, '_');
      return string.IsNullOrWhiteSpace(name) ? "Screenshot" : name.Trim();
    }
  }
}