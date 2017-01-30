using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FormUI.Startup))]
namespace FormUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
