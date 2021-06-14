using System;
using TechTalk.SpecFlow;

namespace SpecFlowMasterClass.SpecOverflow.Specs.WebUI.Support
{
    [Binding]
    public class AppHostingHooks
    {
        [AfterTestRun]
        public static void StopApp()
        {
            AppHostingContext.StopApp();
        }
    }
}
