using Newtonsoft.Json;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using GammaFarming.Model;

namespace GammaFarming
{
    internal class Bot
    {
        private EdgeDriver window;
        private List<string> tabs;
        private User[] dummyData;
        private string tempEmail;
        private List<string> landings;

        public Bot()
        {
            dummyData = JsonConvert.DeserializeObject<User[]>(File.ReadAllText("./users.json"));
            tabs = new();
            tempEmail = "";
            landings = new List<string>() { "landing", "landing.", "landing..", "landing..." };
        }

        public void FarmCredits(string link, byte times)
        {
            for(byte i = 0; i < times; i++)
            {
                Get200Credits(link);
            }
        }

        private void Get200Credits(string referrallink)
        {
            window = new();

            //-------TempMail-------- (Tab 1)
            window.Navigate().GoToUrl(@"https://emailtemp.org/es");
            tabs.Add(window.CurrentWindowHandle);
            this.tempEmail = QuerySelector("input.custom-email-input").GetAttribute("value");

            // Esperar hasta que el valor del correo temporal no sea "landing"
            while (landings.Contains(tempEmail))
            {
                this.tempEmail = QuerySelector("input.custom-email-input").GetAttribute("value");
                Thread.Sleep(100); // Esperar un momento antes de volver a verificar
            }

            //--------------Gama Tab-------------- (Tab 2)
            tabs.Add(CreateAndGoToNewTab());
            window.Navigate().GoToUrl(referrallink);
            QuerySelector("input#email").SendKeys(tempEmail);
            QuerySelector("button.chakra-button.css-500mhu").Click();  //ContinueButton click

            //try //                        I cant bypass CAPTCHA GODdammit :)
            //{
            //    var waiting = new WebDriverWait(window, TimeSpan.FromSeconds(5));
            //    waiting.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div[role=\"presentation\"]")));
            //}
            //catch
            //{
            //}

            //-------------------TempMail--------------------- (Tab 2)
            //    Wait and click received email from gamma
            window.SwitchTo().Window(tabs.First());
            QuerySelector("i.fas.fa-chevron-right").Click(); //Click on emaildeliveredDiv (>)
            string hrefRealEmail = QuerySelector("div.content iframe").GetAttribute("src");

            //----------Verification Email tab ---------- (Tab 3)
            tabs.Add(CreateAndGoToNewTab());
            window.Navigate().GoToUrl(hrefRealEmail);
            string signUpGammaURL = QuerySelector("a.button").GetAttribute("href");

            //----------Sign Up Page GAMMA ------------- (Tab 4)
            tabs.Add(CreateAndGoToNewTab());
            window.Navigate().GoToUrl(signUpGammaURL);
            var firstName = GetElementById("first name");
            var lastName = GetElementById("last name");
            var password = GetElementById("password");

            User u = dummyData[new Random().Next(0, dummyData.Length)];
            firstName.SendKeys(u.first_name);
            lastName.SendKeys(u.last_name);
            password.SendKeys(u.password);

            var buttonSubmit = QuerySelector("button[type=\"submit\"]");
            buttonSubmit.Click();

            var workSpaceField = GetElementById("field-:r11:");
            workSpaceField.SendKeys(u.workspace);

            window.Quit();
            tabs.Clear();
        }



        private IWebElement QuerySelector(string cssSelector, byte time = 50)
        {
            var waiting = new WebDriverWait(window, TimeSpan.FromSeconds(time));
            return waiting.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(cssSelector)));
        }
        private IWebElement GetElementById(string id, byte time = 50 )
        {
            var waiting = new WebDriverWait(window, TimeSpan.FromSeconds(time));
            return waiting.Until(ExpectedConditions.ElementIsVisible(By.Id(id)));
        }
        private void TestTab()
        {
            var window = new EdgeDriver();
            string firstPage = window.CurrentWindowHandle;
            window.Navigate().GoToUrl(@"https://google.com");

            window.SwitchTo().NewWindow(WindowType.Tab);
            window.Navigate().GoToUrl(@"https://facebook.com");
            string secondPage = window.CurrentWindowHandle;
        }

        /// <summary>
        /// Returns id of the new window and adds a new tab to actual window
        /// </summary>
        /// <returns></returns>
        private string CreateAndGoToNewTab()
        {
            window.SwitchTo().NewWindow(WindowType.Tab);
            return window.CurrentWindowHandle;
        }
    }
}
