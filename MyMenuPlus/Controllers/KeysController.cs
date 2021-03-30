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
    public class KeysController : Controller
    {
        [HttpPost]
        public string GenerateKey(int content) {

            ResponseModel response = new ResponseModel();
            response.operation = "attempting to generate new order key";

            if (Session["id"] != null)
            {
                var generateKey = KeyHelper.generateDisplayKey(content, Convert.ToInt32(Session["id"]));

                if (generateKey.success)
                {
                    response.response = generateKey.key;
                }
                else {
                    response.error = "denyed";
                }            
            }
            else
            {
                response.error = "need to login";  
            }

            return JsonConvert.SerializeObject(response);
        }


        public ActionResult Index(int content)
        {

            if (Session["id"] != null)
            {
                ViewData["menuID"] = content;

                string key = KeyHelper.getDisplayKey(content, Convert.ToInt32(Session["id"]));

                if (key != "")
                {
                    ViewData["keyTableRows"] = $"<tr onclick='showKey(this)'><td><input class='key' type='password' value='{key}' disabled /></td><td class='btn-key-new'><i class='fas fa-sync-alt'><span>Generate New Key</span></i></td></tr>";
                    ViewData["keyTableText"] = "1 / 1 Keys Created";
                }
                else {
                    ViewData["keyTableRows"] = "<tr><td style='color: grey;'>You haven't created any keys</td></tr>";
                    ViewData["keyTableText"] = "Create New Key";
                    ViewData["keyTableButtonCreateKeyClass"] = "btn-key-new";
                }

            
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}