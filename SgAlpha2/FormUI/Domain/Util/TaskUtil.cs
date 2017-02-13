using System;
using System.Threading;
using System.Threading.Tasks;

namespace FormUI.Domain.Util
{
    public static class TaskUtil
    {
        /// <summary> await the task synchronously by using a separate thread to .Wait() for the task on </summary>
        public static void Await<T>(Func<Task<T>> taskFunc)
        {
            var thread = new Thread(() => { taskFunc().Wait(); });
            thread.Start();
            thread.Join();
        }
    }
}