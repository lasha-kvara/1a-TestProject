using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.BaseLayer;

namespace TestProject.POM
{
    public class SignUpPage : BaseClass
    {
        public SignUpPage(IWebDriver driver) : base(driver)
        {
            this.driver = driver;
        }
   
        public void RegisterNewUser(string name, string surName, string email, string password)
        {
            Name_Input.SendKeys(name);
            SurName_Input.SendKeys(surName);
            Email_Input.SendKeys(email);
            ScrollDown(driver);
            Password_Input.SendKeys(password); 
            RepeatPassword_Input.SendKeys(password);
            FirstAgreement_Checkbox.Click();
            SecondAgreement_Checkbox.Click();
            Register_Button.Click();
            ExplicitWait(driver, s => RegisterConfirmSuccess_Icon, 10);
            if (RegisterConfirmSuccess_Icon.Displayed)
            {
                Console.WriteLine("Register Successed");
            }
        }

        public IWebElement Name_Input => driver.FindElement(By.Id("user_first_name"));
        public IWebElement SurName_Input => driver.FindElement(By.Id("user_last_name"));
        public IWebElement Email_Input => driver.FindElement(By.Id("user_email"));
        public IWebElement Password_Input => driver.FindElement(By.Id("user_password"));
        public IWebElement RepeatPassword_Input => driver.FindElement(By.Id("user_password_confirmation"));
        public IWebElement FirstAgreement_Checkbox => driver.FindElement(By.Id("user_marketing_consent_1"));
        public IWebElement SecondAgreement_Checkbox => driver.FindElement(By.Id("user_marketing_consent_2"));
        public IWebElement Register_Button => driver.FindElement(By.ClassName("users-session-form__submit"));
        public IWebElement RegisterConfirmSuccess_Icon=> driver.FindElement(By.ClassName("users-confirmation__success-icon"));



        //users-confirmation__success-icon
    }
}
