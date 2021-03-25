using Microsoft.AspNet.SignalR;
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
    public class HomeController : Controller
    {



        public ActionResult Index()
        {
            var OrderDisplayHub = GlobalHost.ConnectionManager.GetHubContext<OrderDisplayHub>();
            OrderDisplayHub.Clients.All.Order(1, 3, "data example");

            return View();
        }

        public ActionResult Display()
        {
            return View();
        }

   
    }
}