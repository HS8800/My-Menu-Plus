using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace My_Menu_Plus
{
    public class BundleConfig
    {
        // For more information on Bundling, visit https://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Jquery").Include(
                    "~/Scripts/jquery-3.5.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/MenuScripts").Include(
                        "~/Scripts/Scroll.js",
                        "~/Scripts/MenuScale.js"));

            bundles.Add(new ScriptBundle("~/bundles/Notifications").Include(
                       "~/Scripts/Notifications.js"));

            bundles.Add(new ScriptBundle("~/bundles/Login").Include(
                      "~/Scripts/Login.js"));

            bundles.Add(new StyleBundle("~/bundles/MenuStyles").Include(
                        "~/Content/Scroll.css",
                        "~/Content/Menu.css",
                        "~/Content/MainScale.css"));

            bundles.Add(new StyleBundle("~/bundles/DefaultStyles").Include(
                        "~/Content/Footer.css",
                        "~/Content/Navbar.css",
                        "~/Content/Login.css",
                        "~/Content/Brand.css",
                        "~/Content/Notifications.css"
                        ));


        }
    }
}