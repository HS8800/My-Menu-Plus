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
        //// GET: Login
        //public string Index()
        //{        
        //    return "hello";
        //}

        ////Get requests
        //public string WelcomeMsg(string input)
        //{
        //    if (!String.IsNullOrEmpty(input))
        //        return "Please welcome " + input + ".";
        //    else
        //        return "Please enter your name.";
        //}

 
        [HttpPost]//send as form-data (no content type needed)
        [ValidateAntiForgeryToken]
        public string Login(string email, string password)
        {

            ResponseModel response = new ResponseModel();
            response.operation = "attempting to login";

            var Login = AccountHelper.Login(email, password);

            if (Login.success)
            {
                Session["id"] = Login.accountID;              
                response.response = "success";
            }
            else 
            {
                response.response = "failed";
                response.error = "Hmm, your credientails don't seem to be right, Please try again or use the Forgot Password button";
            }

            return JsonConvert.SerializeObject(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Register(string firstname, string secondname, string email, string password)
        {
            //Define response model
            ResponseModel response = new ResponseModel();
            response.operation = "attempting to register a new account";

            //Attempt to register account
            var RegisterAttempt = AccountHelper.Register(firstname, secondname, email, password);
            if (RegisterAttempt.success)
            {
                ////Attempt to send welcome email
                //var MailAttempt = Helpers.MailHelper.welcomeEmail(email);
                //if (!MailAttempt.success) {
                //    response.response = "failed";
                //    response.error = MailAttempt.details;
                //}
                //else
                //{
                    response.response = "success";
                //}
            }
            else {
                response.response = "failed";
                response.error = RegisterAttempt.details;
            }

            return JsonConvert.SerializeObject(response);
        }




    }
}