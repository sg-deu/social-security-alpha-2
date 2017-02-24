using System;
using System.Threading;

namespace FormUI.Tests.SystemTests.Util
{
    public static class Wait
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(20);
        private static          TimeSpan Timeout        = DefaultTimeout;

        public static void For(Action action)
        {
            For(Timeout, action);
        }

        public static void For(TimeSpan timeout, Action action)
        {
            var waitUntil = DateTime.Now + timeout;

            while (true)
            {
                try
                {
                    action();
                    Thread.Sleep(0);
                    return;
                }
                catch
                {
                    if (DateTime.Now > waitUntil)
                        throw;
                }
            }
        }
    }
}
