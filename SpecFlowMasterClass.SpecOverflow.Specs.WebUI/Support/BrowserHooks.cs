using System;
using TechTalk.SpecFlow;

namespace SpecFlowMasterClass.SpecOverflow.Specs.WebUI.Support
{
    [Binding]
    public class BrowserHooks
    {
        private readonly BrowserContext _browserContext;
        private readonly TestFolders _testFolders;
        private readonly ScenarioContext _scenarioContext;

        public BrowserHooks(BrowserContext browserContext, TestFolders testFolders, ScenarioContext scenarioContext)
        {
            _browserContext = browserContext;
            _testFolders = testFolders;
            _scenarioContext = scenarioContext;
        }

        [AfterScenario("@web", Order = 1)]
        public void HandleWebErrors()
        {
            if (_scenarioContext.TestError != null && _browserContext.IsDriverCreated)
            {
                var fileName = _testFolders.GetScenarioSpecificFileName();
                _browserContext.TakeScreenshot(_testFolders.OutputFolder, fileName);
            }
        }
    }
}
