using System.Web.Optimization;

namespace Appointment.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/jquery.validate*",
                    "~/Scripts/moment.js",
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/bootstrap-datetimepicker.js",
                    "~/Scripts/vue.js",
                    "~/Scripts/common.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/main/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/bootstrap-datetimepicker.css",
                    "~/Content/site.css"));
        }
    }
}
