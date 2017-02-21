using FormUI.App_Start;
using FormUI.Controllers.Shared;
using FormUI.Domain.Util.Facade;

namespace FormUI.Tests.Controllers.Util
{
    public class FakeStartup : GlobalStartup
    {
        public override void Init()
        {
            // we can change the startup for the unit tests here
            base.Init();
        }

        protected override void InitRepository()
        {
            // repository should not be used in web-tests,
            // and domain tests initialise a local repository
        }

        protected override void InitExecutor()
        {
            // executor is based on WebTest executor that has been setup
            PresentationRegistry.NewExecutor = isValid => new CqExecutor(WebTest.ExecutorStub);
        }
    }
}
