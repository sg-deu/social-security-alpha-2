namespace FormUI.Domain.Util.Facade
{
    public class DomainExecutor : IExecutor
    {
        public object Execute(object executable)
        {
            using (var repository = Repository.New())
            {
                try
                {
                    DomainRegistry.Repository = repository;
                    var iExecutable = executable as IExecutable;
                    return iExecutable.Execute();
                }
                finally
                {
                    DomainRegistry.Repository = null;
                }
            }
        }
    }
}