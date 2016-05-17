using System.Web.Optimization;

namespace BSSiseveeb.Public.Web
{
    public static class ScriptBundles
    {
        public const string Jquery = "~/bundles/jquery";
        public const string Jqueryval = "~/bundles/jqueryval";
        public const string Modernizr = "~/bundles/modernizr";
        public const string Bootstrap = "~/bundles/bootstrap";

        /* Sections */
        public const string Calendar = "~/bundles/calendar";
        public const string Requests = "~/bundles/requests";
        public const string Workers = "~/bundles/workers";
        public const string AdminUsers = "~/bundles/adminusers";
        public const string AdminRequests = "~/bundles/adminrequests";
        public const string AdminVacations = "~/bundles/adminvacations";
        public const string Account = "~/bundles/account";

    }

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif

            bundles.Add(new ScriptBundle(ScriptBundles.Jquery).Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle(ScriptBundles.Jqueryval).Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle(ScriptBundles.Modernizr).Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle(ScriptBundles.Bootstrap).Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle(ScriptBundles.Calendar).Include(
                    Links.Bundles.Scripts.Assets.bootstrap_datepicker_js,
                    Links.Bundles.Scripts.Assets.general_js,
                    Links.Bundles.Scripts.Assets.calendar_js
                ));

            bundles.Add(new ScriptBundle(ScriptBundles.Requests).Include(
                    Links.Bundles.Scripts.Assets.requests_js,
                    Links.Bundles.Scripts.Assets.general_js
                ));

            bundles.Add(new ScriptBundle(ScriptBundles.Workers).Include(
                    Links.Bundles.Scripts.Assets.general_js
                ));

            bundles.Add(new ScriptBundle(ScriptBundles.AdminVacations).Include(
                    Links.Bundles.Scripts.Assets.bootstrap_datepicker_js,
                    Links.Bundles.Scripts.Assets.general_js,
                    Links.Bundles.Scripts.Assets.adminvacations_js
                ));

            bundles.Add(new ScriptBundle(ScriptBundles.AdminUsers).Include(
                    Links.Bundles.Scripts.Assets.bootstrap_datepicker_js,
                    Links.Bundles.Scripts.Assets.addemployee_js,
                    Links.Bundles.Scripts.Assets.general_js,
                    Links.Bundles.Scripts.Assets.jquery_birthday_picker_js
                ));

            bundles.Add(new ScriptBundle(ScriptBundles.AdminRequests).Include(
                    Links.Bundles.Scripts.Assets.adminrequests_js,
                    Links.Bundles.Scripts.Assets.general_js,
                    Links.Bundles.Scripts.Assets.bootstrap_datepicker_js
                ));

            bundles.Add(new ScriptBundle(ScriptBundles.Account).Include(
                    Links.Bundles.Scripts.Assets.jquery_birthday_picker_js,
                    Links.Bundles.Scripts.Assets.general_js,
                    Links.Bundles.Scripts.Assets.account_js
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                Links.Bundles.Content.Assets.bootstrap_css,
                Links.Bundles.Content.Assets.Site_css));
        }
    }
}
