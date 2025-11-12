using System;
using NLog;

namespace S4Demo.Framework.Helpers
{
  public static class FrameworkLogger
  {
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    // ------------------------------
    // 🔹 TEST LIFECYCLE LOGGING
    // ------------------------------

    public static void LogTestStart(string testName, string browser, string environment)
    {
      Logger.Info($"\n----------------------------------------------------------");
      Logger.Info($"[TEST START] {testName}");
      Logger.Info($"    Browser       : {browser}");
      Logger.Info($"    Environment   : {environment}");
      Logger.Info($"    Start Time    : {DateTime.Now:HH:mm:ss.fff}");
      Logger.Info($"----------------------------------------------------------");
    }

    public static void LogTestPass(string testName, TimeSpan duration, int stepCount = 0)
    {
      Logger.Info($"----------------------------------------------------------");
      Logger.Info($"[TEST PASSED] {testName}");
      Logger.Info($"    Duration    : {duration:hh\\:mm\\:ss\\.fff}");
      if (stepCount > 0)
        Logger.Info($"    Total Steps : {stepCount}");
      Logger.Info($"----------------------------------------------------------\n");
    }

    public static void LogTestFail(string testName, Exception ex, TimeSpan duration)
    {
      Logger.Error($"----------------------------------------------------------");
      Logger.Error($"[TEST FAILED] {testName}");
      Logger.Error($"    Duration    : {duration:hh\\:mm\\:ss\\.fff}");
      Logger.Error($"    Exception   : {ex.GetType().Name}");
      Logger.Error($"    Message     : {ex.Message}");
      Logger.Error($"    StackTrace  : {ex.StackTrace}");
      Logger.Error($"----------------------------------------------------------\n");
    }

    // ------------------------------
    // 🔹 STEP / ACTION LOGGING
    // ------------------------------

    public static void LogStep(string message)
    {
      Logger.Info($"[STEP] {message}");
    }

    public static void LogWait(string message)
    {
      Logger.Info($"[WAIT] {message}");
    }

    public static void LogAssert(string message)
    {
      Logger.Info($"[ASSERT] {message}");
    }

    // ------------------------------
    // 🔹 ERROR / WARNING LOGGING
    // ------------------------------

    public static void LogError(string message, Exception? ex = null)
    {
      if (ex == null)
        Logger.Error($"[ERROR] {message}");
      else
        Logger.Error(ex, $"[ERROR] {message}");
    }

    public static void LogWarning(string message)
    {
      Logger.Warn($"[WARN] {message}");
    }

    public static void LogInfo(string message)
    {
      Logger.Info($"[INFO] {message}");
    }
  }
}