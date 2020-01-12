using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.WebPages;

namespace Appointment.Web.Infrastructure.Bundles
{
    public static class BundleCollectionExtensions
    {
        private static readonly DefaultBundleOrderer _orderer;

        static BundleCollectionExtensions()
        {
            _orderer = new DefaultBundleOrderer();
        }

        public static IHtmlString RenderScripts(this HtmlHelper helper, params string[] filePaths)
        {
            return RenderScripts(helper, true, filePaths);
        }

        /// <param name="isMinified">Set to false if you want to disable minification in bundle</param>
        public static IHtmlString RenderScripts(this HtmlHelper helper, bool isMinified, params string[] filePaths)
        {
            //Use WebPageBase VirtualPath, because ViewPath returns the path to the parent view even if there are nested views, which inserts duplicated bundle names in the BundleTable.
            var path = (helper.ViewDataContainer as WebPageBase).VirtualPath.Replace("~/", "~/Scripts/").Replace(".cshtml", "");
            var bundle = new ScriptBundle(path).Include(filePaths);
            bundle.Orderer = _orderer;
            if (!isMinified)
            {
                bundle.Transforms.Clear();
            }
            BundleTable.Bundles.Add(bundle);
            return Scripts.Render(path);
        }

        public static IHtmlString RenderStyles(this HtmlHelper helper, params string[] filePaths)
        {
            return RenderStyles(helper, true, filePaths);
        }

        /// <param name="isMinified">Set to false if you want to disable minification in bundle</param>
        public static IHtmlString RenderStyles(this HtmlHelper helper, bool isMinified, params string[] filePaths)
        {
            var path = (helper.ViewDataContainer as WebPageBase).VirtualPath.Replace("~/", "~/Content/").Replace(".cshtml", "");
            var bundle = new StyleBundle(path).Include(filePaths);
            bundle.Orderer = _orderer;
            if (!isMinified)
            {
                bundle.Transforms.Clear();
            }
            BundleTable.Bundles.Add(bundle);
            return Styles.Render(path);
        }

    }
}