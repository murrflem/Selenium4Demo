using OpenQA.Selenium;
using S4Demo.Framework.Helpers;
using S4Demo.Framework.Setup;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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

    public void VerifyPageLinks()
    {
      // STEP 1: Get all <a> elements on the current page
      // Selenium.GetAllPageLinks() should return a list of IWebElement objects representing links
      var allLinks = Selenium.GetAllPageLinks();


      // STEP 2: Extract the href attribute from each link and filter only valid HTTP/HTTPS URLs
      // - Skip null or empty hrefs
      // - Skip javascript:void(0), #anchors, mailto:, tel:, etc.
      // - Keep only links starting with http:// or https://
      // - Distinct() removes duplicates so the same link isn't checked multiple times
      var validUrls = allLinks
          .Select(l => l.GetAttribute("href"))
          .Where(url =>
              !string.IsNullOrEmpty(url) &&
              (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
               url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
          .Distinct()
          .ToList();

      // Apply domain filtering BEFORE performing any HTTP checks
      if (ConfigManager.HttpFilterByDomain)
      {
        string baseDomain = new Uri(ConfigManager.TestSiteUrl).Host.ToLowerInvariant();

        validUrls = validUrls
            .Where(url => new Uri(url).Host.ToLowerInvariant() == baseDomain)
            .ToList();
      }

      // STEP 3: Prepare a list to collect any failures found during link verification
      var failures = new List<string>();

      // STEP 4: Loop through each valid URL and check its HTTP status + response time
      foreach (string urlToCheck in validUrls)
      {
        var result = HttpHelper.CheckUrlStatusSync(urlToCheck);

        if (result.StatusCode != HttpStatusCode.OK)
        {
          failures.Add($"{urlToCheck} returned {result.StatusCode}");
        }

        if (result.ResponseTimeMs > ConfigManager.MaxExpectedResponseTimeMs)
        {
          failures.Add($"{urlToCheck} took {result.ResponseTimeMs}ms (too slow)");
        }
      }

      // STEP 5: After checking all links, assert that no failures were found
      // - If failures exist, the test fails and prints all broken/slow links
      // - If no failures, the test passes
      if (failures.Any())
      {
        FrameworkLogger.LogError("Broken links detected:\n" + string.Join("\n", failures));
        throw new Exception("Broken links detected:\n" + string.Join("\n", failures));
      }
    }

    public void VerifyPageImages()
    {
      // 1. Collect all <img> elements
      var allImages = Selenium.GetAllPageImages();

      // 2. Extract valid src URLs
      var validImageUrls = allImages
          .Select(img => img.GetAttribute("src"))
          .Where(src =>
              !string.IsNullOrEmpty(src) &&
              (src.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
               src.StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
          .Distinct()
          .ToList();

      // 3. Loop through the clean list
      foreach (string urlToCheck in validImageUrls)
      {
        var result = HttpHelper.CheckUrlStatusSync(urlToCheck);
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
      }
    }
  }
}