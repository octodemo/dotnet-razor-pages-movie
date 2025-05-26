using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using System;

namespace RazorPagesMovie.UITests
{
    public class WebDriverFixture
    {
        public IWebDriver CreateDriver()
        {
            string browser = Environment.GetEnvironmentVariable("BROWSER") ?? "chrome";
            var options = GetOptions(browser); // Centralized options

            switch (browser.ToLower())
            {
                case "firefox":
                    return new FirefoxDriver(options as FirefoxOptions);

                case "edge":
                    return new EdgeDriver(options as EdgeOptions);

                case "chromium":
                case "chrome":
                default:
                    return new ChromeDriver(options as ChromeOptions);
            }
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
    }
}