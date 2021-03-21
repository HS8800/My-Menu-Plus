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

        //public ActionResult Index() //Need an oops, looks like you took a wrong turn
        //{
        //    return View("MenuNotFound");
        //}

        BrainTree brain = new BrainTree();

        public ActionResult Index(int content)
        {
            ViewData["menuID"] = content;
            var menuComponents = MenuContentHelper.createMenuComponents(content);

            ViewData["title"] = menuComponents.title;
            ViewData["tags"] = menuComponents.tags;
            ViewData["menuSections"] = menuComponents.sections;
            ViewData["bannerImage"] = menuComponents.bannerImage;
            ViewData["menuNavigaton"] = menuComponents.menuNavigaton;

            ViewData["ClientToken"] = brain.CreateClientToken();

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