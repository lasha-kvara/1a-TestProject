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
    public class Dashboard : BaseClass
    {
        public Dashboard(IWebDriver driver) : base(driver)
        {
            this.driver = driver;
        }

        public void SearchProductAndVerify(string searchValue)
        {
            Search_Input.SendKeys(searchValue);
            SearchSubmit_Button.Click();

            // Verify that the search results page is displayed
            var pageTitle = driver.Title;
            Assert.IsTrue(pageTitle.Contains(searchValue), "Search results page not displayed");
            Thread.Sleep(3000);

            // Verify that the search results are accurate and relevant to the search query
            var searchResultItems = driver.FindElements(By.XPath("(//*[@class='sn-docs ks-product-grid-row'])[1]//*[@class='sn-product-inner ks-gtm-categories']//*[@class='ks-new-product-name']"));
            ScrollDown(driver);
            for (int i = 1; i <= searchResultItems.Count; i++)
            {
                var productName = driver.FindElement(By.XPath($"((//*[@class='sn-docs ks-product-grid-row'])[1]//*[@class='sn-product-inner ks-gtm-categories']//*[@class='ks-new-product-name'])[{i}]")).Text;
                //Console.WriteLine($"Product Name {i}: " + productName); //If you want to see all the searched product names in console, uncomment this line
                Assert.IsTrue(productName.ToLower().Contains(searchValue.ToLower()), "Search results not relevant to the search query");
            }
        }


        public void AddProductToCartAndVerify()
        {
            //Add To Cart
            Actions action = new Actions(driver);
            string searchedFirstElement_Name = searchedFirstproductName.Text;
            Console.WriteLine("Searched First Element Name : " + searchedFirstElement_Name);
            string firstProductPrice = FirstProductPrice_OnDashboard.Text;
            double firstProductPrice_double = ConvertToDouble(firstProductPrice);
            Console.WriteLine("Price Double = " + firstProductPrice_double);
            searchedFirstproductName.Click();
            string prodName_FromDetailPage = ProductName_FromDetailPage.Text;
            Assert.That(searchedFirstElement_Name, Is.EqualTo(prodName_FromDetailPage));
            Console.WriteLine("Product Name From Detail Page : " + prodName_FromDetailPage);
            DisableCookieMessage();
            Thread.Sleep(2000);
            action.MoveToElement(AddToCart_Button_FromDetailPage).Perform();
            AddToCart_Button_FromDetailPage.Click();
            Thread.Sleep(3000);
            string successMessage = AddCart_SuccessMessage.Text;
            Assert.That(successMessage, Is.EqualTo("Prekė sėkmingai įdėta į pirkinių krepšelį"));
            string prodNameInCart = ProductNameInCart.Text;
            Assert.That(prodNameInCart, Is.EqualTo(prodName_FromDetailPage));
            TakeScreenshot(driver, "Add To Cart Product.png");

            //Verify Product In Cart
            Thread.Sleep(2000);
            ViewShoppingCart_Button_FromAddCartPopup.Click();
            string itemPrice_inCart = RemoveChars(ItemPrice_InCart.Text);
            double itemPrice_inCart_Double = ConvertToDouble(itemPrice_inCart);
            string itemTotalPrice_inCart = RemoveChars(ItemTotalPrice_InCart.Text);
            double itemTotalPrice_inCart_Double = ConvertToDouble(itemTotalPrice_inCart);
            Console.WriteLine("Item Price From Cart : " + itemPrice_inCart_Double);
            Console.WriteLine("Item Total Price From Cart : " + itemTotalPrice_inCart_Double);
            Assert.That(itemPrice_inCart_Double, Is.EqualTo(firstProductPrice_double));
            string productCountInCart = RemoveChars(ProductCountInCart_Input.GetAttribute("value"));
            double productCountInCart_double = ConvertToDouble(productCountInCart);
            Console.WriteLine("Product Count in Cart = " + productCountInCart_double);
            Assert.That(itemTotalPrice_inCart_Double, Is.EqualTo(itemPrice_inCart_Double * productCountInCart_double));
            Console.WriteLine(itemPrice_inCart_Double + " * " + productCountInCart_double + " = " + itemTotalPrice_inCart_Double);
            
        }


        public IWebElement Search_Input => driver.FindElement(By.XPath("//*[@id='top-search-form']//input[2]"));
        public IWebElement SearchSubmit_Button => driver.FindElement(By.ClassName("main-search__submit"));
        public IWebElement SearchResultItems => (IWebElement)driver.FindElements(By.ClassName("ks-new-product-name"));
        public IWebElement ProductPrices_OnDashboard => (IWebElement)driver.FindElements(By.XPath("//*[@class='sn-product-inner ks-gtm-categories']//*[@class='ks-item-price']/span[1]"));
        public IWebElement FirstProductPrice_OnDashboard => driver.FindElement(By.XPath("(//*[@class='sn-product-inner ks-gtm-categories']//*[@class='ks-item-price']/span[1])[1]"));
        public IWebElement searchedFirstproductName => driver.FindElement(By.XPath("((//*[@class='sn-docs ks-product-grid-row'])[1]//*[@class='sn-product-inner ks-gtm-categories']//*[@class='ks-new-product-name'])[1]"));
        public IWebElement ProductName_FromDetailPage => driver.FindElement(By.CssSelector("body > div.site > div.site-center > div > div.detailed-product-block.content-block.shadow-depth-1 > div.clearfix.min-600 > div.product-righter.google-rich-snippet > h1"));
        public IWebElement AddToCart_Button_FromDetailPage => driver.FindElement(By.XPath("//button[@id='add_to_cart_btn']"));
        public IWebElement ContinueShopping_Button_FromAddCartPopup => driver.FindElement(By.Id("continue_shopping"));
        public IWebElement ViewShoppingCart_Button_FromAddCartPopup => driver.FindElement(By.CssSelector("#add-to-cart > div.controls.clearfix.tac > a.main-button"));
        public IWebElement AddCart_SuccessMessage => driver.FindElement(By.ClassName("title-success"));
        public IWebElement CookieButton => driver.FindElement(By.XPath("//*[@id='cookie-btns']/a[1]"));

        public IWebElement ItemPrice_InCart => driver.FindElement(By.XPath("//*[@class='detailed-cart-item__price']"));
        public IWebElement ItemTotalPrice_InCart => driver.FindElement(By.XPath("//*[@class='detailed-cart-item__total']"));
        public IWebElement DeleteProductFromCart_Button => driver.FindElement(By.ClassName("detailed-cart-item__delete-wrap-text"));
        public IWebElement ProductCountInCart_Input => driver.FindElement(By.XPath("//*[@class='detailed-cart-item__form-item']/input"));
        public IWebElement ProductNameInCart => driver.FindElement(By.XPath("//*[@class='detailed-cart-item__name-link']"));



    }
}
