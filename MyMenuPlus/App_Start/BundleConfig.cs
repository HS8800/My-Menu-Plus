using System.Web;
using System.Web.Optimization;

namespace MyMenuPlus
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {



            //Keys Page
            bundles.Add(new StyleBundle("~/bundles/KeysPageStyles").Include(
                "~/Content/Home.css",
                 "~/Content/MenuSelection.css",
                 "~/Content/keys.css",
                 "~/Content/Tickbox.css"
                 ));

            bundles.Add(new ScriptBundle("~/bundles/KeysScripts").Include(
                  "~/Scripts/jquery-3.5.1.min.js",
                   "~/Scripts/Keys.js")); ; ;


            //OrderDisplay
            bundles.Add(new ScriptBundle("~/bundles/OrderDisplayScripts").Include(
                    "~/Scripts/jquery-3.5.1.min.js",
                    "~/Scripts/Orders.js"));


            //Success Page           
            bundles.Add(new StyleBundle("~/bundles/ResponseStyles").Include("~/Content/Response.css"));

            //Menu Editor
            bundles.Add(new StyleBundle("~/bundles/MenuEditorPageStyles").Include(
                    "~/Content/Home.css",
                     "~/Content/MenuSelection.css",
                    "~/Content/Notifications.css",
                    "~/Content/Menu.css",
                    "~/Content/MainScale.css",
                    "~/Content/SectionEditor.css"));

            bundles.Add(new ScriptBundle("~/bundles/MenuEditorPageScripts").Include(
                "~/Scripts/jquery-3.5.1.min.js",
                "~/Scripts/Notifications.js",
                 "~/Scripts/MenuEditor.js",
                 "~/Scripts/Currency.js"
             ));



       

            //Menu
            bundles.Add(new StyleBundle("~/bundles/MenuPageStyles").Include(
                    "~/Content/Scroll.css",
                    "~/Content/Menu.css",
                    "~/Content/MainScale.css",
                    "~/Content/TableSelection.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/MenuPageScripts").Include(
                 "~/Scripts/jquery-3.5.1.min.js",
                 "~/Scripts/Scroll.js",
                 "~/Scripts/MenuScale.js",
                 "~/Scripts/Menu.js",
                 "~/Scripts/Currency.js",
                 "~/Scripts/Basket.js",
                 "~/Scripts/TableSelection.js"
            ));



            //MenuSelection
            bundles.Add(new StyleBundle("~/bundles/MenuSelectionPageStyles").Include(
                "~/Content/Form.css",
                "~/Content/Home.css",
                "~/Content/Notifications.css",
                 "~/Content/MenuSelection.css"
             ));

            bundles.Add(new ScriptBundle("~/bundles/MenuSelectionPageScripts").Include(             
             "~/Scripts/jquery-3.5.1.min.js",
             "~/Scripts/Notifications.js",
             "~/Scripts/MenuSelection.js"
           ));


            //Home
            bundles.Add(new StyleBundle("~/bundles/HomePageStyles").Include(
               "~/Content/Home.css",
               "~/Content/Footer.css",
               "~/Content/Form.css",
               "~/Content/Notifications.css"
               ));

            bundles.Add(new ScriptBundle("~/bundles/HomePageScripts").Include(
                   "~/Scripts/jquery-3.5.1.min.js",
                   "~/Scripts/Form.js",
                   "~/Scripts/Notifications.js"));


            //Main Site Layout
            bundles.Add(new StyleBundle("~/bundles/MainSiteLayout").Include(
                        "~/Content/Aos.css",
                        "~/Content/Footer.css",
                        "~/Content/Navbar.css",
                        "~/Content/LoadingPage.css"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/MainSiteLayoutScripts").Include(
               "~/Scripts/Aos.js"
           ));

            

            //Menu Layout
            bundles.Add(new StyleBundle("~/bundles/MenuLayoutStyles").Include(
                    "~/Content/Footer.css",
                    "~/Content/MenuNavbar.css",
                    "~/Content/Form.css",
                    "~/Content/Brand.css",
                    "~/Content/Notifications.css",
                    "~/Content/LoadingPage.css"
                    ));


            bundles.Add(new ScriptBundle("~/bundles/MenuLayoutScripts").Include(
                "~/Scripts/jquery-3.5.1.min.js",
                "~/Scripts/Notifications.js",
                "~/Scripts/Form.js"                           
            ));


        }
    }
}
