using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace RazorPagesMovie.Tests.UITests
{
    public class WebDriverFixture : IDisposable
    {
        public IWebDriver Driver { get; }

        public WebDriverFixture()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AcceptInsecureCertificates = true; // Accept self-signed certificates
            chromeOptions.AddArgument("--headless"); // Optional: Run Chrome headlessly for CI
            chromeOptions.AddArgument("--no-sandbox"); // Optional: Bypass any OS security checks
            chromeOptions.AddArgument("--disable-dev-shm-usage"); // Optional: Overcome limited resource problems
            chromeOptions.AddArgument("--disable-gpu"); // Optional: Disable GPU
            chromeOptions.AddArgument("--window-size=1920,1080"); // Optional: Set window size
            Driver = new ChromeDriver(chromeOptions);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4); // Set implicit wait
        }

        public void Dispose()
        {
            Driver?.Quit();
            Driver?.Dispose();
        }
    }
}