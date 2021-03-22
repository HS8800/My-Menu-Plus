using Braintree;
using MyMenuPlus.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMenuPlus.Helpers
{
    public class BrainTree
    {

        BraintreeGateway gateway = new BraintreeGateway
        {
            Environment = Braintree.Environment.SANDBOX,
            MerchantId = "nfydc3g3765f3cn6",
            PublicKey = "57scb6kt4k93z69h",
            PrivateKey = "91380c8ae8665677c61f8fc9c86d65ef"
        };


        public string CreateClientToken() {
            return gateway.ClientToken.Generate();
        }

        public IBraintreeGateway CreateGateway()
        {
            return new BraintreeGateway(gateway.Environment, gateway.MerchantId, gateway.PublicKey, gateway.PrivateKey);
        }

    }

    public class BrainTreeHelper {


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
                            PriceModel priceInfo = new PriceModel();
                            priceInfo.name = Convert.ToString(sectionItems[itemIndex].name);
                            priceInfo.price = Convert.ToDecimal(sectionItems[itemIndex].price);
                            priceLookup.Add(Convert.ToInt32(sectionItems[itemIndex].id), priceInfo);
                        }
                    }

                    return (true, priceLookup);//return dictionary
                }

                return (false, null);//if no rows returned 
            }
            catch (MySqlException ex)
            {
                return (false, null);
            }
        

    }


    }

}