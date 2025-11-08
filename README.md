# Selenium4DemoProject

This project is a Selenium 4 demo test suite supporting **Chrome** and **Edge** only (Chrome headless supported for CI/Grid).
- Default browser: **Chrome**
- Supported modes: **chrome**, **chrome-headless**, **edge**
- Selenium Grid: supported (use chrome-headless on remote non-GUI nodes)

## Quick start
1. Open `Selenium4DemoProject.sln` in Visual Studio 2022 or later.
2. Update NuGet packages (Selenium.WebDriver 4.x, NLog, NUnit).
3. Place `chromedriver` and `msedgedriver` in the project `bin` folder or ensure they are in PATH.
4. Build solution and run tests via Test Explorer.
