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

        /// <summary> await the result synchronously by using a separate thread to wait for the .Result on </summary>
        public static T Result<T>(Func<Task<T>> taskFunc)
        {
            T result = default(T);
            var thread = new Thread(() => { result = taskFunc().Result; });
            thread.Start();
            thread.Join();
            return result;
        }
    }
}