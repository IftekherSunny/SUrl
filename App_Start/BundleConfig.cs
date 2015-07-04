using System.Web;
using System.Web.Optimization;

namespace SUrl
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //My JavaScript
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.7.1.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/modernizr.js"
                        ));


            //My StyleSheet
            bundles.Add(new StyleBundle("~/Content/themes/base/css/bundles").Include(
                        "~/Content/themes/base/css/font-awesome.css",
                        "~/Content/themes/base/css/bootstrap.css",
                        "~/Content/themes/base/css/style.css"
                        ));

           
             BundleTable.EnableOptimizations = true;
        }
    }
}