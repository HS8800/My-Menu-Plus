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
        /// Generates a secret code attached to a user account used to reset their password
        /// </summary>
        /// <param name="email">email as plain text</param>
        /// <returns>Returns a secret code to reset password</returns>
        internal static (bool exists, string code) generatePasswordResetCode(string email)
        {

            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
            try
            {
                connection.Open();
                string query = "Call generatePasswordResetCode(@email)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);
                string response = Convert.ToString(command.ExecuteScalar());
                connection.Close();

                if (response != "")
                {
                    return (true, response);                 
                }
                else
                {
                    return (false, "");
                }

            }
            catch (MySqlException ex)
            {
                return (false, "");
            }

        }


        /// <summary>
        /// Use the secret code from generatePasswordResetCode(string email) to reset a users password
        /// </summary>
        /// <param name="email">email as plain text</param>
        /// <param name="passwprd">password as plain text</param>
        /// <param name="code">secret code from generatePasswordResetCode(string email)</param>
        /// <returns>Returns a secret code to reset password</returns>
        internal static void resetPassword(string email,string password,string code)
        {
            
            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);         
            connection.Open();
            string query = "Call resetPassword(@email,@code,@passwordHash)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@code", code);
            command.Parameters.AddWithValue("@passwordHash", Cryptography.Hash(password));
            command.ExecuteNonQuery();
            connection.Close();

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

        /// <summary>
        /// Used to connect a kitchen order display to a menu using its private key
        /// </summary>
        /// <param name="displaykey">plain text private key found in the keys section of all menus</param>   
        /// <returns>If operation was successful</returns>
        internal static (bool success,int menuID) displayLogin(string displaykey) {

            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
            try
            {
                connection.Open();
                string query = "CALL displayLogin(@displayKey)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@displayKey", displaykey);


                string f = Convert.ToString(command.ExecuteScalar());

                if (Convert.ToString(command.ExecuteScalar()) != "false")
                {
                    int menuID = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                    return (true, menuID);
                }
                else {
                    connection.Close();
                    return (false, -1);
                }
            }
            catch (MySqlException ex)
            {
                connection.Close();
                return (false, -1);
            }


        }


        /// <summary>
        /// Checks user permissions for a menu
        /// </summary>
        /// <param name="menuID">An int32 menu identifier</param>   
        /// <param name="accountID">An int32 account identifier</param>   
        /// <returns>If user can edit menu</returns>
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


                Boolean canEdit = Convert.ToBoolean(command.ExecuteScalar());
                connection.Close();
                return canEdit;
            }
            catch (MySqlException ex)
            {               
                connection.Close();
                return false;
            }

        }


        

    }
}