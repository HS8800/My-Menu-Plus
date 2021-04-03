using MyMenuPlus.Helpers;
using MyMenuPlus.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMenuPlus.Controllers
{
    public class MenuEditorController : Controller
    {


        [HttpPost]//send as form-data (no content type needed)
        [ValidateAntiForgeryToken]
        public string UpdateMenu(int menuID, string menuImage, string menuData)
        {
            ResponseModel response = new ResponseModel();
            response.operation = "attempting to update menu";

            if (Session["id"] != null)
            {

                var update = MenuContentHelper.updateMenu(menuID, Convert.ToInt32(Session["id"]), menuData, menuImage);

                if (update.success)
                {
                    response.response = "success";
                }
                else
                {
                    response.response = "failed";
                    response.error = "Could not update the menu at this time";
                }

                return JsonConvert.SerializeObject(response);
            }

            response.response = "failed";
            response.error = "User must be logged in";
            return JsonConvert.SerializeObject(response);

        }


    

        public ActionResult Index(int content)
        {
            if (Session["id"] != null)
            {
                var menuComponents = MenuContentHelper.createMenuEditorComponents(content);

                if (menuComponents.success)
                {
                    ViewData["title"] = menuComponents.title;
                    ViewData["tags"] = menuComponents.tags;
                    ViewData["sections"] = menuComponents.sections;
                    ViewData["bannerImage"] = menuComponents.bannerImage;
                    ViewData["footer"] = menuComponents.footer;
                    

                    ViewData["menuID"] = content;
                    return View();
                }
                else {
                    ViewData["error"] = "Menu components couldn't be loaded at this time";
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }         
        }

    }
}
