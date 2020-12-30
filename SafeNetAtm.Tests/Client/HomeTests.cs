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
        private const string url = "https://localhost:44365/";

        [Fact]
        public void AtmAction_Failure_Test()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                //Test failure alert
                driver.Navigate().GoToUrl(url);
                driver.FindElement(By.Id("btnSubmit")).Click();
                Assert.NotNull(driver.FindElement(By.Id("validationAtmActionChange")).Text);
            }
        }

        [Fact]
        public void AtmAction_Balance_Test()
        {
            using(IWebDriver driver = new ChromeDriver())
            {
                //Test Balances returned
                driver.Navigate().GoToUrl(url);
                new SelectElement(driver.FindElement(By.Id("ddlAtmAction"))).SelectByValue("B");
                driver.FindElement(By.Id("btnSubmit")).Click();
                Thread.Sleep(1000); //used to allow for the client to load the records
                var tableRows = driver.FindElements(By.XPath("//*[@id='tblBalance']/tbody/tr"));
                Assert.True(tableRows.Count == 6);
            }
        }

        [Fact]
        public void AtmAction_DenomBalance_Test()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                //Test Denomination Balances returned
                driver.Navigate().GoToUrl(url);
                new SelectElement(driver.FindElement(By.Id("ddlAtmAction"))).SelectByValue("I");
                new SelectElement(driver.FindElement(By.Id("ddlDenoms"))).SelectByValue("50");
                new SelectElement(driver.FindElement(By.Id("ddlDenoms"))).SelectByValue("20");
                new SelectElement(driver.FindElement(By.Id("ddlDenoms"))).SelectByValue("10");
                driver.FindElement(By.Id("btnSubmit")).Click();
                Thread.Sleep(1000); //used to allow for the client to load the records
                var tableRows = driver.FindElements(By.XPath("//*[@id='tblBalance']/tbody/tr"));
                Assert.True(tableRows.Count == 3);
            }
        }

        [Fact]
        public void AtmAction_RestockAndWithdraw_Test()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(url);
                var ddlAtmAction = new SelectElement(driver.FindElement(By.Id("ddlAtmAction")));
                var btnSubmit = driver.FindElement(By.Id("btnSubmit"));

                //Test Restock
                ddlAtmAction.SelectByValue("R");
                btnSubmit.Click();
                Thread.Sleep(1000); //used to allow for the client to load the records
                var tableRows = driver.FindElements(By.XPath("//*[@id='tblBalance']/tbody/tr"));
                Assert.True(tableRows.Count == 6);

                //Test Withdraw success
                ddlAtmAction.SelectByValue("W");
                var numWithdraw = driver.FindElement(By.Id("numWithdraw"));
                numWithdraw.Click();
                numWithdraw.Clear();
                numWithdraw.SendKeys("1234");
                btnSubmit.Click();
                Thread.Sleep(1000); //used to allow for the client to load the records
                Assert.Contains("Success: Dispensed $ 1,234", driver.FindElement(By.Id("SuccessWithdraw")).Text);

                //Test Withdraw insufficient funds
                ddlAtmAction.SelectByValue("W");
                numWithdraw.Click();
                numWithdraw.Clear();
                numWithdraw.SendKeys("1234");
                btnSubmit.Click();
                Thread.Sleep(1000); //used to allow for the client to load the records
                Assert.Contains("Failure: insufficient funds", driver.FindElement(By.Id("balanceErrMsg")).Text);
            }
        }
    }
}
