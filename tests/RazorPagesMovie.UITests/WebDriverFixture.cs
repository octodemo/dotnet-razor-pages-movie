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
        private static int _instanceCount = 0;
        private static readonly object _httpClientLock = new object();
        
        public WebDriverFixture()
        {
            lock (_httpClientLock)
            {
                _instanceCount++;
                Console.WriteLine($"WebDriverFixture instance created. Count: {_instanceCount}");
                
                // Initialize HttpClient with proper configuration for CI/CD environments
                if (_httpClient == null || IsHttpClientDisposed(_httpClient))
                {
                    if (_httpClient != null)
                    {
                        Console.WriteLine("HttpClient was disposed, creating a new instance");
                    }
                    
                    _httpClient = CreateHttpClient();
                }
            }
        }
        
        private static System.Net.Http.HttpClient CreateHttpClient()
        {
            var httpClient = new System.Net.Http.HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30); // Set reasonable timeout
            
            // Add headers that might be needed for Azure Container Apps
            httpClient.DefaultRequestHeaders.Add("User-Agent", "RazorPagesMovie-UITests/1.0");
            
            Console.WriteLine("Created new HttpClient instance");
            return httpClient;
        }
        
        private static bool IsHttpClientDisposed(System.Net.Http.HttpClient client)
        {
            try
            {
                // Try to access a property that would throw if disposed
                var timeout = client.Timeout;
                return false;
            }
            catch (ObjectDisposedException)
            {
                return true;
            }
            catch (Exception)
            {
                // Other exceptions mean it's not disposed
                return false;
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
            lock (_httpClientLock)
            {
                if (_httpClient == null || IsHttpClientDisposed(_httpClient))
                {
                    _httpClient = CreateHttpClient();
                }
                return _httpClient;
            }
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
            bool isCI = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"));
            Console.WriteLine($"Running in CI environment: {isCI}");
            
            switch (browser.ToLower())
            {
                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    firefoxOptions.AcceptInsecureCertificates = true;
                    firefoxOptions.AddArgument("--headless");
                    if (isCI)
                    {
                        firefoxOptions.AddArgument("--no-sandbox");
                        firefoxOptions.AddArgument("--disable-gpu");
                    }
                    return firefoxOptions;

                case "edge":
                    var edgeOptions = new EdgeOptions();
                    edgeOptions.AcceptInsecureCertificates = true;
                    edgeOptions.AddArgument("--headless");
                    if (isCI)
                    {
                        edgeOptions.AddArgument("--no-sandbox");
                        edgeOptions.AddArgument("--disable-gpu");
                        edgeOptions.AddArgument("--disable-dev-shm-usage");
                    }
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
                    
                    if (isCI)
                    {
                        // Additional options for CI environments
                        chromeOptions.AddArgument("--disable-gpu");
                        chromeOptions.AddArgument("--disable-background-timer-throttling");
                        chromeOptions.AddArgument("--disable-backgrounding-occluded-windows");
                        chromeOptions.AddArgument("--disable-renderer-backgrounding");
                        chromeOptions.AddArgument("--disable-features=TranslateUI");
                        chromeOptions.AddArgument("--disable-ipc-flooding-protection");
                        chromeOptions.AddArgument("--no-first-run");
                        chromeOptions.AddArgument("--disable-default-apps");
                        chromeOptions.AddArgument("--disable-component-extensions-with-background-pages");
                    }
                    
                    return chromeOptions;
            }
        }
        
        public void Dispose()
        {
            if (!_disposed)
            {
                lock (_httpClientLock)
                {
                    _instanceCount--;
                    Console.WriteLine($"WebDriverFixture disposing. Remaining instances: {_instanceCount}");
                    
                    // Only dispose resources when this is the last instance
                    if (_instanceCount <= 0)
                    {
                        Console.WriteLine("Last WebDriverFixture instance - disposing shared resources");
                        
                        // Dispose WebDriver
                        if (_driver != null)
                        {
                            try
                            {
                                _driver.Quit();
                                _driver.Dispose();
                                _driver = null;
                                Console.WriteLine("WebDriver disposed");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error disposing WebDriver: {ex.Message}");
                            }
                        }
                        
                        // Dispose HttpClient
                        if (_httpClient != null)
                        {
                            try
                            {
                                _httpClient.Dispose();
                                _httpClient = null;
                                Console.WriteLine("HttpClient disposed");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error disposing HttpClient: {ex.Message}");
                            }
                        }
                        
                        // Reset instance count to prevent negative values
                        _instanceCount = 0;
                    }
                }
                
                _disposed = true;
            }
        }
    }
}