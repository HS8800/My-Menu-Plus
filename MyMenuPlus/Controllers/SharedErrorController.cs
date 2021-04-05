using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMenuPlus.Controllers
{
    public class SharedErrorController : Controller
    {
        public ActionResult Error()
        {
            return View();
        }

    }
}