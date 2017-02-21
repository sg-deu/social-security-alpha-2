using FormUI.Domain.Util.Facade;

namespace FormUI.Controllers.Shared
{
    public class PresentationRegistry
    {
        public delegate CqExecutor ExecutorFactory(bool modelIsValid);

        public static ExecutorFactory NewExecutor;
    }
}