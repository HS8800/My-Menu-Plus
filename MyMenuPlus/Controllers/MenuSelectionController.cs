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
    public class MenuSelectionController : Controller
    {
        public ActionResult Index()
        {

            if (Session["email"] != null)
            {
                ViewData["menuThumbnails"] = MenuContentHelper.menuThumbnails(Session["email"].ToString());
            }
            else {
                return RedirectToAction("Index","Home");
            }
            

            return View();
        }

    }
}