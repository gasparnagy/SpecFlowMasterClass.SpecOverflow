using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Assist.ValueComparers;

// ReSharper disable once CheckNamespace
namespace SpecFlowMasterClass.SpecOverflow.Specs.Support
{
    [Binding]
    public static class CurrentTimeProvider
    {
        class DateTimeWithNowComparer : IValueComparer
        {
            private readonly DateTimeValueComparer _baseComparer = new();

            public bool CanCompare(object actualValue) => _baseComparer.CanCompare(actualValue);

            public bool Compare(string expectedValue, object actualValue)
            {
                // This comparer enables to use "now" in data table comparisons (CompareToSet,
                // CompareToInstance). It could be improved by using a time service in the app
                
                if ("now".Equals(expectedValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    var nowDateTime = DateTime.Now;
                    var actualDateTime = (DateTime) actualValue;
                    var minutesDiffFromCurrentTestTime = Math.Abs(actualDateTime.Subtract(nowDateTime).TotalSeconds);
                    return minutesDiffFromCurrentTestTime <= 60;
                }
                return _baseComparer.Compare(expectedValue, actualValue);
            }
        }

        [BeforeTestRun]
        public static void InitValueComparers()
        {
            Service.Instance.ValueComparers.Register(new DateTimeWithNowComparer());
        }

        public static DateTime CurrentActionTime = DateTime.Now.AddMinutes(-100);

        public static DateTime GetActionTime()
        {
            CurrentActionTime = CurrentActionTime.AddMinutes(1);
            return CurrentActionTime;
        }
    }
}
