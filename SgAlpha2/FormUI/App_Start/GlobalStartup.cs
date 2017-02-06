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
            Trace.WriteLine("Trace.WriteLine() GlobalStartup.Init()");
            Trace.TraceError("Trace.TraceError() GlobalStartup.Init()");
            Trace.TraceInformation("Trace.TraceInformation() GlobalStartup.Init()");
            Trace.TraceWarning("Trace.TraceWarning() GlobalStartup.Init()");

            FeatureFolders.Register(ViewEngines.Engines);
            GlobalFilters.Filters.Add(new Alpha2EntryFilter());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}