using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace RazorPagesMovie.UITests
{
    /// <summary>
    /// Extension methods for Selenium WebElement operations to make tests more robust
    /// </summary>
    public static class ElementExtensions
    {
        /// <summary>
        /// Try to find an element first using a list of selectors, returning the first one that works
        /// </summary>
        public static IWebElement TryFindElement(this IWebDriver driver, params By[] selectors)
        {
            List<Exception> exceptions = new List<Exception>();
            
            foreach (var selector in selectors)
            {
                try
                {
                    return driver.FindElement(selector);
                }
                catch (NoSuchElementException ex)
                {
                    exceptions.Add(ex);
                    // Continue to next selector
                }
            }
            
            throw new NoSuchElementException(
                $"Could not find element with any of {selectors.Length} selectors. " +
                $"Last exception: {exceptions[exceptions.Count - 1].Message}");
        }
        
        /// <summary>
        /// Try to find elements first using a list of selectors, returning elements from the first successful selector
        /// </summary>
        public static IReadOnlyCollection<IWebElement> TryFindElements(this IWebDriver driver, params By[] selectors)
        {
            foreach (var selector in selectors)
            {
                var elements = driver.FindElements(selector);
                if (elements.Count > 0)
                {
                    return elements;
                }
            }
            
            // If no elements were found with any selector, return an empty collection
            return new List<IWebElement>().AsReadOnly();
        }
        
        /// <summary>
        /// Check if an element exists in the DOM
        /// </summary>
        public static bool ElementExists(this IWebDriver driver, By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        
        /// <summary>
        /// Get element text safely, returning empty string if element is null
        /// </summary>
        public static string GetTextSafely(this IWebElement element)
        {
            if (element == null)
            {
                return string.Empty;
            }
            
            try
            {
                return element.Text;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
