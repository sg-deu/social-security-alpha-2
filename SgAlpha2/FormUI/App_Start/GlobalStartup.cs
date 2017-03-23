using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FormUI.Controllers.Shared;
using FormUI.Domain.Util;
using FormUI.Domain.Util.Facade;

namespace FormUI.App_Start
{
    public class GlobalStartup
    {
        public virtual void Init()
        {
            LoggingFilter.Info("GlobalStartup.Init()");

            FeatureFolders.Register(ViewEngines.Engines);
            GlobalFilters.Filters.Add(new EntryFilter());
            GlobalFilters.Filters.Add(new LoggingFilter());

            Binder.Register();
            ModelMetadataProviders.Current = new MetadataProvider();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            InitRepository();
            InitCloudStore();
            InitExecutor();
        }

        protected virtual void InitRepository()
        {
            var dbUri = new Uri(ConfigurationManager.AppSettings["dbUri"]);
            var dbKey = ConfigurationManager.AppSettings["dbKey"];

            Repository.Init(dbUri, dbKey);
        }

        protected virtual void InitCloudStore()
        {
            var storageConnectionString = ConfigurationManager.AppSettings["storageConnectionString"];
            var storageName = ConfigurationManager.AppSettings["storageName"];

            CloudStore.Init(storageConnectionString, storageName);
        }

        protected virtual void InitExecutor()
        {
            PresentationRegistry.NewExecutor = isValid =>
                new CqExecutor(
                    new DomainExecutor(isValid)
                );
        }
    }
}