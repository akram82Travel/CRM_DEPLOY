using System.Web;
using System.Web.Optimization;

namespace CRMSTUBSOFT
{
    public class BundleConfig
    {
        // Pour plus d'informations sur le regroupement, visitez https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Content/js/vendors.min.js",
                        "~/Content/js/bootstrap-switch.min.js",
                        "~/Content/js/switchery.min.js",
                        "~/Content/js/bootstrap-checkbox.min.js",
                        "~/Content/js/selectize.min.js",
                        "~/Content/js/form-selectize.min.js",
                        "~/Content/js/picker.js",
                        "~/Content/js/picker.date.js",
                        "~/Content/Vendors/DataTable/js/jquery.dataTables.min.js",
                        "~/Content/Vendors/DataTable/js/dataTables.bootstrap4.min.js",
                        "~/Content/Vendors/Chart/chartist.min.js",
                        "~/Content/Vendors/Chart/chartist-plugin-tooltip.min.js",
                        "~/Content/Vendors/Chart/raphael-min.js",
                        "~/Content/Vendors/Chart/morris.min.js",
                        "~/Content/Vendors/Chart/horizontal-timeline.js",
                        "~/Content/js/app-menu.min.js",
                        "~/Content/js/app.min.js",
                        "~/Content/js/customizer.min.js",
                        "~/Content/js/footer.min.js",
                        "~/Content/js/select2.full.min.js",
                        "~/Content/js/bootstrap-select.min.js",
                        "~/Content/js/jquery-ui.min.js",
                        "~/Content/js/moment.min.js",
                        "~/Content/js/fullcalendar.min.js",
                        "~/Content/js/sweetalert.min.js",
                        "~/Content/js/switch.min.js",
                        "~/Content/js/form-select2.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilisez la version de développement de Modernizr pour développer et apprendre. Puis, lorsque vous êtes
            // prêt pour la production, utilisez l'outil de génération à l'adresse https://modernizr.com pour sélectionner uniquement les tests dont vous avez besoin.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
