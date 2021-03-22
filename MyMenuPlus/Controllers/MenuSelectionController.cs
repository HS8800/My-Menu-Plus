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

            if (Session["id"] != null)
            {
                ViewData["menuThumbnails"] = MenuContentHelper.MenuThumbnails(Convert.ToInt32(Session["id"]));
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

            if (Session["id"] != null)
            {
                var CreateMenu = MenuContentHelper.CreateMenu(menuName, Convert.ToInt32(Session["id"]));
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
        public string DeleteMenu(int menuID)
        {
            ResponseModel response = new ResponseModel();
            response.operation = "attempting to delete a menu";

            if (Session["id"] != null)
            {
                var DeleteMenu = MenuContentHelper.deleteMenu(menuID, Convert.ToInt32(Session["id"]));
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