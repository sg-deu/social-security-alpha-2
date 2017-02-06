using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FormUI.App_Start
{
    public class GlobalStartup
    {
        public virtual void Init()
        {
            Alpha2LoggingFilter.Info("GlobalStartup.Init()");

            FeatureFolders.Register(ViewEngines.Engines);
            GlobalFilters.Filters.Add(new Alpha2EntryFilter());
            GlobalFilters.Filters.Add(new Alpha2LoggingFilter());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}