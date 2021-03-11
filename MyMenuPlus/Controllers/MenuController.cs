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
            
            return View();
        }

        
 

    }
}