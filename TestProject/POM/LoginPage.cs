using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.BaseLayer;

namespace TestProject.POM
{
    public class LoginPage : BaseClass
    {

        public LoginPage(IWebDriver driver) : base(driver)
        {
            this.driver = driver;
        }

        public void VerifyLogin(string userEmail, string password)
        {
            var profilePage = new ProfilePage(driver);
            UserEmail_Input.SendKeys(userEmail);
            UserPassword_Input.SendKeys(password);
            LoginButton.Click();
            var userblock = ExplicitWait(driver, d => UserBlock, 10);
            userblock.Click();
            var myProfileButton_isDisplayed = IsElementDisplayed(profilePage.MyProfile_Button_InProfilePage);
            Assert.IsTrue(myProfileButton_isDisplayed, "My Profile button is not visible on the page.");
            DisableCookieMessage();
            Console.WriteLine("Login Successed");
        }





        public IWebElement UserBlock => driver.FindElement(By.ClassName("user-block__title"));
        public IWebElement UserEmail_Input => driver.FindElement(By.Id("user_email"));
        public IWebElement UserPassword_Input => driver.FindElement(By.Id("user_password"));
        public IWebElement LoginButton => driver.FindElement(By.ClassName("users-session-form__submit"));
        public IWebElement RegistrationButton => driver.FindElement(By.XPath("//*[@class='users-session-form__signup']//*[contains(text(),'Registruotis')]"));
    
    
    
    
    
    
    }
}
