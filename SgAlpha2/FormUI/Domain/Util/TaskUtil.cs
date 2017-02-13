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
            Result(taskFunc);
        }

        /// <summary> await the result synchronously by using a separate thread to wait for the .Result on </summary>
        public static T Result<T>(Func<Task<T>> taskFunc)
        {
            T result = default(T);
            Exception threadException = null;
            var thread = new Thread(() =>
            {
                try
                {
                    result = taskFunc().Result;
                }
                catch (Exception ex)
                {
                    threadException = ex;
                }
            });
            thread.Start();
            thread.Join();

            var aggregateException = threadException as AggregateException;

            if (aggregateException != null)
                throw aggregateException.GetBaseException();

            if (threadException != null)
                throw threadException;

            return result;
        }
    }
}