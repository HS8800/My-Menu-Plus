using MyMenuPlus.Helpers;
using MyMenuPlus.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyMenuPlus.Controllers
{
    public class LoginController : Controller
    {
 
 
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
        public string ResetPassword(string email)
        {

            ResponseModel response = new ResponseModel();
            response.operation = "attempting to reset password";

            var ResetCode = AccountHelper.generatePasswordResetCode(email);

            if (ResetCode.exists)
            {
               
                MailHelper.resetPassword(email, ResetCode.code);
                
                response.response = "success";
            }
            else {
                response.response = "failed";
                response.error = "The users password can not be reset at this time";                
            }

            return JsonConvert.SerializeObject(response);
        }

        
        public ActionResult NewPassword(string email, string code)
        {
            ViewData["email"] = email;
            ViewData["code"] = code;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public string NewPassword(string email, string code, string password)
        {
            ResponseModel response = new ResponseModel();
            response.operation = "attempting to assign new password";

            if (password.Length < 10)
            {
                response.response = "failed";
                response.error = "Password must be at least 10 characters";
                return JsonConvert.SerializeObject(response);
            }

            try
            {
                AccountHelper.resetPassword(email, password, code);
                response.response = "success";
            }
            catch {
                response.response = "failed";
                response.error = "internal error";
            }
            
            return JsonConvert.SerializeObject(response);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Register(string firstname, string secondname, string email, string password)
        {
            //Define response model
            ResponseModel response = new ResponseModel();
            response.operation = "attempting to register a new account";


            if (password.Length < 10) {
                response.response = "failed";
                response.error = "Password must be at least 10 characters";
                return JsonConvert.SerializeObject(response);
            }

            //Attempt to register account
            var RegisterAttempt = AccountHelper.Register(firstname, secondname, email, password);
            if (RegisterAttempt.success)
            {              
                response.response = "success";             
            }
            else {
                response.response = "failed";
                response.error = RegisterAttempt.details;
            }

            return JsonConvert.SerializeObject(response);
        }




    }
}