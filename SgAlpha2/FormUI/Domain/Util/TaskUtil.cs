using System;
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
            // taken from:  http://stackoverflow.com/a/17094801/357728
            try
            {
                var t = Task.Run(taskFunc);
                return t.Result;
            }
            catch (AggregateException ae)
            {
                throw ae.GetBaseException();
            }
        }
    }
}