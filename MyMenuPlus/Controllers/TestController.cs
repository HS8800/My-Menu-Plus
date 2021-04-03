using MyMenuPlus.Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMenuPlus.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index(int content)
        {
    


            using (MySqlConnection connection = new MySqlConnection(ConfigHelper.connectionString))
            {
                MySqlCommand command = new MySqlCommand("CALL getBraintreeKeys(@menuID)", connection);
                command.Connection.Open();
                command.Parameters.AddWithValue("@menuID", 25);
                
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dynamic Enviroment = Braintree.Environment.SANDBOX;
                        if (Convert.ToBoolean(reader["Production"]))
                        {
                            Enviroment = Braintree.Environment.PRODUCTION;
                        }

                        ViewData["footer"] = Enviroment;
                    }
                } 
            }












            return View();
        }
    }
}