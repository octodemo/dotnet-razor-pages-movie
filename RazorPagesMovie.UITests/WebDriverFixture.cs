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
            chromeOptions.AddArgument("--headless"); // Optional: Run Chrome headlessly for CI

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