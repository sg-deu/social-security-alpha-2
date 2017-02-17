namespace FormUI.Domain.Util.Facade
{
    public class CqExecutor
    {
        private IExecutor _executor;

        public CqExecutor(IExecutor executor)
        {
            _executor = executor;
        }

        public void Execute(Command cmd)
        {
            _executor.Execute(cmd);
        }

        public TReturn Execute<TReturn>(Command<TReturn> cmd)
        {
            return (TReturn)_executor.Execute(cmd);
        }

        public TReturn Execute<TReturn>(Query<TReturn> query)
        {
            return (TReturn)_executor.Execute(query);
        }
    }
}