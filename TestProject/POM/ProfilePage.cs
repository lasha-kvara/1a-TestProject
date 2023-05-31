using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.BaseLayer;

namespace TestProject.POM
{
    public class ProfilePage : BaseClass
    {
        public ProfilePage(IWebDriver driver) : base(driver)
        {
            this.driver = driver;
        }

        public string GetFirstName_FromProfile()
        {
            string currencName = Name_InProfilePage.Text;
            return currencName;
        }
        public void UpdateAccountFirstName(string newName)
        {
            Actions action = new Actions(driver);
            string currentName = GetFirstName_FromProfile();
            Console.WriteLine(currentName);
            Thread.Sleep(2000);
            Change_Button.Click();
            TakeScreenshot(driver, "After Click Change Profile Details Button.png");
            Thread.Sleep(2000);
            action.MoveToElement(ChangeName_Input).Build().Perform();
            ChangeName_Input.Clear();
            ChangeName_Input.SendKeys(newName);
            ExplicitWait(driver, sa => ConfirmChange_Button, 10);
            action.MoveToElement(ConfirmChange_Button).Click().Build().Perform();
            Thread.Sleep(2000);
            string changedName = GetFirstName_FromProfile();
            Assert.That(changedName, Is.EqualTo(newName));

        }






        public IWebElement MyProfile_Button_InProfilePage => driver.FindElement(By.XPath("//*[@class='profile-menu__link'][contains(text(),'Mano profilis')]"));
        public IWebElement Change_Button => driver.FindElement(By.XPath("//*[@class='profile-user-data__edit-button']/a"));
        public IWebElement Name_InProfilePage => driver.FindElement(By.XPath("(//*[@class='profile-user-data__text-row']//td)[8]"));
        public IWebElement ChangeName_Input => driver.FindElement(By.Id("user_first_name"));
        public IWebElement ConfirmChange_Button => driver.FindElement(By.XPath("//*[@class='profile-user-form__form-controls']/input"));





    }
}
