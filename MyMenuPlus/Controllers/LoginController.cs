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
    public class LoginController : Controller
    {
        // GET: Login
        public string Index()
        {
            return "hello";
        }

        //Get requests
        public string WelcomeMsg(string input)
        {
            if (!String.IsNullOrEmpty(input))
                return "Please welcome " + input + ".";
            else
                return "Please enter your name.";
        }

        public string Sec()
        {
            return Session["test"].ToString();
        }

        [HttpPost]//send as form-data (no content type needed)
        [ValidateAntiForgeryToken]//@Html.AntiForgeryToken()
        public string Login(string email, string password)
        {

            ResponseModel response = new ResponseModel();
            response.operation = "Attempting to login";

            if (LoginHelper.Login(email, password))
            {        
                Session["email"] = email;
                response.response = "Success";
                
            }
            else
            {
                response.response = "Failed";
            }

            return JsonConvert.SerializeObject(response);
        }




     }
}