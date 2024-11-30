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

            switch (browser.ToLower())
            {
                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    firefoxOptions.AcceptInsecureCertificates = true;
                    firefoxOptions.AddArgument("--headless");
                    Driver = new FirefoxDriver(firefoxOptions);
                    break;

                case "edge":
                    var edgeOptions = new EdgeOptions();
                    edgeOptions.AcceptInsecureCertificates = true;
                    edgeOptions.AddArgument("--headless");
                    Driver = new EdgeDriver(edgeOptions);
                    break;

                case "chromium":
                    var chromiumOptions = new ChromeOptions();
                    chromiumOptions.AcceptInsecureCertificates = true;
                    chromiumOptions.AddArgument("--headless");
                    chromiumOptions.AddArgument("--no-sandbox");
                    Driver = new ChromeDriver(chromiumOptions);
                    break;

                case "chrome":
                default:
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AcceptInsecureCertificates = true;
                    chromeOptions.AddArgument("--headless");
                    chromeOptions.AddArgument("--no-sandbox");
                    Driver = new ChromeDriver(chromeOptions);
                    break;
            }

            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);
        }

        public void Dispose()
        {
            Driver?.Quit();
            Driver?.Dispose();
        }
    }
}