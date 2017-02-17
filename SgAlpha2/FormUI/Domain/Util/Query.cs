using FormUI.Domain.Util.Facade;

namespace FormUI.Domain.Util
{
    public abstract class Query<T> : IExecutable
    {
        public abstract T Find();

        public object Execute()
        {
            return Find();
        }
    }
}