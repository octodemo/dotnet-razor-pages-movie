using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading.Tasks;

namespace RazorPagesMovie.UITests
{
    public class LoginUITests : IClassFixture<WebDriverFixture>
    {
        private readonly IWebDriver _driver;
        private readonly string _url;
        private readonly string _baseUrl;
        private const string DEFAULT_HOST = "https://localhost";
        private const int DEFAULT_PORT = 5001;
        private const string LOGIN_PATH = "/Account/Login";
        private static bool _hasLoggedBaseUrl = false;

        public LoginUITests(WebDriverFixture fixture)
        {
            _driver = fixture.Driver;
            _baseUrl = Environment.GetEnvironmentVariable("BASE_URL") ?? $"{DEFAULT_HOST}:{DEFAULT_PORT}";
            _url = $"{_baseUrl.TrimEnd('/')}{LOGIN_PATH}";
            
            if (!_hasLoggedBaseUrl)
            {
                Console.WriteLine($"Using base URL: {_baseUrl}");
                _hasLoggedBaseUrl = true;
            }
        }

        [Fact]
        public async Task Login_WithValidCredentials_ShouldRedirectToHomePage()
        {
            await _driver.Navigate().GoToUrlAsync(_url);

            var usernameField = _driver.FindElement(By.Name("LoginInput.Username"));
            var passwordField = _driver.FindElement(By.Name("LoginInput.Password"));
            var loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

            usernameField.SendKeys("user");
            passwordField.SendKeys("password");
            loginButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var expectedUrl = $"{_baseUrl}/Movies";
            wait.Until(d => d.Url == expectedUrl);
            
            Assert.Equal(expectedUrl, _driver.Url);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ShouldShowErrorMessage()
        {
            await _driver.Navigate().GoToUrlAsync(_url); // Added await
            _driver.Navigate().GoToUrl(_url);

            var usernameField = _driver.FindElement(By.Name("LoginInput.Username"));
            var passwordField = _driver.FindElement(By.Name("LoginInput.Password"));
            var loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

            usernameField.SendKeys("invalidUsername");
            passwordField.SendKeys("invalidPassword");
            loginButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.text-danger.text-center.mt-3")));

            var errorMessage = _driver.FindElement(By.CssSelector("div.text-danger.text-center.mt-3"));
            Assert.NotNull(errorMessage);
            // Update the expected message to match the actual message
            Assert.Equal("Invalid username or password", errorMessage.Text);
        }

        [Fact]
        public async Task Login_WithEmptyPassword_ShouldShowErrorMessage()
        {
            await _driver.Navigate().GoToUrlAsync(_url); // Added await
            _driver.Navigate().GoToUrl(_url);

            var usernameField = _driver.FindElement(By.Name("LoginInput.Username"));
            var passwordField = _driver.FindElement(By.Name("LoginInput.Password"));
            var loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

            usernameField.SendKeys("validUsername");
            passwordField.SendKeys(""); // Empty password
            loginButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("span.text-danger.field-validation-error[data-valmsg-for='LoginInput.Password']")));

            var errorMessage = _driver.FindElement(By.CssSelector("span.text-danger.field-validation-error[data-valmsg-for='LoginInput.Password']"));
            Assert.NotNull(errorMessage);
            Assert.Equal("The Password field is required.", errorMessage.Text);
        }

