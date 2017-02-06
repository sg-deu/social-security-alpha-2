using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FormUI.App_Start
{
    public class GlobalStartup
    {
        public virtual void Init()
        {
            Trace.WriteLine("Alpha2::Trace.WriteLine() GlobalStartup.Init()");
            Trace.TraceError("Alpha2::Trace.TraceError() GlobalStartup.Init()");
            Trace.TraceInformation("Alpha2::Trace.TraceInformation() GlobalStartup.Init()");
            Trace.TraceWarning("Alpha2::Trace.TraceWarning() GlobalStartup.Init()");

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