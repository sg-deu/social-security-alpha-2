using System.Web.Mvc;
using System.Web.Routing;

namespace FormUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // GOV.UK Verify routes
            // rather than get UKV to change their URLs, for time being, hardcode the controller in our route
            routes.MapRoute(
                name: "UKVerifyLoginReturn",
                url: "{controller}/login-return/{id}",
                defaults: new { controller = "Coc", action = "Identity", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "UKVerifyApplyReturn",
                url: "{controller}/apply-return/{id}",
                defaults: new { controller = "Bsg", action = "ApplicantDetails", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
