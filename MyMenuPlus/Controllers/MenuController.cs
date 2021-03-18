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


        public ActionResult Index(int content)
        {
            ViewData["menuID"] = content;

            //NEED TO REMOVE owener ID/accountID requirements
            var menuComponents = MenuContentHelper.createMenuComponents(content, Convert.ToInt32(Session["id"]));

            ViewData["title"] = menuComponents.title;
            ViewData["tags"] = menuComponents.tags;
            ViewData["menuSections"] = menuComponents.sections;
            ViewData["bannerImage"] = menuComponents.bannerImage;
            ViewData["menuNavigaton"] = menuComponents.menuNavigaton;

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