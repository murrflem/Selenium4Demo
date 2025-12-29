# S4Demo Selenium Automation Framework

S4Demo is a C# Selenium 4 automation framework designed to **simplify automated UI testing** while remaining maintainable, readable, and extensible. It supports both **local execution** and **remote execution via Selenium Grid**, includes **helpers for common tasks**, and optionally supports **lightweight visual testing**.

---

## 🚀 Getting Started

1. Clone the repository and open it in **Visual Studio 2022**.
2. Restore NuGet packages.
3. Configure `appsettings.json` (see below for details).
4. Run your tests either **locally** or on **Selenium Grid**.

---

## ⚙️ Configuration (`appsettings.json`)

The framework behavior is controlled via `appsettings.json`:

```json
{
  "Environment": {...},
  "Browser": {...},
  "Timeouts": {...},
  "Screenshots": {...},
  "HttpSettings": {...},
  "FrameworkLogging": {...},
  "VisualComparison": {...}
}
```

### Environment
- `TestSiteUrl`: Base URL of the application under test.
- `RunOnGrid`: `false` for local, `true` for Selenium Grid.
- `GridHubIp` / `GridPort`: Selenium Grid connection details.

### Browser
- `TestedBrowser`: Browser to use (`chrome`, `firefox`, `edge`).
- `HighlightElements`: Highlight elements during interactions (good for debugging).

### Timeouts
- `DefaultTimeoutInSeconds`: Default wait for element or page conditions.
- `PollingIntervalMs`: Frequency of rechecking wait conditions.

### Screenshots
- `EnableScreenshots`: Global toggle for capturing screenshots (pages, elements, errors, visual comparison).

### HTTP Settings
- `TimeoutSeconds`: Max time to wait for HTTP requests.
- `RetryCount` / `RetryDelayMs`: Retry behavior for HTTP requests.
- `MaxExpectedResponseTimeMs`: Flag slow responses.
- `FilterByDomain`: Limit monitoring to your app’s domain.

### Logging
- `Mode`: Logging mode (`AllTests` by default).
- `MaxLogsPerTest`: Max log entries stored per test.

### Visual Comparison (Optional)
- `PixelTolerance`: Tolerance for per-pixel differences in images.
- `FailOnDifference`: Fail test if images differ (`false` = log warning only).

---

## 🏗 Framework Structure

### Setup Classes

- `TestSetup`:
  - Initializes WebDriver (local or grid).
  - Creates screenshot folders.
  - Sets browser size and waits.
  - Provides `SetupTest()` and `QuitDriver()`.

- `ConfigManager`:
  - Reads `appsettings.json`.
  - Provides typed access to configuration values.

### Helpers

- `ScreenshotHelper`:
  - Capture **page screenshots** and **element screenshots**.
  - Compare screenshots with optional visual diff highlighting.
  - Reset baselines for visual comparisons.

- `WaitHelper`:
  - Centralized wait methods to avoid using `Thread.Sleep()`.
  - Waits for elements to be visible, clickable, or removed.

---

## 📂 Folder Structure

```
S4Demo/
├─ Screenshots/
│  ├─ BaselineImages/     # Reference images for visual comparison
│  ├─ ImageDiffs/         # Highlighted visual differences
│  └─ ErrorScreenshots/   # Screenshots captured on test failure
├─ Tests/                 # Your test classes
├─ Framework/             # Helpers, Setup, and configuration classes
└─ appsettings.json       # Global configuration
```

---

## 🧪 Running Tests

### Local Execution
1. Set `RunOnGrid` to `false`.
2. Set `TestedBrowser` in `appsettings.json`.
3. Run the test project in Visual Studio.

### Selenium Grid Execution
1. Set `RunOnGrid` to `true`.
2. Ensure your grid hub is running.
3. Configure `GridHubIp` and `GridPort`.
4. Run the test project — WebDriver sessions will be created remotely.

---

## 📝 Test Results & Artifacts

- **Screenshots**:
  - Page screenshots are saved in `Screenshots/PageScreenshots`.
  - Failed test screenshots are saved in `Screenshots/ErrorScreenshots`.
  - Visual comparison diffs saved in `Screenshots/ImageDiffs`.

- **Logging**:
  - Framework logs are written according to the `FrameworkLogging` settings.
  - Each log includes timestamps, test step info, and errors.

---

## ⚡ Visual Testing

Visual comparisons are **optional**.

- Baseline images are automatically saved on the first run.
- Differences are highlighted in the diff image.
- Behavior is controlled by:
  - `PixelTolerance`
  - `FailOnDifference`

> ⚠️ This is **lightweight visual validation** and intended for occasional checks. For comprehensive visual regression testing, a dedicated tool should be used.

---

## ✅ Summary

- **Centralized configuration** via `appsettings.json`.
- **Local and remote execution** via Selenium Grid.
- **Helper classes** for waits, screenshots, and visual comparison.
- **Clear folder structure** for test artifacts.
- **Optional visual testing** with configurable tolerance and fail behavior.

---

## 💡 Tips

- Use `HighlightElements` during debugging to visualize interactions.
- Reset baselines using `ScreenshotHelper.ResetBaseline()` if the UI changes intentionally.
- Keep `EnableScreenshots` on during test development; you can disable it in CI if storage is limited.
- Use `PixelTolerance` to balance between minor rendering differences and actual UI changes.
```

## 🗂 Test Data Import

S4Demo supports using external test data from **CSV** or **Excel** files to drive your tests with different input values.

- Place your test data files in the `TestData` folder at the root of the project (e.g., `TestData/FlightData.xlsx` or `TestData/FlightData.csv`).  
- Each file should have headers that match the property names of your data model class.  
- The framework will load the data into typed objects, which can then be used directly in your tests.  

This enables **data-driven testing**, letting you maintain and update test data separately from your test logic.

