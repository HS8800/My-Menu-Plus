using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMenuPlus.Controllers
{
    public class KeysController : Controller
    {

        public ActionResult Index(int content)
        {

            if (Session["id"] != null)
            {
                ViewData["menuID"] = content;
                //var menuComponents = MenuContentHelper.createMenuEditorComponents(content, Convert.ToInt32(Session["id"]));

                //if (menuComponents.success)
                //{
                //    ViewData["title"] = menuComponents.title;
                //    ViewData["tags"] = menuComponents.tags;
                //    ViewData["sections"] = menuComponents.sections;
                //    ViewData["bannerImage"] = menuComponents.bannerImage;

                //    ViewData["menuID"] = content;
                //    return View();
                //}
                //else
                //{
                //    ViewData["error"] = "Menu components couldn't be loaded at this time";
                //    return View("~/Views/Shared/Error.cshtml");
                //}
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}