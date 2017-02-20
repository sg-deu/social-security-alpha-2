using System.Diagnostics;
using FormUI.Domain.Util.Facade;

namespace FormUI.Domain.Util
{
    public abstract class Query<T> : IExecutable
    {
        protected static IRepository Repository { [DebuggerStepThrough] get { return DomainRegistry.Repository; } }

        public abstract T Find();

        public object Execute()
        {
            return Find();
        }
    }
}