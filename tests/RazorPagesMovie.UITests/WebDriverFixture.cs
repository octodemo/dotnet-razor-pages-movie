using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using System;

namespace RazorPagesMovie.UITests
{
    public class WebDriverFixture : IDisposable
    {
        public IWebDriver Driver { get; }

        public WebDriverFixture()
        {
            string browser = Environment.GetEnvironmentVariable("BROWSER") ?? "chrome";
            var options = GetOptions(browser); // Centralized options

            switch (browser.ToLower())
            {
                case "firefox":
                    Driver = new FirefoxDriver(options as FirefoxOptions);
                    break;

                case "edge":
                    Driver = new EdgeDriver(options as EdgeOptions);
                    break;

                case "chromium":
                case "chrome":
                default:
                    Driver = new ChromeDriver(options as ChromeOptions);
                    break;
            }

            // Remove implicit wait - rely on explicit waits in tests
            // Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4); 
        }

        private DriverOptions GetOptions(string browser)
        {
            switch (browser.ToLower())
            {
                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    firefoxOptions.AcceptInsecureCertificates = true;
                    firefoxOptions.AddArgument("--headless");
                    return firefoxOptions;

                case "edge":
                    var edgeOptions = new EdgeOptions();
                    edgeOptions.AcceptInsecureCertificates = true;
                    edgeOptions.AddArgument("--headless");
                    return edgeOptions;

                case "chromium":
                case "chrome":
                default:
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AcceptInsecureCertificates = true;
                    chromeOptions.AddArgument("--headless");
                    chromeOptions.AddArgument("--no-sandbox"); // Added for consistency
                    return chromeOptions;
            }
        }

        public void Dispose()
        {
            Driver?.Quit();
            Driver?.Dispose();
        }
    }
}