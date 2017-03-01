using System.Diagnostics;
using FormUI.Domain.Util.Facade;

namespace FormUI.Domain.Util
{
    public interface IQuery<T>
    {
        T Find();
    }

    public abstract class Query<T> : IExecutable, IQuery<T>
    {
        protected static IRepository Repository { [DebuggerStepThrough] get { return DomainRegistry.Repository; } }

        public abstract T Find();

        public object Execute()
        {
            return Find();
        }
    }
}