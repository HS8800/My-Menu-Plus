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
            ViewData["menuID"] = content;
            return View();
        }

    }
}