        [Fact]
        public async Task Login_WithEmptyUsername_ShouldShowErrorMessage()
        {
            await _driver.Navigate().GoToUrlAsync(_url); // Added await
            _driver.Navigate().GoToUrl(_url);

            var usernameField = _driver.FindElement(By.Name("LoginInput.Username"));
            var passwordField = _driver.FindElement(By.Name("LoginInput.Password"));
            var loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

            usernameField.SendKeys(""); // Empty username
            passwordField.SendKeys("password");
            loginButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("span.text-danger.field-validation-error[data-valmsg-for='LoginInput.Username']")));

            var errorMessage = _driver.FindElement(By.CssSelector("span.text-danger.field-validation-error[data-valmsg-for='LoginInput.Username']"));
            Assert.NotNull(errorMessage);
            Assert.Equal("The Username field is required.", errorMessage.Text);
        }

        [Fact]
        public async Task Login_WithEmptyUsernameAndPassword_ShouldShowErrorMessages()
        {
            await _driver.Navigate().GoToUrlAsync(_url); // Added await
            _driver.Navigate().GoToUrl(_url);

            var usernameField = _driver.FindElement(By.Name("LoginInput.Username"));
            var passwordField = _driver.FindElement(By.Name("LoginInput.Password"));
            var loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

            usernameField.SendKeys(""); // Empty username
            passwordField.SendKeys(""); // Empty password
            loginButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("span.text-danger.field-validation-error[data-valmsg-for='LoginInput.Username']")));

            var usernameError = _driver.FindElement(By.CssSelector("span.text-danger.field-validation-error[data-valmsg-for='LoginInput.Username']"));
            var passwordError = _driver.FindElement(By.CssSelector("span.text-danger.field-validation-error[data-valmsg-for='LoginInput.Password']"));

            Assert.NotNull(usernameError);
            Assert.Equal("The Username field is required.", usernameError.Text);

            Assert.NotNull(passwordError);
            Assert.Equal("The Password field is required.", passwordError.Text);
        }

        [Fact]
        public async Task Login_WithWhitespaceUsername_ShouldShowErrorMessage()
        {
            await _driver.Navigate().GoToUrlAsync(_url); // Added await
            _driver.Navigate().GoToUrl(_url);

            var usernameField = _driver.FindElement(By.Name("LoginInput.Username"));
            var passwordField = _driver.FindElement(By.Name("LoginInput.Password"));
            var loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

            usernameField.SendKeys("   "); // Whitespace username
            passwordField.SendKeys("password");
            loginButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("span.text-danger.field-validation-error[data-valmsg-for='LoginInput.Username']")));

            var errorMessage = _driver.FindElement(By.CssSelector("span.text-danger.field-validation-error[data-valmsg-for='LoginInput.Username']"));
            Assert.NotNull(errorMessage);
            Assert.Equal("The Username field is required.", errorMessage.Text);
        }

        [Fact]
        public async Task Admin_Can_See_Edit_Delete_Others_Cannot()
        {
            await _driver.Navigate().GoToUrlAsync(_url); // Added await
            // Login as admin user
            _driver.Navigate().GoToUrl(_url);

            var usernameField = _driver.FindElement(By.Name("LoginInput.Username"));
            var passwordField = _driver.FindElement(By.Name("LoginInput.Password"));
            var loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

            usernameField.SendKeys("admin"); // Use actual admin username
            passwordField.SendKeys("password"); // Use actual admin password
            loginButton.Click();

            // Wait for the Movies page to load
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(1));
            wait.Until(d => d.Url.Contains("/Movies"));

            Assert.Contains("/Movies", _driver.Url);

            // Click on a Movie card if necessary
            var movieCard = _driver.FindElement(By.CssSelector(".movie-card"));
            movieCard.Click();

            // Wait for the links to become visible
            wait.Until(ExpectedConditions.ElementIsVisible(By.LinkText("Edit")));

            // Verify Edit and Delete links are visible
            var editLinks = _driver.FindElements(By.LinkText("Edit"));
            var deleteLinks = _driver.FindElements(By.LinkText("Delete"));

            Assert.True(editLinks.Count > 0, "Admin user should see Edit links.");
            Assert.True(deleteLinks.Count > 0, "Admin user should see Delete links.");

            // Log out
            var logoutLink = _driver.FindElement(By.LinkText("Logout"));
            logoutLink.Click();

            // Login as regular user
            _driver.Navigate().GoToUrl(_url);

            usernameField = _driver.FindElement(By.Name("LoginInput.Username"));
            passwordField = _driver.FindElement(By.Name("LoginInput.Password"));
            loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

            usernameField.SendKeys("user"); // Use actual regular user username
            passwordField.SendKeys("password"); // Use actual regular user password
            loginButton.Click();

            // Wait for the Movies page to load
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/Movies"));

            Assert.Contains("/Movies", _driver.Url);

            // Click on a Movie card to reveal additional options
            movieCard = _driver.FindElement(By.CssSelector(".movie-card"));
            movieCard.Click();

            // Wait for the "Add to favorites list" button to be visible
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[text()='Add to favorites list']")));

            // Verify Edit and Delete links are not visible
            editLinks = _driver.FindElements(By.LinkText("Edit"));
            deleteLinks = _driver.FindElements(By.LinkText("Delete"));

            Assert.True(editLinks.Count == 0, "Regular user should not see Edit links.");
            Assert.True(deleteLinks.Count == 0, "Regular user should not see Delete links.");

            // Verify "Add to favorites list" button is visible
            var favoritesButton = _driver.FindElement(By.XPath("//button[text()='Add to favorites list']"));

            Assert.NotNull(favoritesButton);
            Assert.Equal("Add to favorites list", favoritesButton.Text);
        }
    }
}