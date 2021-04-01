using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMenuPlus.Helpers
{
    public class KeyHelper
    {


        internal static (bool success, string key) generateDisplayKey(int menuID, int accountID)
        {

            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
            try
            {
                connection.Open();
                string query = "CALL generateDisplayKey(@menuID,@accountID)";
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@menuID", menuID);
                command.Parameters.AddWithValue("@accountID", accountID);

                string key = Convert.ToString(command.ExecuteScalar());
                connection.Close();

                if (key == "Denyed") {
                    return (false, "");
                }

                return (true, key);

            }
            catch (MySqlException ex)
            {
                connection.Close();
                return (false, "");
            }

        }

        internal static string getDisplayKey(int menuID, int accountID)
        {

            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
            try
            {
                connection.Open();
                string query = "CALL getDisplayKey(@menuID,@accountID)";
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@menuID", menuID);
                command.Parameters.AddWithValue("@accountID", accountID);
                string key = Convert.ToString(command.ExecuteScalar());
                connection.Close();

                return key;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                connection.Close();
                return "";
            }

        }

       


    }
}