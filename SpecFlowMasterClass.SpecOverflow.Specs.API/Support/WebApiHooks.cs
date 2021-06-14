using System;
using TechTalk.SpecFlow;

namespace SpecFlowMasterClass.SpecOverflow.Specs.API.Support
{
    [Binding]
    public class WebApiHooks
    {
        private readonly WebApiContext _webApiContext;
        private readonly TestFolders _testFolders;
        private readonly ScenarioContext _scenarioContext;

        public WebApiHooks(WebApiContext webApiContext, TestFolders testFolders, ScenarioContext scenarioContext)
        {
            _webApiContext = webApiContext;
            _testFolders = testFolders;
            _scenarioContext = scenarioContext;
        }

        [AfterScenario("@webapi")]
        public void WriteLog()
        {
            if (_scenarioContext.TestError != null)
            {
                var fileName = _testFolders.GetScenarioSpecificFileName(".log");
                _webApiContext.SaveLog(_testFolders.OutputFolder, fileName);
            }
        }

        [AfterTestRun]
        public static void StopApp()
        {
            AppHostingContext.StopApp();
        }
    }
}
