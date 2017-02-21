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
                    var iExecutable = executable as IExecutable;
                    return iExecutable.Execute();
                }
                finally
                {
                    DomainRegistry.ValidationContext = null;
                    DomainRegistry.Repository = null;
                }
            }
        }
    }
}