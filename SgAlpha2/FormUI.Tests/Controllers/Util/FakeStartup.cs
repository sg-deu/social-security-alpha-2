using FormUI.App_Start;

namespace FormUI.Tests.Controllers.Util
{
    public class FakeStartup : GlobalStartup
    {
        public override void Init()
        {
            // we can change the startup for the unit tests here
            base.Init();
        }
    }
}
