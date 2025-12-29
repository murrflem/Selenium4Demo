using OpenQA.Selenium;
using S4Demo.Framework.Setup;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace S4Demo.Framework.Helpers
{
  public class ScreenshotHelper
  {
    private readonly IWebDriver _webDriver;

    public ScreenshotHelper(IWebDriver webDriver)
    {
      _webDriver = webDriver ?? throw new ArgumentNullException(nameof(webDriver));
    }

    public string ScreenshotPage()
    {
      return ScreenshotPage(_webDriver.Title);
    }

    public string ScreenshotPage(string screenshotName)
    {
      if (!ConfigManager.EnableScreenshots)
      {
        FrameworkLogger.LogInfo("Screenshots are disabled in configuration.");
        return string.Empty;
      }

      try
      {
        var safeName = MakeSafeFileName(screenshotName);
        var folder = TestSetup.PageScreenshotsFolder;
        Directory.CreateDirectory(folder);

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var filePath = Path.Combine(folder, $"{safeName}_{timestamp}.png");

        var screenshot = ((ITakesScreenshot)_webDriver).GetScreenshot();
        screenshot.SaveAsFile(filePath);

        FrameworkLogger.LogInfo($"Screenshot saved: {filePath}");
        return filePath;
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError($"Failed to capture page screenshot: {screenshotName}", ex);
        return string.Empty;
      }
    }

    public void ComparePage(string screenshotName = null, byte? tolerance = null)
    {
      if (!ConfigManager.EnableScreenshots)
      {
        FrameworkLogger.LogInfo("Screenshots are disabled in configuration.");
        return;
      }

      try
      {
        byte actualTolerance = tolerance ?? (byte)ConfigManager.VisualComparisonTolerance;

        screenshotName ??= _webDriver.Title;
        var safeName = MakeSafeFileName(screenshotName);

        // Baseline folder
        var baselineFolder = Path.Combine(TestSetup.BaseFolder, "BaselineImages");
        Directory.CreateDirectory(baselineFolder);

        var baselineFile = Path.Combine(baselineFolder, $"{safeName}.png");

        // Create baseline if missing
        if (!File.Exists(baselineFile))
        {
          var screenshot = ((ITakesScreenshot)_webDriver).GetScreenshot();
          screenshot.SaveAsFile(baselineFile);
          FrameworkLogger.LogInfo($"No existing baseline image found. Saved new baseline: {baselineFile}");
          return;
        }

        // Capture current screenshot
        var tempFile = Path.Combine(baselineFolder, $"{safeName}_Current.png");
        var currentScreenshot = ((ITakesScreenshot)_webDriver).GetScreenshot();
        currentScreenshot.SaveAsFile(tempFile);

        bool areEqual = true;

        using (var img1 = new Bitmap(baselineFile))
        using (var img2 = new Bitmap(tempFile))
        {
          if (img1.Width != img2.Width || img1.Height != img2.Height)
          {
            areEqual = false;
          }
          else
          {
            var rect = new Rectangle(0, 0, img1.Width, img1.Height);
            var bmpData1 = img1.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var bmpData2 = img2.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            int bytes = Math.Abs(bmpData1.Stride) * img1.Height;
            byte[] rgb1 = new byte[bytes];
            byte[] rgb2 = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(bmpData1.Scan0, rgb1, 0, bytes);
            System.Runtime.InteropServices.Marshal.Copy(bmpData2.Scan0, rgb2, 0, bytes);

            for (int i = 0; i < bytes; i += 4)
            {
              if (Math.Abs(rgb1[i + 2] - rgb2[i + 2]) > actualTolerance || // R
                  Math.Abs(rgb1[i + 1] - rgb2[i + 1]) > actualTolerance || // G
                  Math.Abs(rgb1[i] - rgb2[i]) > actualTolerance)   // B
              {
                areEqual = false;

                // Highlight difference (semi-transparent blue)
                rgb2[i] = 255;
                rgb2[i + 1] = 0;
                rgb2[i + 2] = 0;
                rgb2[i + 3] = 128;
              }
            }

            System.Runtime.InteropServices.Marshal.Copy(rgb2, 0, bmpData2.Scan0, bytes);
            img1.UnlockBits(bmpData1);
            img2.UnlockBits(bmpData2);
          }

          if (!areEqual)
          {
            var diffsFolder = Path.Combine(TestSetup.BaseFolder, "ImageDiffs");
            Directory.CreateDirectory(diffsFolder);

            var diffPath = Path.Combine(diffsFolder, $"{safeName}_Diff.png");
            img2.Save(diffPath, ImageFormat.Png);

            var message =
              $"Visual differences detected for '{screenshotName}' " +
              $"(tolerance={actualTolerance}). Diff saved to: {diffPath}";

            if (ConfigManager.FailOnVisualDifference)
            {
              FrameworkLogger.LogError(message);
              throw new Exception(message);
            }

            FrameworkLogger.LogWarning(message);
          }
        }

        if (File.Exists(tempFile))
          File.Delete(tempFile);

        if (areEqual)
        {
          FrameworkLogger.LogInfo(
            $"Compared screenshots: {baselineFile} vs current screenshot. No differences found (tolerance={actualTolerance}).");
        }
      }
      catch (Exception ex)
      {
        // Avoid double-logging expected visual comparison failures
        if (!ConfigManager.FailOnVisualDifference)
        {
          FrameworkLogger.LogError("Failed to compare page screenshots.", ex);
        }

        throw;
      }
    }

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

        FrameworkLogger.LogInfo($"Element screenshot saved: {filePath}");
        return filePath;
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError($"Failed to capture element screenshot: {screenshotName}", ex);
        return string.Empty;
      }
    }

    /// <summary>
    /// Deletes the baseline image for the specified page so that a new baseline
    /// will be created on the next ComparePage() call.
    /// </summary>
    public void ResetBaseline(string screenshotName = null)
    {
      try
      {
        screenshotName ??= _webDriver.Title;
        var safeName = MakeSafeFileName(screenshotName);

        var baselineFolder = Path.Combine(TestSetup.BaseFolder, "BaselineImages");
        Directory.CreateDirectory(baselineFolder); // Ensure folder exists

        var baselineFile = Path.Combine(baselineFolder, $"{safeName}.png");

        if (File.Exists(baselineFile))
        {
          File.Delete(baselineFile);
          FrameworkLogger.LogInfo($"Baseline image deleted for '{screenshotName}': {baselineFile}. A new baseline will be created on next ComparePage() call.");
        }
        else
        {
          FrameworkLogger.LogInfo($"No baseline image found to delete for '{screenshotName}'.");
        }
      }
      catch (Exception ex)
      {
        FrameworkLogger.LogError($"Failed to reset baseline image for '{screenshotName}'.", ex);
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