using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using System;
using System.Net.Http;
using System.Reflection;
using Xunit;

namespace RazorPagesMovie.UITests
{
    // Define fixture for browser instance reuse
    [CollectionDefinition("Browser")]
    public class BrowserCollection : ICollectionFixture<WebDriverFixture>
    {
        // This class has no code, and is never created.
        // Its purpose is to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
    
    public class WebDriverFixture : IDisposable
    {
        private static IWebDriver _driver;
        private static readonly object _lock = new object();
        private bool _disposed = false;
        private static System.Net.Http.HttpClient _httpClient;
        
        public WebDriverFixture()
        {
            // Initialize shared HttpClient if needed
            if (_httpClient == null)
            {
                _httpClient = new System.Net.Http.HttpClient();
            }
        }
        
        public IWebDriver GetDriver()
        {
            // Return the current driver if it exists and is in a good state
            if (_driver != null)
            {
                try
                {
                    // Simple check to see if the driver is still valid
                    var windowHandles = _driver.WindowHandles;
                    return _driver;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"WebDriver appears to be in a bad state: {ex.Message}");
                    Console.WriteLine("Creating a new WebDriver instance...");
                    // If this fails, the driver might be in a bad state
                    // so we'll need to recreate it
                    DisposeDriverOnly();
                }
            }
            
            return CreateDriver();
        }
        
        public System.Net.Http.HttpClient GetHttpClient()
        {
            if (_httpClient == null)
            {
                _httpClient = new System.Net.Http.HttpClient();
            }
            else
            {
                // Check if HttpClient is disposed
                try
                {
                    // Try to access a property that would throw if disposed
                    var baseAddress = _httpClient.BaseAddress;
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("HttpClient was disposed, creating a new instance");
                    _httpClient = new System.Net.Http.HttpClient();
                }
            }
            return _httpClient;
        }
        
        private void DisposeDriverOnly()
        {
            if (_driver != null)
            {
                try
                {
                    _driver.Quit();
                    _driver.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error disposing driver: {ex.Message}");
                }
                finally
                {
                    _driver = null;
                }
            }
        }
        
        public IWebDriver CreateDriver()
        {
            if (_driver == null)
            {
                lock (_lock)
                {
                    if (_driver == null)
                    {
                        string browser = Environment.GetEnvironmentVariable("BROWSER") ?? "chrome";
                        var options = GetOptions(browser); // Centralized options

                        switch (browser.ToLower())
                        {
                            case "firefox":
                                _driver = new FirefoxDriver(options as FirefoxOptions);
                                break;

                            case "edge":
                                _driver = new EdgeDriver(options as EdgeOptions);
                                break;

                            case "chromium":
                            case "chrome":
                            default:
                                _driver = new ChromeDriver(options as ChromeOptions);
                                break;
                        }
                        
                        // Set timeouts
                        _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
                        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
                    }
                }
            }
            
            return _driver;
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
                    chromeOptions.AddArgument("--no-sandbox");
                    chromeOptions.AddArgument("--disable-dev-shm-usage");
                    chromeOptions.AddArgument("--disable-extensions");
                    chromeOptions.AddArgument("--disable-translate");
                    chromeOptions.AddArgument("--disable-sync");
                    chromeOptions.AddArgument("--disable-background-networking");
                    chromeOptions.AddArgument("--window-size=1280,1024");
                    return chromeOptions;
            }
        }
        
        public void Dispose()
        {
            if (!_disposed)
            {
                // Only dispose the driver when the fixture itself is being disposed
                // which should happen at the end of the collection
                if (_driver != null)
                {
                    try
                    {
                        _driver.Quit();
                        _driver.Dispose();
                        _driver = null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error disposing WebDriver: {ex.Message}");
                    }
                }
                
                // Dispose HttpClient too
                if (_httpClient != null)
                {
                    try
                    {
                        _httpClient.Dispose();
                        _httpClient = null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error disposing HttpClient: {ex.Message}");
                    }
                }
                
                _disposed = true;
            }
        }
    }
}