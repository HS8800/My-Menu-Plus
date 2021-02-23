using MyMenuPlus.Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace MyMenuPlus.Helpers
{
    internal sealed class AccountHelper
    {
 
        internal static (bool success,string details) Register(string firstname, string secondname, string email, string password) {

            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
            try
            {
                connection.Open();
                string query = "CAll register(@firstname,@secondname,@email,@hashedPassword)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@firstname", firstname);
                command.Parameters.AddWithValue("@secondname", secondname);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@hashedPassword", Cryptography.Hash(password));

                string response = Convert.ToString(command.ExecuteScalar());

                if (response == "account created")
                {
                    return (true, "account created");
                }
                else {
                    return (false, "account already exists");
                }

            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return (false, "an internal error occured");
            }

        }


        internal static bool Login(string email,string rawPassword)
        {
           
            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString); 
            try
            {
                connection.Open();             
                string query = "SELECT password FROM account WHERE email = @email limit 1";
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@email", email);
                string hashedPassword = Convert.ToString(command.ExecuteScalar());


                if (hashedPassword != "" && Cryptography.VerifyHash(rawPassword, hashedPassword))
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