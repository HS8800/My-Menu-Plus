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