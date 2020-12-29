using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;
using Microsoft.EntityFrameworkCore;
using SafeNetAtm.Data;
using SafeNetAtm.Domain;
using System.Threading;

namespace SafeNetAtm.Tests.Client
{
    public class HomeTests
    {
        [Fact]
        public void Balance()
        {
            using(IWebDriver driver = new ChromeDriver())
            {
                //Test failure alert
                driver.Navigate().GoToUrl("https://localhost:44365/");
                driver.FindElement(By.Id("btnSubmit")).Click();
                Assert.NotNull(driver.FindElement(By.Id("validationAtmActionChange")).Text);

                //Test Balances returned
                new SelectElement(driver.FindElement(By.Id("ddlAtmAction"))).SelectByValue("B");
                driver.FindElement(By.Id("btnSubmit")).Click();
                Thread.Sleep(1000); //used to allow for the client to load the records
                var tableRows = driver.FindElements(By.XPath("//*[@id='tblBalance']/tbody/tr"));
                Assert.True(tableRows.Count == 6);
            }
        }
    }
}
