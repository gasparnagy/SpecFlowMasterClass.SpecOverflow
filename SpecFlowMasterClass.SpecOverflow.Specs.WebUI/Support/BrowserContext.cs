using System;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace SpecFlowMasterClass.SpecOverflow.Specs.WebUI.Support
{
    public class BrowserContext : IDisposable
    {
        private readonly AppHostingContext _appHostingContext;
        private readonly Lazy<IWebDriver> _webDriver;

        public bool IsDriverCreated => _webDriver.IsValueCreated;
        public IWebDriver Driver => _webDriver.Value;
        public Uri BaseUrl => _appHostingContext.RootUrl;

        public BrowserContext(BrowserFactory browserFactory, AppHostingContext appHostingContext, ScenarioContext scenarioContext)
        {
            _appHostingContext = appHostingContext;
            _webDriver = new Lazy<IWebDriver>(() =>
            {
                _appHostingContext.StartApp();
                var webDriver = browserFactory.CreateBrowser();
                scenarioContext.ScenarioContainer.RegisterInstanceAs(webDriver); // to be able to directly use the IWebDriver dependency
                return webDriver;
            });
        }

        public void NavigateTo(string relativeUrl, bool waitForContentLoaded = false)
        {
            Driver.Navigate().GoToUrl(new Uri(BaseUrl, relativeUrl));
            StringAssert.Contains(Driver.Title, "Spec Overflow");
            if (waitForContentLoaded)
                WaitForPageLoaded();
        }

        public void AssertOnPath(string relativeUrl)
        {
            Wait.For(() => // the busy waiting is needed by the new Firefox driver
            {
                var expectedUrl = new Uri(BaseUrl, relativeUrl).GetLeftPart(UriPartial.Path);
                var actualUrl = new Uri(Driver.Url).GetLeftPart(UriPartial.Path);
                actualUrl.Should().BeEquivalentTo(expectedUrl, $"browser should be on page '{relativeUrl}'");
            });
        }

        public void AssertNotOnPath(string relativeUrl)
        {
            Wait.For(() => // the busy waiting is needed by the new Firefox driver
            {
                var expectedUrl = new Uri(BaseUrl, relativeUrl).GetLeftPart(UriPartial.Path);
                var actualUrl = new Uri(Driver.Url).GetLeftPart(UriPartial.Path);
                actualUrl.Should().NotBeEquivalentTo(expectedUrl, $"browser should not be on page '{relativeUrl}'");
            });
        }

        public void WaitForFormSubmitFinished()
        {
            Wait.For(() => // the busy waiting is needed by the new Firefox driver
            {
                var ajaxFormStatus = Driver.FindElement(By.TagName("body")).GetAttribute("data-ajax-form") ?? "n/a";
                ajaxFormStatus.Should().BeOneOf(new [] {"0", "n/a"}, "the form submit should have been finished");
            });
        }

        public void WaitForPageLoaded()
        {
            Wait.For(() => // the busy waiting is needed by the new Firefox driver
            {
                var ajaxLoadedStatus = Driver.FindElement(By.TagName("body")).GetAttribute("data-ajax-loaded") ?? "n/a";
                ajaxLoadedStatus.Should().BeOneOf(new[] { "1" }, "the page content should have been loaded");
            });
        }

        public void ShouldNotHaveFormErrors(string errors = null)
        {
            errors ??= Driver.FindElements(By.Id("ErrorMessage")).FirstOrDefault()?.Text;
            errors.Should().BeNullOrWhiteSpace("the form should not have errors");
        }

        public void SubmitFormWith(IWebElement submitButton, bool verify)
        {
            submitButton.Click();
            WaitForFormSubmitFinished();
            if (verify)
            {
                ShouldNotHaveFormErrors();
            }
        }

        public void TakeScreenshot(string outputFolder, string fileNameBase)
        {
            try
            {
                string pageSource = Driver.PageSource;
                string sourceFilePath = Path.Combine(outputFolder, fileNameBase + "_source.html");
                File.WriteAllText(sourceFilePath, pageSource, Encoding.UTF8);
                Console.WriteLine("Page source: {0}", new Uri(sourceFilePath));

                if (Driver is ITakesScreenshot takesScreenshot)
                {
                    var screenshot = takesScreenshot.GetScreenshot();
                    string screenshotFilePath = Path.Combine(outputFolder, fileNameBase + "_screenshot.png");
                    screenshot.SaveAsFile(screenshotFilePath, ScreenshotImageFormat.Png);

                    Console.WriteLine("Screenshot: {0}", new Uri(screenshotFilePath));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while taking screenshot: {0}", ex);
            }
        }

        public void ClearSession()
        {
            try
            {
                if (IsDriverCreated)
                    Driver.Manage().Cookies.DeleteAllCookies();
            }
            catch (Exception ex)
            {
                Console.WriteLine("SessionClearingError: {0}", ex);
            }
        }
        
        public void Dispose()
        {
            // the browser is closed by the BrowserFactory, but we clear the cookies at least
            // so that if the browser was cached, it should be set to fresh
            ClearSession();
        }
    }
}
