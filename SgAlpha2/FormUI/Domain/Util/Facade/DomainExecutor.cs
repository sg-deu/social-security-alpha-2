namespace FormUI.Domain.Util.Facade
{
    public class DomainExecutor : IExecutor
    {
        private bool _isValid;

        public DomainExecutor(bool isValid)
        {
            _isValid = isValid;
        }

        public object Execute(object executable)
        {
            using (var repository = Repository.New())
            {
                try
                {
                    DomainRegistry.Repository = repository;
                    DomainRegistry.ValidationContext = new ValidationContext(_isValid);
                    DomainRegistry.CloudStore = CloudStore.New();
                    var iExecutable = executable as IExecutable;
                    return iExecutable.Execute();
                }
                finally
                {
                    DomainRegistry.CloudStore = null;
                    DomainRegistry.ValidationContext = null;
                    DomainRegistry.Repository = null;
                }
            }
        }
    }
}