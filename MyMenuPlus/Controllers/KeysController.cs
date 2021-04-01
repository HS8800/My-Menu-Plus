using Braintree;
using MyMenuPlus.Helpers;
using MyMenuPlus.Models;
using MySql.Data.MySqlClient;
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
        [ValidateAntiForgeryToken]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string updateBraintreeKeys(int content,bool production ,string MerchantID,string PublicKey,string PrivateKey)
        {

            ResponseModel response = new ResponseModel();
            response.operation = "Updating braintree api keys";

            //Get correct environment
            dynamic _Enviroment = Braintree.Environment.SANDBOX;
            if (production) {
                _Enviroment = Braintree.Environment.PRODUCTION;
            }

            //Build gateway with new keys
            BraintreeGateway gateway = new BraintreeGateway
            {
                Environment = _Enviroment,
                MerchantId = MerchantID,
                PublicKey = PublicKey,
                PrivateKey = PrivateKey
            };

            //Test new keys work
            try
            {
                gateway.ClientToken.Generate();
            }
            catch {
                response.error = "Invalid api keys";
                return JsonConvert.SerializeObject(response);
            }


            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);       
            connection.Open();
            string query = "CALL updateBraintreeKeys(@Production,@MerchantID,@PublicKey,@PrivateKey,@MenuID,@ownerID)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Production", production);
            command.Parameters.AddWithValue("@MerchantID", MerchantID);
            command.Parameters.AddWithValue("@PublicKey", PublicKey);
            command.Parameters.AddWithValue("@PrivateKey", PrivateKey);
            command.Parameters.AddWithValue("@MenuID", content);
            command.Parameters.AddWithValue("@ownerID", Convert.ToInt32(Session["id"]));

            command.ExecuteNonQuery();
            connection.Close();
             


            response.response = "Success";
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