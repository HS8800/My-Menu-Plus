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
        /// <summary>
        /// Create new account
        /// </summary>
        /// <param name="firstname">string of account firstname</param>
        /// <param name="secondname">string of account secondname</param>
        /// <param name="email">string of associated email</param>
        /// <param name="password">string of raw plain text password</param>
        /// <returns>bool success, string details</returns>
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

        /// <summary>
        /// Attempt to login with supplied credentials
        /// </summary>
        /// <param name="email">string of login email</param>
        /// <param name="rawPassword">string of raw plain text password</param>
        /// <returns>bool success, string details</returns>
        internal static (bool success, string details, int accountID) Login(string email,string rawPassword)
        {
           
            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString); 
            try
            {
                connection.Open();             
                string query = "SELECT id, password FROM account WHERE email = @email limit 1";
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@email", email);
           
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string hashedPassword = reader["password"].ToString();
                    int accountID = Convert.ToInt32(reader["id"]);

                    if (hashedPassword != "" && Cryptography.VerifyHash(rawPassword, hashedPassword))
                    {
                        System.Diagnostics.Debug.WriteLine("login success");
                        return (true, "login success", accountID);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("login failed");
                        return (false, "login failed", -1);
                    }
                }

                return (false, "no such account found", -1);

            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return (false, "an internal error occured", -1);
            }

        }
        internal static bool CanEditMenu(int menuID, int accountID)
        {

            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
            try
            {
                connection.Open();
                string query = "CALL canEditMenu(@menuID,@accountID)";
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@menuID", menuID);
                command.Parameters.AddWithValue("@accountID", accountID);

                return Convert.ToBoolean(command.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return false;
            }

        }


        

    }
}