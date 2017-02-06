using System.Web.Optimization;

namespace FormUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/mygov.scot.global.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/mygov.scot.main.css",
                      "~/Content/sg.css"));

            // attempt to use mygov-assets.css generated from source code here:  https://github.com/scottishgovernment/mygov-assets
            // however this relies on mygov.scot.main.partial.css cos the header&footer and the button sizes didn't work
            bundles.Add(new StyleBundle("~/Content/myGovCss").Include(
                      "~/Content/styles/mygov-assets.css",
                      "~/Content/mygov.scot.main.partial.css",
                      "~/Content/sg.css"));
        }
    }
}
