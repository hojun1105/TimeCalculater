using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using static System.Net.Mime.MediaTypeNames;

namespace TimeCalculator
{
    public class Crawler
    {
        private string _id;
        private string _password;
        public Crawler(string id, string password)
        {
            _id = id;
            _password = password;
            SetXPath();
        }

        private List<string> xPathList;

        private void SetXPath() 
        {
            xPathList = new List<string>()
            {
                "//*[@id=\'contents\']/div/section[3]/div/section/div[2]/table/tbody/tr/td[1]/div/div[2]/div[2]/span",
                "//*[@id=\'contents\']/div/section[3]/div/section/div[2]/table/tbody/tr/td[1]/div/div[2]/div[4]/span",
                "//*[@id=\'contents\']/div/section[3]/div/section/div[2]/table/tbody/tr/td[2]/div/div[2]/div[2]/span",
                "//*[@id=\'contents\']/div/section[3]/div/section/div[2]/table/tbody/tr/td[2]/div/div[2]/div[4]/span",
                "//*[@id=\'contents\']/div/section[3]/div/section/div[2]/table/tbody/tr/td[3]/div/div[2]/div[2]/span",
                "//*[@id=\'contents\']/div/section[3]/div/section/div[2]/table/tbody/tr/td[3]/div/div[2]/div[4]/span",
                "//*[@id=\'contents\']/div/section[3]/div/section/div[2]/table/tbody/tr/td[4]/div/div[2]/div[2]/span",
                "//*[@id=\'contents\']/div/section[3]/div/section/div[2]/table/tbody/tr/td[4]/div/div[2]/div[4]/span",
                "//*[@id=\'contents\']/div/section[3]/div/section/div[2]/table/tbody/tr/td[5]/div/div[2]/div[2]/span",
                "//*[@id=\'contents\']/div/section[3]/div/section/div[2]/table/tbody/tr/td[5]/div/div[2]/div[4]/span"
            };
    }

        #region Method

        public List<string> Crawl()
        {
            Login(out ChromeDriver driver);
            return CrawlDate(driver);
        }

        public void Login(out ChromeDriver driver)
        {
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("headless");
            options.AddArgument("ignore-certificate-errors");
            driver = new ChromeDriver(driverService, options);
            driver.Navigate().GoToUrl("https://login.office.hiworks.com/smartdoctor.onhiworks.com");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

            var loginInput = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\'root\']/div/main/div/div[1]/form/fieldset/div[2]/div/input")));
            loginInput.SendKeys(_id);
            
            var button = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\'root\']/div/main/div/div[1]/form/fieldset/button")));
            button.Click();

            var passWordInput = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[class*='mantine-TextInput-input'][type='password']")));
            passWordInput.SendKeys(_password);
            button = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\'root\']/div/main/div/div[1]/form/fieldset/button")));
            button.Click();

            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\'contents\']/div[2]/div[1]/div[1]/div/p")));
            }
            catch (TimeoutException)
            {
                Console.WriteLine("수동 로그인이 제 시간에 완료되지 않았습니다.");
            }
        }

        public List<string> CrawlDate(ChromeDriver driver)
        {
            WebDriverWait waitForTimeData = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
            var timeDataList = new List<string>();
            
            driver.Navigate().GoToUrl("https://hr-work.office.hiworks.com/personal/index");       
            foreach (string path in xPathList)
            {
                
                By aPath = By.XPath(path);
                try
                {
                    var element = waitForTimeData.Until(ExpectedConditions.ElementExists(aPath));
                    timeDataList.Add(element.Text);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return timeDataList;
        }

        #endregion
    }
}
