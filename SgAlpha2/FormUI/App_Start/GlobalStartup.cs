using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FormUI.Domain.Util;

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

            Alpha2Binder.Register();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            InitRepository();
        }

        protected virtual void InitRepository()
        {
            var dbUri = new Uri(ConfigurationManager.AppSettings["dbUri"]);
            var dbKey = ConfigurationManager.AppSettings["dbKey"];

            Repository.Init(dbUri, dbKey);
        }
    }
}