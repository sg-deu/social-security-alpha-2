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

    public interface ICommand<T>
    {
        T Execute();
    }

    public abstract class Command<T> : IExecutable, ICommand<T>
    {
        protected static IRepository Repository { [DebuggerStepThrough] get { return DomainRegistry.Repository; } }

        public abstract T Execute();

        object IExecutable.Execute()
        {
            return Execute();
        }
    }
}