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
    public class OpenTimesController : Controller
    {

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string updateMenuTime(
            int content,
            TimeSpan MondayOpen,
            TimeSpan TuesdayOpen,
            TimeSpan WednesdayOpen,
            TimeSpan ThursdayOpen,
            TimeSpan FridayOpen,
            TimeSpan SaturdayOpen,
            TimeSpan SundayOpen,
            TimeSpan MondayClose,
            TimeSpan TuesdayClose,
            TimeSpan WednesdayClose,
            TimeSpan ThursdayClose,
            TimeSpan FridayClose,
            TimeSpan SaturdayClose,
            TimeSpan SundayClose)
        {

            ResponseModel response = new ResponseModel();
            response.operation = "attempting to update open times";


            if (Session["id"] == null) {
                response.response = "failed";
                response.error = "need to login";
                return JsonConvert.SerializeObject(response);
            }
                

            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
            try
            {
                connection.Open();
                string query = "CALL setOpenTimes(@menuID,@accountID,@mondayOpenTime,@tuesdayOpenTime,@wednesdayOpenTime,@thursdayOpenTime,@fridayOpenTime,@saturdayOpenTime,@sundayOpenTime,@mondayCloseTime,@tuesdayCloseTime,@wednesdayCloseTime,@thursdayCloseTime,@fridayCloseTime,@saturdayCloseTime,@sundayCloseTime)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@menuID", content);
                command.Parameters.AddWithValue("@accountID", Convert.ToInt32(Session["id"]));

                command.Parameters.AddWithValue("@mondayOpenTime", MondayOpen);
                command.Parameters.AddWithValue("@tuesdayOpenTime", TuesdayOpen);
                command.Parameters.AddWithValue("@wednesdayOpenTime", WednesdayOpen);
                command.Parameters.AddWithValue("@thursdayOpenTime", ThursdayOpen);
                command.Parameters.AddWithValue("@fridayOpenTime", FridayOpen);
                command.Parameters.AddWithValue("@saturdayOpenTime", SaturdayOpen);
                command.Parameters.AddWithValue("@sundayOpenTime", SundayOpen);

                command.Parameters.AddWithValue("@mondayCloseTime", MondayClose);
                command.Parameters.AddWithValue("@tuesdayCloseTime", TuesdayClose);
                command.Parameters.AddWithValue("@wednesdayCloseTime", WednesdayClose);
                command.Parameters.AddWithValue("@thursdayCloseTime", ThursdayClose);
                command.Parameters.AddWithValue("@fridayCloseTime", FridayClose);
                command.Parameters.AddWithValue("@saturdayCloseTime", SaturdayClose);
                command.Parameters.AddWithValue("@sundayCloseTime", SundayClose);

                command.ExecuteNonQuery();
                connection.Close();

                response.response = "success";
            }
            catch (MySqlException ex)
            {
                connection.Close();
                response.response = "failed";
                response.error = "Internal Error";
            }

            return JsonConvert.SerializeObject(response);
        }

        
        public ActionResult Index(int content)
        {

            

            var menuTimes = Helpers.MenuContentHelper.menuTimes(content);
            if (menuTimes.success) {
                MenuTimeModel menuTime =  menuTimes.menuTime;

                ViewData["MondayOpen"] = menuTime.MondayOpen;
                ViewData["TuesdayOpen"] = menuTime.TuesdayOpen;
                ViewData["WednesdayOpen"] = menuTime.WednesdayOpen;
                ViewData["ThursdayOpen"] = menuTime.ThursdayOpen;
                ViewData["FridayOpen"] = menuTime.FridayOpen;
                ViewData["SaturdayOpen"] = menuTime.SaturdayOpen;
                ViewData["SundayOpen"] = menuTime.SundayOpen;

                ViewData["MondayClose"] = menuTime.MondayClose;
                ViewData["TuesdayClose"] = menuTime.TuesdayClose;
                ViewData["WednesdayClose"] = menuTime.WednesdayClose;
                ViewData["ThursdayClose"] = menuTime.ThursdayClose;
                ViewData["FridayClose"] = menuTime.FridayClose;
                ViewData["SaturdayClose"] = menuTime.SaturdayClose;
                ViewData["SundayClose"] = menuTime.SundayClose;
            }

            ViewData["menuID"] = content;
            return View();
        }
    }
}