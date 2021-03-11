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
                ViewData["menuThumbnails"] = MenuContentHelper.MenuThumbnails(Session["email"].ToString());
            }
            else {
                return RedirectToAction("Index","Home");
            }
            

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public string CreateMenu(string menuName)
        {

            ResponseModel response = new ResponseModel();
            response.operation = "attempting to create a new menu";

            if (Convert.ToString(Session["email"]) != "")
            {
                var CreateMenu = MenuContentHelper.CreateMenu(menuName, Convert.ToString(Session["email"]));
                if (CreateMenu.success)
                {
                    response.response = "success";
                }
                else 
                {
                    response.response = "failed";
                    response.error = CreateMenu.details;
                }
            }
            else 
            {
                response.response = "failed";
                response.error = "This action requires you to login";
            }



            return JsonConvert.SerializeObject(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public string DeleteMenu(string menuID)
        {
            ResponseModel response = new ResponseModel();
            response.operation = "attempting to delete a menu";

            if (Convert.ToString(Session["email"]) != "")
            {
                var DeleteMenu = MenuContentHelper.DeleteMenu(menuID, Convert.ToString(Session["email"]));
                if (DeleteMenu.success)
                {
                    response.response = "success";
                }
                else
                {
                    response.response = "failed";
                    response.error = DeleteMenu.details;
                }
            }
            else
            {
                response.response = "failed";
                response.error = "This action requires you to login";
            }

            return JsonConvert.SerializeObject(response);
        }

    }
}