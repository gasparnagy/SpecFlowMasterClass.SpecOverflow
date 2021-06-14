using System;
using System.Collections.Concurrent;
using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace SpecFlowMasterClass.SpecOverflow.Specs.WebUI.Support
{
    public class BrowserFactory
    {
        public class BrowserInstance : IDisposable
        {
            private readonly Lazy<IWebDriver> _webDriver;

            public BrowserInstance()
            {
                _webDriver = new Lazy<IWebDriver>(CreateWebDriver);
            }

            private IWebDriver CreateWebDriver()
            {
                return new ChromeDriver();
            }

            public IWebDriver GetWebDriver()
            {
                return _webDriver.Value;
            }

            public void Dispose()
            {
                if (_webDriver.IsValueCreated)
                    _webDriver.Value.Dispose();
            }
        }
        
        private readonly IObjectContainer _browserObjectContainer;
        private IWebDriver _browserCreated;

        public BrowserFactory(ScenarioContext scenarioContext)
        {
            // creating browser per test-thread (requires TestThreadContainerDisposer below)
            _browserObjectContainer = scenarioContext.ScenarioContainer.Resolve<TestThreadContext>().TestThreadContainer;
            // creating browser per feature
            //_browserObjectContainer = scenarioContext.ScenarioContainer.Resolve<FeatureContext>().FeatureContainer;
            // creating browser per scenario
            //_browserObjectContainer = scenarioContext.ScenarioContainer;
        }

        public IWebDriver CreateBrowser()
        {
            if (_browserCreated == null)
                _browserCreated = _browserObjectContainer.Resolve<BrowserInstance>().GetWebDriver();
            return _browserCreated;
        }

        #region TestThreadContainerDisposer

        [Binding]
        public class TestThreadContainerDisposer
        {
            private static readonly ConcurrentBag<IObjectContainer> TestThreadContainers = new();

            [BeforeScenario(Order = -1)]
            public void EnsureTestThreadContainerRegistered(ScenarioContext scenarioContext)
            {
                var testThreadContainer = scenarioContext.ScenarioContainer.Resolve<TestThreadContext>().TestThreadContainer;
                TestThreadContainers.Add(testThreadContainer);
            }

            [AfterTestRun]
            public static void DisposeTestThreadContainers()
            {
                var containers = TestThreadContainers.ToArray();
                foreach (var container in containers)
                {
                    container.Dispose();
                }
            }
        }

        #endregion
    }
}
