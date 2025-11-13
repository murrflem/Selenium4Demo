using System;
using System.IO;
using System.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;
using S4Demo.Framework.Setup;

namespace S4Demo.Framework.Helpers
{
  public static class FrameworkLogger
  {
    private static Logger _logger;

    static FrameworkLogger()
    {
      InitializeLogger();
    }

    // ================================================================
    // 🔹 Logger Initialization
    // ================================================================
    private static void InitializeLogger()
    {
      string mode = ConfigManager.LoggingMode ?? "AllTests";
      string logsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
      Directory.CreateDirectory(logsFolder);

      string fileName;
      string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

      if (mode.Equals("PerTest", StringComparison.OrdinalIgnoreCase))
      {
        var testName = TestContext.CurrentContext?.Test?.Name ?? "UnknownTest";

        // Cleanup is now handled externally (per test)
        fileName = Path.Combine(logsFolder, $"{testName}_{timestamp}.log");
      }
      else
      {
        // Shared log file for all tests
        fileName = Path.Combine(logsFolder, $"FrameworkLog_{DateTime.Now:yyyyMMdd}.log");
      }

      var config = new LoggingConfiguration();

      var fileTarget = new FileTarget("logfile")
      {
        FileName = fileName,
        Layout = "${longdate}|${level:uppercase=true}|${logger}|${message}"
      };

      var consoleTarget = new ConsoleTarget("logconsole")
      {
        Layout = "${longdate}|${level:uppercase=true}|${message}"
      };

      config.AddTarget(fileTarget);
      config.AddTarget(consoleTarget);

      config.AddRule(LogLevel.Info, LogLevel.Fatal, fileTarget);
      config.AddRule(LogLevel.Info, LogLevel.Fatal, consoleTarget);

      LogManager.Configuration = config;

      _logger = LogManager.GetCurrentClassLogger();
      _logger.Info($"[INIT] Logging mode: {mode}, Log file: {Path.GetFileName(fileName)}");
    }

    // ================================================================
    // 🔹 Per-Test Cleanup (called manually each test)
    // ================================================================
    public static void RunLogCleanup()
    {
      try
      {
        if (ConfigManager.LoggingMode.Equals("PerTest", StringComparison.OrdinalIgnoreCase))
        {
          string logsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
          string testName = TestContext.CurrentContext?.Test?.Name ?? "UnknownTest";
          int keepLatest = ConfigManager.MaxLogsPerTest;

          CleanupOldTestLogs(logsFolder, testName, keepLatest);
        }
      }
      catch (Exception ex)
      {
        LogManager.GetCurrentClassLogger().Warn($"[CLEANUP] Error running per-test cleanup: {ex.Message}");
      }
    }

    // ================================================================
    // 🔹 Log Cleanup Helper
    // ================================================================
    private static void CleanupOldTestLogs(string folder, string testName, int keepLatest)
    {
      try
      {
        var pattern = $"{testName}_*.log";
        var files = new DirectoryInfo(folder)
            .GetFiles(pattern)
            .OrderByDescending(f => f.CreationTimeUtc)
            .ToList();

        if (files.Count > keepLatest)
        {
          foreach (var file in files.Skip(keepLatest))
          {
            try
            {
              file.Delete();
              LogManager.GetCurrentClassLogger().Info($"[CLEANUP] Deleted old log: {file.Name}");
            }
            catch (Exception ex)
            {
              LogManager.GetCurrentClassLogger().Warn($"[CLEANUP] Could not delete log {file.Name}: {ex.Message}");
            }
          }
        }
      }
      catch (Exception ex)
      {
        LogManager.GetCurrentClassLogger().Warn($"[CLEANUP] Error during log cleanup: {ex.Message}");
      }
    }

    // ================================================================
    // 🔹 Test Lifecycle Logging
    // ================================================================
    public static void LogTestStart(string testName, string browser, string environment)
    {
      _logger.Info($"\n----------------------------------------------------------");
      _logger.Info($"[TEST START] {testName}");
      _logger.Info($"    Browser       : {browser}");
      _logger.Info($"    Environment   : {environment}");
      _logger.Info($"    Start Time    : {DateTime.Now:HH:mm:ss.fff}");
      _logger.Info($"----------------------------------------------------------");
    }

    public static void LogTestPass(string testName, TimeSpan duration, int stepCount = 0)
    {
      _logger.Info($"----------------------------------------------------------");
      _logger.Info($"[TEST PASSED] {testName}");
      _logger.Info($"    Duration    : {duration:hh\\:mm\\:ss\\.fff}");
      if (stepCount > 0)
        _logger.Info($"    Total Steps : {stepCount}");
      _logger.Info($"----------------------------------------------------------\n");
    }

    public static void LogTestFail(string testName, Exception ex, TimeSpan duration)
    {
      _logger.Error($"----------------------------------------------------------");
      _logger.Error($"[TEST FAILED] {testName}");
      _logger.Error($"    Duration    : {duration:hh\\:mm\\:ss\\.fff}");
      _logger.Error($"    Exception   : {ex.GetType().Name}");
      _logger.Error($"    Message     : {ex.Message}");
      _logger.Error($"    StackTrace  : {ex.StackTrace}");
      _logger.Error($"----------------------------------------------------------\n");
    }

    // ================================================================
    // 🔹 Step / Action Logging
    // ================================================================
    public static void LogStep(string message)
    {
      _logger.Info($"[STEP] {message}");
    }

    public static void LogWait(string message)
    {
      _logger.Info($"[WAIT] {message}");
    }

    public static void LogAssert(string message)
    {
      _logger.Info($"[ASSERT] {message}");
    }

    // ================================================================
    // 🔹 Error / Warning / Info Logging
    // ================================================================
    public static void LogError(string message, Exception? ex = null)
    {
      if (ex == null)
        _logger.Error($"[ERROR] {message}");
      else
        _logger.Error(ex, $"[ERROR] {message}");
    }

    public static void LogWarning(string message)
    {
      _logger.Warn($"[WARN] {message}");
    }

    public static void LogInfo(string message)
    {
      _logger.Info($"[INFO] {message}");
    }
  }
}