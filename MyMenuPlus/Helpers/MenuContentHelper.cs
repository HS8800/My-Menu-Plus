using MyMenuPlus.Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;


namespace MyMenuPlus.Helpers
{
    internal sealed class MenuContentHelper
    {

        internal static (bool success, string details) updateMenu(int menuID, int accountID, string menuData, string menuImage)
        {

            if (menuImage.Length > 800000) {
                return (false, "Image is to large, please use an image less than 600kbs");
            } else if (menuData.Length > 16777215) {
                return (false, "Your menu is to large, please reduce the number of items to save");
            }

            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
            try
            {
                connection.Open();
                string query = "CALL updateMenu(@accountID,@ownerID,@menuData,@menuImage)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@accountID", menuID);
                command.Parameters.AddWithValue("@ownerID", accountID);
                command.Parameters.AddWithValue("@menuData", menuData);
                command.Parameters.AddWithValue("@menuImage", menuImage);

                command.ExecuteNonQuery();
                return (true, "Menu updated");
            }
            catch (MySqlException ex)
            {
                return (false, "an internal error occured");
            }
        }


        internal static (bool success, string details) DeleteMenu(int menuID, int accountID)
        {
            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
            try
            {
                connection.Open();
                string query = "CALL deleteMenu(@menuID,@accountID)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@menuID", menuID);
                command.Parameters.AddWithValue("@accountID", accountID);

                command.ExecuteNonQuery();
                return (true, "Menu deleted");
            }
            catch (MySqlException ex)
            {
                return (false, "an internal error occured");
            }
        }


        internal static (bool success, string details) CreateMenu(string menuName, int accountID) {
            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
            try
            {
                connection.Open();
                string query = "CALL createMenu(@accountID,@menuName)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@menuName", menuName);
                command.Parameters.AddWithValue("@accountID", accountID);

                command.ExecuteNonQuery();
                return (true, "New menu created");
            }
            catch (MySqlException ex)
            {
                return (false, "an internal error occured");
            }
        }


        internal static string MenuThumbnails(int accountID)
        {
           
            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString); 
            try
            {
                connection.Open();             
                string query = "CAll getMenus(@accountID)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@accountID", accountID);


                StringBuilder thumbnailBuilder = new StringBuilder();
                MySqlDataReader reader = command.ExecuteReader();

                int count = 1;
                while (reader.Read())
                {

             
                    thumbnailBuilder.Append("<div data-aos='fade-up' data-aos-delay='");//reader["title"]

                    thumbnailBuilder.Append(count*100);//animation fade in delay
                    thumbnailBuilder.Append("'>");
                    thumbnailBuilder.Append("<div class='menu-btn-container' ><i class='fas fa-trash-alt btn-menu-delete' data-id='");
                    thumbnailBuilder.Append(reader["id"]);
                    thumbnailBuilder.Append("' btn-menu-delete' ></i></div>");
                    thumbnailBuilder.Append("<div class='section-title title-bold select-thumbnail'>");
                    thumbnailBuilder.Append(Convert.ToString(reader["title"]).Length > 13 ? Convert.ToString(reader["title"]).Substring(0, 10)+"..." : reader["title"]);//title LIMIT to no more than 13
                    thumbnailBuilder.Append("</div>");
                    thumbnailBuilder.Append("<i class='fa fa-plus-square'></i>");
                    thumbnailBuilder.Append("</div>");

                    count++;

                }

                return thumbnailBuilder.ToString();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return "<h3>Your menus could not be loaded at this time<h3>";
            }

        }

    }
}