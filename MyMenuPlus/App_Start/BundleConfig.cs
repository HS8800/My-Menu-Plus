using System.Web;
using System.Web.Optimization;

namespace MyMenuPlus
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/Jquery").Include(
                    "~/Scripts/jquery-3.5.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/MenuScripts").Include(
                        "~/Scripts/Scroll.js",
                        "~/Scripts/MenuScale.js"));

            bundles.Add(new ScriptBundle("~/bundles/Notifications").Include(
                       "~/Scripts/Notifications.js"));

            bundles.Add(new ScriptBundle("~/bundles/Form").Include(
                      "~/Scripts/Form.js"));

            bundles.Add(new ScriptBundle("~/bundles/MenuEditor").Include(
                      "~/Scripts/MenuEditor.js",
                      "~/Scripts/Currency.js"));



            bundles.Add(new ScriptBundle("~/bundles/Aos").Include(
                    "~/Scripts/Aos.js"));

            bundles.Add(new StyleBundle("~/bundles/AosStyles").Include(
                        "~/Content/Aos.css"));

            bundles.Add(new StyleBundle("~/bundles/Home").Include(
                        "~/Content/Home.css",
                        "~/Content/Footer.css",
                        "~/Content/Form.css",
                        "~/Content/Notifications.css"
                        ));


            bundles.Add(new StyleBundle("~/bundles/EditorStyles").Include(
                        "~/Content/SectionEditor.css"));

            bundles.Add(new StyleBundle("~/bundles/MenuStyles").Include(
                        "~/Content/Scroll.css",
                        "~/Content/Menu.css",
                        "~/Content/MainScale.css"));

            bundles.Add(new StyleBundle("~/bundles/DefaultMenuStyles").Include(
                        "~/Content/Footer.css",
                        "~/Content/MenuNavbar.css",
                        "~/Content/Form.css",
                        "~/Content/Brand.css",
                        "~/Content/Notifications.css"
                        ));

        }
    }
}
