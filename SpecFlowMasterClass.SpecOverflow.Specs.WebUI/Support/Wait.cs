using System;
using System.Threading;

namespace SpecFlowMasterClass.SpecOverflow.Specs.WebUI.Support
{
    /// <summary>
    /// Simple implementation of a busy-waiting strategy that waits for an assertion to succeed for a certain time.
    /// </summary>
    public static class Wait
    {
        private const int ACTIVE_WAIT_TIMEOUT_MSEC = 2000;
        private const int ACTIVE_WAIT_POLL_PERIOD_MSEC = 100;

        public static void For(Action action)
        {
            var waitUntil = DateTime.Now + TimeSpan.FromMilliseconds(ACTIVE_WAIT_TIMEOUT_MSEC);
            while (true)
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception)
                {
                    if (DateTime.Now >= waitUntil)
                        throw;
                }
                Thread.Sleep(ACTIVE_WAIT_POLL_PERIOD_MSEC);
            }
        }

        public static bool ForCondition(Func<bool> condition)
        {
            var waitUntil = DateTime.Now + TimeSpan.FromMilliseconds(ACTIVE_WAIT_TIMEOUT_MSEC);
            while (true)
            {
                if (condition())
                    return true;
                if (DateTime.Now >= waitUntil)
                    return false;
                Thread.Sleep(ACTIVE_WAIT_POLL_PERIOD_MSEC);
            }
        }
    }
}
