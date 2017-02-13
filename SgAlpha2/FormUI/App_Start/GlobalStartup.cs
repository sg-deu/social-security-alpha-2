using System;
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
            // this only work on local machine for now
            var localDbUri = new Uri("https://localhost:8081");
            var localDbKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

            Repository.Init(localDbUri, localDbKey);
        }
    }
}