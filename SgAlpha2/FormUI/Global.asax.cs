using FormUI.App_Start;

namespace FormUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static GlobalStartup Startup = new GlobalStartup();

        protected void Application_Start()
        {
            Startup.Init();
        }
    }
}
