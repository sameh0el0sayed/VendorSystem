
using System.Web;
using System.Web.Optimization;

namespace VendorSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                     "~/Scripts/jquery-1.10.2.min.js",
                     "~/Scripts/bootstrap.min.js",
                     "~/Scripts/jquery-ui.min.js",
                     "~/Scripts/jquery.ui.touch-punch.min.js",
                     "~/Scripts/moment.min.js",
                     "~/Scripts/bootstrap-datepicker.min.js",
                     "~/Scripts/bootstrap-datetimepicker.min.js",
                     "~/Scripts/daterangepicker.min.js",
                     "~/Scripts/bootstrap-colorpicker.min.js",
                     "~/Scripts/jquery.easypiechart.min.js",
                     "~/Scripts/jquery.sparkline.index.min.js",
                     "~/Scripts/jquery.flot.min.js",
                     "~/Scripts/jquery.flot.pie.min.js",
                     "~/Scripts/jquery.sparkline.index.min.js",
                    // "~/Scripts/jquery.flot.min.js",
                     "~/Scripts/jquery.flot.resize.min.js",
                     "~/Scripts/ace-elements.min.js",
                     "~/Scripts/ace.min.js",
                     "~/Scripts/DataTable/jquery.dataTables.min.js",
                     //"~/Scripts/DataTable/sum.js",
                     "~/Scripts/select2.min.js",
                     "~/Scripts/bootstrap-notify.min.js",
                     "~/Scripts/ScriptsViews/SharedJS.js"
                       ));
          


            bundles.Add(new StyleBundle("~/Content/css").Include(
                       "~/Content/bootstrap.min.css",
                       "~/Content/bootstrap-datepicker3.min.css",
                       "~/Content/bootstrap-timepicker.min.css",
                       "~/Content/daterangepicker.min.css",
                       "~/Content/bootstrap-datetimepicker.min.css",
                       "~/Content/bootstrap-colorpicker.min.css",
                       "~/Content/font-awesome.min.css",
                       "~/Content/jquery-ui.min.css",                   
                       "~/Content/fonts.googleapis.com.css",
                       "~/Content/ace.min.css",
                       "~/Content/ace-skins.min.css",
                       "~/Content/ace-rtl.min.css",
                       "~/Content/DataTable/jquery.dataTables.min.css",
                       "~/Content/select2.min.css","~/Content/ViewsStyle/SharedCSS.css",
                        "~/Content/font-awesome/4.5.0/css/font-awesome.min.css"
                        ,"~/Content/animate.css"
                       )
                       );
        }
    }
}
