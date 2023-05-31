using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TestProject.POM;

namespace TestProject.BaseLayer
{
    public class BaseClass
    {
        public IWebDriver driver;
        public BaseClass(IWebDriver driver)
        {
            this.driver = driver;
        }
        public BaseClass()
        {

        }
        [SetUp]
        public void Setup()
        {
            //Setup Driver Before Each Test and Navigate to URL
            ChromeOptions chromeoptions = new ChromeOptions();
            driver = new ChromeDriver(chromeoptions);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://www.1a.lt/");
            TakeScreenshot(driver, "SetUp Success.png");
        }

        //This Method Takes Screenshot And Saves in to the Project Directory/bin/Screenshots Folder
        public void TakeScreenshot(IWebDriver driver, string fileName)
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            var projectDirectory = Directory.GetParent(TestContext.CurrentContext.TestDirectory).Parent.FullName;
            var screenshotDirectory = Path.Combine(projectDirectory, "Screenshots");
            Directory.CreateDirectory(screenshotDirectory);
            var filePath = Path.Combine(screenshotDirectory, fileName);
            screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);
        }

        public void OpenNewTabAndSwitch()
        {
            Actions action = new Actions(driver);
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
            // Switch to the new tab
            driver.SwitchTo().Window(driver.WindowHandles.Last());
        }

        //This Method Verifies If Element is Displayed
        public bool IsElementDisplayed(IWebElement element)
        {
            try
            {
                return element.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        //This Method do Switch to Tab
        public static void SwitchToTab(IWebDriver driver, int to = 1) => driver.SwitchTo().Window(driver.WindowHandles[to]);

        public static IWebElement ExplicitWait(IWebDriver driver, Func<IWebDriver, IWebElement> condition, int timeoutInSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(condition);
        }

        public static string RandomString(int length)
        {
            var random = new Random();
            const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static void ScrollUP(IWebDriver driver)
        {
            Actions action = new Actions(driver);
            action.SendKeys(Keys.PageUp).Build().Perform();
        }

        public static void ScrollDown(IWebDriver driver)
        {
            Actions action = new Actions(driver);
            action.SendKeys(Keys.PageDown).Build().Perform();
        }

        public double ConvertToDouble(string str_number)
        {
            double number1_double;
            CultureInfo culture;

            if (str_number.Contains("."))
            {
                culture = new CultureInfo("en-US");
            }
            else
            {
                culture = new CultureInfo("fr-FR");
            }

            if (double.TryParse(str_number, NumberStyles.Number, culture, out number1_double))
            {
                Console.WriteLine("Double ----- " + number1_double);
            }
            else
            {
                Console.WriteLine("Unable to parse the string as a double.");
            }
            return number1_double;
        }


        public void DisableCookieMessage()
        {
            var dashboard = new Dashboard(driver);
            try
            {
                if (IsElementDisplayed(dashboard.CookieButton))
                {
                    dashboard.CookieButton.Click();
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Catch"); ;
            }
            
        }

        public static string RemoveChars(string someString)
        {
            var charsToRemove = new string[] { "$", "₾", "€", "v", "n", "t", ".", " " };
            foreach (var c in charsToRemove)
            {
                someString = someString.Replace(c, string.Empty);
            }
            return someString;
        }

        [TearDown] 
        public void TearDown()
        {
            //After Each Test
            TakeScreenshot(driver, "After Test Screen.png");
            driver.Quit();
        }

    }
}
