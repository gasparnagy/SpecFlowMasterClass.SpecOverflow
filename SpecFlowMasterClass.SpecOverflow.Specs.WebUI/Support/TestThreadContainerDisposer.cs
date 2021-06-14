using System;
using System.Collections.Concurrent;
using BoDi;
using TechTalk.SpecFlow;

namespace SpecFlowMasterClass.SpecOverflow.Specs.WebUI.Support
{
    //[Binding]
    //public class TestThreadContainerDisposer
    //{
    //    private static ConcurrentBag<IObjectContainer> _testThreadContainers = new();

    //    [BeforeScenario(Order = -1)]
    //    public void EnsureTestThreadContainerRegistered(ScenarioContext scenarioContext)
    //    {
    //        var testThreadContainer = scenarioContext.ScenarioContainer.Resolve<TestThreadContext>().TestThreadContainer;
    //        _testThreadContainers.Add(testThreadContainer);
    //    }

    //    [AfterTestRun]
    //    public static void DisposeTestThreadContainers()
    //    {
    //        var containers = _testThreadContainers.ToArray();
    //        foreach (var container in containers)
    //        {
    //            container.Dispose();
    //        }
    //    }
    //}
}
