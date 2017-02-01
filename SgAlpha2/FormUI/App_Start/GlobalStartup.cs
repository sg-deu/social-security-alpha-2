using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FormUI.App_Start
{
    public class GlobalStartup
    {
        public virtual void Init()
        {
            //GlobalFilters.Filters.Add(new Alpha2EntryFilter());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}