using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMenuPlus.Controllers
{
    public class QRController : Controller
    {
        // GET: QR
        public ActionResult Index(int content)
        {

            ViewData["menuID"] = content;
            return View();
        }
    }
}