using MyMenuPlus.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MyMenuPlus.Controllers
{
    public class MenuController : Controller
    {


        public ActionResult Index(int content = -1,int table = -1)
        {
            BrainTree brain = new BrainTree(content);
            
            if (content == -1) {
                TempData["Error"] = "The menu you are looking for doesn't exist";
                return View("MenuNotFound");
            }

            var brainToken = brain.CreateClientToken();

            if (!brainToken.success) {
                @TempData["Alert"] = "You just need to connect your BrainTree Account to your menu to take payments";
                @TempData["Redirect"] = "/Keys?content="+content;
                return RedirectToAction("Alert", "Braintree");
            }

            ViewData["ClientToken"] = brainToken.token;


            ViewData["menuID"] = content;
            var menuComponents = MenuContentHelper.createMenuComponents(content);

            ViewData["title"] = menuComponents.title;
            ViewData["tags"] = menuComponents.tags;
            ViewData["menuSections"] = menuComponents.sections;
            ViewData["bannerImage"] = menuComponents.bannerImage;
            ViewData["menuNavigaton"] = menuComponents.menuNavigaton;
            ViewData["footer"] = menuComponents.footer;


            if (Session["id"] != null && AccountHelper.CanEditMenu(content, Convert.ToInt32(Session["id"]))) {
                ViewData["editButton"] = $@"
                    <div class='nav-button btn-effect' id='btn-edit-menu' data-id='{content}'>
                        <i class='fas fa-edit'></i>
                        <span id='login-text'>Edit</span>
                    </div>
                ";                   
            }
           
            return View();
        }

        
 

    }
}