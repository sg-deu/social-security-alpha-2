using FormUI.Domain.Util.Facade;

namespace FormUI.Domain.Util
{
    public abstract class Command : IExecutable
    {
        public abstract void Execute();

        object IExecutable.Execute()
        {
            Execute();
            return null;
        }
    }

    public abstract class Command<T> : IExecutable
    {
        public abstract T Execute();

        object IExecutable.Execute()
        {
            return Execute();
        }
    }
}