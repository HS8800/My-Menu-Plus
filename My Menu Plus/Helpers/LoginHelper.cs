using My_Menu_Plus.Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMenuPlus.Helpers
{
    internal sealed class LoginHelper
    {

        internal static bool Login(string email,string rawPassword)
        {
            string connectionString = "server=localhost:3306,mymenuplus.com;database=mymenlus_main;uid=mymen_admin8800;password=5%3epk9W5%3epk9W";
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();

                // string query = "SELECT EXISTS(SELECT * FROM account WHERE email = @email AND password = @password) AS 'canlogin'";
                string query = "SELECT password FROM account WHERE email = @email limit 1";
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@email", email);
  
                if (Cryptography.VerifyHash(rawPassword, Convert.ToString(command.ExecuteScalar())))
                {
                    System.Diagnostics.Debug.WriteLine("login success");
                    return true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("login failed");
                    return false;
                }

            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return false;
            }

        }

    }
}