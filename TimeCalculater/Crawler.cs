using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V112.Debugger;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace TimeCalculator;

public class Crawler
{
    private readonly string _id;
    private readonly string _password;

    private List<string> xPathList;

    public Crawler(string id, string password)
    {
        _id = id;
        _password = password;
        SetXPath();
    }

    private void SetXPath()
    {
        xPathList = new List<string>
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

    public string Crawl()
    {
        Login(out var driver);
        var res = CrawlDate2(driver);
        return res;
    }

    public void Login(out ChromeDriver driver)
    {
        new DriverManager().SetUpDriver(new ChromeConfig());
        var driverService = ChromeDriverService.CreateDefaultService();
        driverService.HideCommandPromptWindow = true;

        var options = new ChromeOptions();
        options.AddArgument("headless");
        options.AddArgument("ignore-certificate-errors");
        driver = new ChromeDriver(driverService,options);
        driver.Navigate().GoToUrl("https://login.office.hiworks.com/smartdoctor.onhiworks.com");
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        var loginInput =
            wait.Until(ExpectedConditions.ElementExists(
                By.XPath("//*[@id=\'root\']/div/main/div/div[1]/form/fieldset/div[2]/div/input")));
        loginInput.SendKeys(_id);

        var button = 
            wait.Until(ExpectedConditions.ElementExists(
                By.XPath("//*[@id=\'root\']/div/main/div/div[1]/form/fieldset/button")));
        
        button.Click();

        var passWordInput =
            wait.Until(ExpectedConditions.ElementExists(
                By.CssSelector("input[class*='mantine-TextInput-input'][type='password']")));
        
        passWordInput.SendKeys(_password);
        
        button = wait.Until(
            ExpectedConditions.ElementExists(By.XPath("//*[@id=\'root\']/div/main/div/div[1]/form/fieldset/button")));

        button.Click();

        try
        {
            wait.Until(
                ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\'contents\']/div[2]/div[1]/div[1]/div/p")));
        }
        catch (TimeoutException)
        {
            Console.WriteLine("수동 로그인이 제 시간에 완료되지 않았습니다.");
        }
    }

    public List<string> CrawlDate(ChromeDriver driver)
    {
        var waitForTimeData = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
        var timeDataList = new List<string>();

        driver.Navigate().GoToUrl("https://hr-work.office.hiworks.com/personal/index");
        foreach (var path in xPathList)
        {
            var aPath = By.XPath(path);
            try
            {
                var element = waitForTimeData.Until(ExpectedConditions.ElementExists(aPath));
                timeDataList.Add(element.Text);
            }
            catch (Exception)
            {
            }
        }

        return timeDataList;
    }

    public string CrawlDate2(ChromeDriver driver)
    {
        WebDriverWait waitForTimeData = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
        driver.Navigate().GoToUrl("https://hr-work.office.hiworks.com/personal/index");

        var timeDataList = waitForTimeData.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"contents\"]/div/section[3]/div/section/div[2]/table/tbody/tr")));
        return timeDataList.Text;
    }

    #endregion
}