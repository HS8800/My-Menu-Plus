using Braintree;
using MyMenuPlus.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace MyMenuPlus.Helpers
{
    public class BrainTree
    {

        public static BraintreeGateway gateway;


        /// <summary>
        /// Set braintree payment gateway credentials
        /// </summary>
        /// <param name="menuID">An int32 menu identifier</param>   
        public BrainTree(int menuID)
        {
            var braintreeKeys = BrainTreeHelper.getBraintreeKeys(menuID);
         
            BraintreeGateway _gateway = new BraintreeGateway
            {
                Environment = braintreeKeys.enviroment,
                MerchantId = braintreeKeys.merchantID,
                PublicKey = braintreeKeys.publicKey,
                PrivateKey = braintreeKeys.privateKey
            };

            gateway = _gateway;
            
        }

        /// <summary>
        /// Generate a client token for a braintree client
        /// </summary>
        /// <returns>Braintree token used for authentication</returns>
        public (bool success,string token) CreateClientToken() {
            try
            {
                return (true, gateway.ClientToken.Generate());
            }
            catch {
                return (false, "");
            }
        }


        /// <summary>
        /// Create braintree payment gateway
        /// </summary>       
        /// <returns>Braintree gateway object</returns>
        public IBraintreeGateway CreateGateway()
        {
            return new BraintreeGateway(gateway.Environment, gateway.MerchantId, gateway.PublicKey, gateway.PrivateKey);
        }

    }


    /// <summary>
    /// Utility class for braintree transactions
    /// </summary>       
    public class BrainTreeHelper {


        /// <summary>
        /// Get braintree credentials from database
        /// </summary>
        /// <returns>Braintree API keys</returns>
        internal static (bool success, Braintree.Environment enviroment, string merchantID, string publicKey, string privateKey) getBraintreeKeys(int menuID)
        {

            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);   
            connection.Open();
            string query = "CALL getBraintreeKeys(@menuID)";
            MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@menuID", menuID);


            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {

                dynamic Enviroment = Braintree.Environment.SANDBOX;
                if (Convert.ToBoolean(reader["Production"])) {
                    Enviroment = Braintree.Environment.PRODUCTION;
                }

                return (
                    true,
                    Enviroment,
                    Convert.ToString(reader["MerchantID"]),
                    Convert.ToString(reader["PublicKey"]),
                    Convert.ToString(reader["PrivateKey"])
                );

            }
                connection.Close();

                
            return (false, Braintree.Environment.SANDBOX, "", "", "");

        }

        /// <summary>
        /// Build dictionary of pricing for a menu
        /// </summary>
        /// <returns>Dictionary of pricing for a menu</returns>
        public static (bool Success, Dictionary<int, PriceModel> PriceDictionary) getPriceDictionary(int menuID) {

            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
            try
            {
                connection.Open();
                string query = "CALL getMenuContent(@menuID)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@menuID", menuID);

                MySqlDataReader reader = command.ExecuteReader();

                var priceLookup = new Dictionary<int, PriceModel>();
 
                while (reader.Read())
                {
                    //Build pricing dictionary from stored section item info
                    dynamic menuData = JsonConvert.DeserializeObject(Convert.ToString(reader["menuData"]));              
                    dynamic sections = menuData.sections;
                    for (int sectionIndex = 0; sectionIndex < sections.Count; sectionIndex++)
                    {  
                        dynamic sectionItems = sections[sectionIndex].sectionItems;
                        for (int itemIndex = 0; itemIndex < sectionItems.Count; itemIndex++)
                        {
                            try
                            {
                                if (Convert.ToDecimal(sectionItems[itemIndex].price) > 0.01m)
                                {
                                    PriceModel priceInfo = new PriceModel();
                                    priceInfo.name = Convert.ToString(sectionItems[itemIndex].name);
                                    priceInfo.price = Convert.ToDecimal(sectionItems[itemIndex].price);
                                    priceLookup.Add(Convert.ToInt32(sectionItems[itemIndex].id), priceInfo);
                                }
                            }
                            catch { }
                        }
                    }

                   
                }

                connection.Close();
                return (true, priceLookup);//return dictionary                            
            }
            catch (MySqlException ex)
            {
                connection.Close();
                return (false, null);
            }
        

    }


    }

}