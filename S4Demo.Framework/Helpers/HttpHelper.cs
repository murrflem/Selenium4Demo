using System;
using System.Diagnostics;
using S4Demo.Framework.Setup;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace S4Demo.Framework.Helpers
{
  /// <summary>
  /// Provides helper methods to check HTTP response status and response time for URLs.
  /// </summary>
  public static class HttpHelper
  {
    private static readonly HttpClient _httpClient = new HttpClient();

    static HttpHelper()
    {
      _httpClient.Timeout = TimeSpan.FromSeconds(ConfigManager.HttpTimeoutSeconds);
    }

    /// <summary>
    /// Sends an HTTP GET request to the specified URL and returns the status code and response time.
    /// Includes structured logging for start and finish events.
    /// </summary>
    public static async Task<HttpCheckResult> CheckUrlStatusAsync(string url)
    {
      if (string.IsNullOrWhiteSpace(url))
        throw new ArgumentException("URL cannot be null or empty.", nameof(url));

      FrameworkLogger.LogStep($"Starting HTTP check for: {url}");

      var stopwatch = new Stopwatch();
      var result = new HttpCheckResult();

      try
      {
        stopwatch.Start();

        using (var response = await _httpClient.GetAsync(url))
        {
          stopwatch.Stop();
          result.StatusCode = response.StatusCode;
          result.ResponseTimeMs = stopwatch.ElapsedMilliseconds;

          FrameworkLogger.LogInfo(
            $"HTTP check completed for {url} | Status: {(int)result.StatusCode} {result.StatusCode}, " +
            $"Response time: {result.ResponseTimeMs} ms");
        }
      }
      catch (HttpRequestException ex)
      {
        stopwatch.Stop();
        result.StatusCode = HttpStatusCode.ServiceUnavailable;
        result.ResponseTimeMs = stopwatch.ElapsedMilliseconds;

        FrameworkLogger.LogError($"HTTP check failed for {url}", ex);
      }
      catch (TaskCanceledException)
      {
        stopwatch.Stop();
        result.StatusCode = HttpStatusCode.RequestTimeout;
        result.ResponseTimeMs = stopwatch.ElapsedMilliseconds;

        FrameworkLogger.LogWarning($"HTTP check timed out for {url}");
      }

      FrameworkLogger.LogStep($"Finished HTTP check for: {url}");
      return result;
    }

    /// <summary>
    /// Synchronous wrapper for compatibility with non-async test methods.
    /// </summary>
    public static HttpCheckResult CheckUrlStatus(string url)
    {
      return CheckUrlStatusAsync(url).GetAwaiter().GetResult();
    }
  }

  /// <summary>
  /// Represents the result of an HTTP status check.
  /// </summary>
  public class HttpCheckResult
  {
    public HttpStatusCode StatusCode { get; set; }
    public long ResponseTimeMs { get; set; }
  }
}