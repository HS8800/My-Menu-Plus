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

        [OutputCache(Duration = 30, VaryByParam = "content")]
        public ActionResult Index(int content = -1,int table = -1)
        {
            //payment controls
            BrainTree brain = new BrainTree(content);

            if (content == -1)
            {
                TempData["Error"] = "The menu you are looking for doesn't exist";
                return View("MenuNotFound");
            }

          
            var brainToken = brain.CreateClientToken();

            if (!brainToken.success)
            {
                TempData["Alert"] = "You just need to connect your BrainTree Account to your menu to take payments";
                TempData["Redirect"] = "/Keys?content=" + content;
                return RedirectToAction("Alert", "Braintree");
            }


            ViewData["ClientToken"] = brainToken.token;
           


            //menu componets
            ViewData["menuID"] = content;
            var menuComponents = MenuContentHelper.createMenuComponents(content);

            ViewData["title"] = menuComponents.title;
            ViewData["tags"] = menuComponents.tags;
            ViewData["menuSections"] = menuComponents.sections;
            ViewData["bannerImage"] = menuComponents.bannerImage;
            ViewData["menuNavigaton"] = menuComponents.menuNavigaton;
            ViewData["footer"] = menuComponents.footer;
        



            //editor button
            if (Session["id"] != null && AccountHelper.CanEditMenu(content, Convert.ToInt32(Session["id"]))) {
                ViewData["editButton"] = $@"
                    <div class='nav-button btn-effect' id='btn-edit-menu' data-id='{content}'>
                        <i class='fas fa-edit'></i>
                        <span id='login-text'>Edit</span>
                    </div>
                ";                   
            }


            //is menu taking orders
            var menuTimes = Helpers.MenuContentHelper.menuTimes(content);
            if (!menuTimes.isOpen)
            {
                ViewData["notOpenDisplay"] = "block";
            }
            else {
                ViewData["notOpenDisplay"] = "none";
            }

            ViewData["openMonday"] = menuTimes.menuTime.MondayOpen + "-" + menuTimes.menuTime.MondayClose;
            ViewData["openTuesday"] = menuTimes.menuTime.TuesdayOpen + "-" + menuTimes.menuTime.TuesdayClose;
            ViewData["openWednesday"] = menuTimes.menuTime.WednesdayOpen + "-" + menuTimes.menuTime.WednesdayClose;
            ViewData["openThursday"] = menuTimes.menuTime.ThursdayOpen + "-" + menuTimes.menuTime.ThursdayClose;
            ViewData["openFriday"] = menuTimes.menuTime.FridayOpen + "-" + menuTimes.menuTime.FridayClose;
            ViewData["openSaturday"] = menuTimes.menuTime.SaturdayOpen + "-" + menuTimes.menuTime.SaturdayClose;
            ViewData["openSunday"] = menuTimes.menuTime.SundayOpen + "-" + menuTimes.menuTime.SundayClose;



            return View();
        }

        
 

    }
}