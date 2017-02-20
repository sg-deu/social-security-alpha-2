using System.Diagnostics;
using FormUI.Domain.Util.Facade;

namespace FormUI.Domain.Util
{
    public abstract class Command : IExecutable
    {
        protected static IRepository Repository { [DebuggerStepThrough] get { return DomainRegistry.Repository; } }

        public abstract void Execute();

        object IExecutable.Execute()
        {
            Execute();
            return null;
        }
    }

    public abstract class Command<T> : IExecutable
    {
        protected static IRepository Repository { [DebuggerStepThrough] get { return DomainRegistry.Repository; } }

        public abstract T Execute();

        object IExecutable.Execute()
        {
            return Execute();
        }
    }
}