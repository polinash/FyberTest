using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium.IE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace TestProject1
{  
    /// <summary>
	/// Base Test Class incapable of crossbrowser test runs
	/// </summary>
    [TestClass]
    public class BaseTest
    {
        
        protected IWebDriver Driver;
        //number of tries - to bypass Selemium's problem of not finding elements
        private const int TimeoutEffortCount = 2;

       	#region Init section

		[TestInitialize]
		public void SetupTest()
		{
            Driver = new OpenQA.Selenium.IE.InternetExplorerDriver();
		}

		[TestCleanup]
		public void TeardownTest()
		{
			try
			{
				Driver.Quit();
			}
			catch
			{
			}
		}

		#endregion


        public void Navigate(string navUrl)
		{
            if (navUrl != null)
                Driver.Navigate().GoToUrl(navUrl);
			Wait();
		}

        protected void Wait()
        {
            Thread.Sleep(3000);
        }

        public bool IsElementPresent(string xPath)
        {
            return (FindElement(xPath) != null);
        }

        public void AssertElementPresent(string xPath, string messageText)
        {
            var i = 0;
            while (i < TimeoutEffortCount)
            {
                try
                {
                    IsElementPresent(xPath).ShouldBeTrue(messageText);
                    return;
                }
                catch (Exception)
                {
                    i++;
                    Thread.Sleep(1000);
                }
            }
            IsElementPresent(xPath).ShouldBeTrue(messageText);
        }

        protected IWebElement FindElement(string by)
		{
            //Selenium WebDriver has a commonly known problem of sometimes 
            //not finding the element so i try to bypass if by offering a few
            //tries - it increases the total time but really helps with misses. 
			var i = 0;
			while (i < TimeoutEffortCount)
			{
				try
				{
					return Driver.FindElement(By.XPath(by));
				}
				catch (NoSuchElementException)
				{
					i++;
					Thread.Sleep(3000);
				}
			}
			return null;
		}

        protected IWebElement FindElement(string xpath, string text)
        {
            return Driver.FindElement(By.XPath(String.Format("{0}[contains(text(), '{1}')]", xpath, text)));

        }

        protected void ClickElement(string xpath, string text)
        {
            FindElement(xpath, text).Click();
        }

        protected void ClickElement(string by)
        {
            FindElement(by).Click();
        }

        protected string GetElementText(string by)
        {
            return FindElement(by).Text;
        }

    }
}
