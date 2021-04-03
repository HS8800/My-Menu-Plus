using Braintree;
using Microsoft.AspNet.SignalR;
using MyMenuPlus.Helpers;
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
  
    public class BrainTreeController : Controller
    {


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePurchase(FormCollection collection)
        {         
            string nonceFromTheClient;
            int menuID;
            dynamic basketItems;
            int tableNumber;
            string comment = "";

            //Validate Parameters 
            try
            {
                //Get post fields
                nonceFromTheClient = collection["payment_method_nonce"];
                menuID = Convert.ToInt32(collection["menu-id"]);
                basketItems = JsonConvert.DeserializeObject(collection["basket-items"]);             
                tableNumber = Convert.ToInt32(collection["table-number"]);
                comment = Convert.ToString(collection["basket-notes"]);
            }
            catch {
                TempData["Error"] = "Missing Parameters";
                return RedirectToAction("Error");
            }
         
            if (tableNumber < 1) {
                TempData["Error"] = "Invalid Table Number";
                return RedirectToAction("Error");
            }

            if (menuID < 1) {
                TempData["Error"] = "Invalid Reference To Menu";
                return RedirectToAction("Error");
            }


            if (comment.Length > 30)
            {
                comment = comment.Substring(0, 30);
            }



                //Create braintree object
                BrainTree brain = new BrainTree(Convert.ToInt32(collection["menu-id"]));


            //Find menu prices
            var PriceDictionary = Helpers.BrainTreeHelper.getPriceDictionary(menuID);
            if (!PriceDictionary.Success) {
                TempData["Error"] = "Unable to confirm prices, we were unable to complete the translation";
                return RedirectToAction("Error");
            }

    
            //Check that pricing and item names are correct
            decimal trustedTotal = 0;
            List<OrderItemModel> trustedOrderItems = new List<OrderItemModel>();

            try{
                foreach (var item in basketItems)
                {
                    var itemLookup = PriceDictionary.PriceDictionary[Convert.ToInt32(item.id)];
                    if (Convert.ToDecimal(Convert.ToString(item.price).Substring(1)) == itemLookup.price && item.name == itemLookup.name)
                    {

                        //Create new verifyed item for order
                        OrderItemModel orderItemType = new OrderItemModel();
                        orderItemType.id = Convert.ToInt32(item.id);
                        orderItemType.name = itemLookup.name;
                        orderItemType.pricePerUnit = Convert.ToDecimal(Convert.ToString(item.price).Substring(1));
                        orderItemType.qty = Convert.ToInt32(item.qty);

                        trustedOrderItems.Add(orderItemType);

                        //Add to order total
                        trustedTotal += Convert.ToDecimal(Convert.ToString(item.price).Substring(1)) * Convert.ToInt32(item.qty);
                    }
                    else
                    {
                        //return error when item info don't match server info
                        TempData["Error"] = "Pricing error, we were unable to complete the translation";
                        return RedirectToAction("Error");
                    }
                }
            }
            catch {
                TempData["Error"] = "Your basket items seem to be damaged, we were unable to complete the translation ";
                return RedirectToAction("Error");
            }




 
            var request = new TransactionRequest
            {
                Amount = trustedTotal,
                PaymentMethodNonce = nonceFromTheClient,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            var gateway = brain.CreateGateway();
            Result<Transaction> result = gateway.Transaction.Sale(request);



            int newOrderID;
            if (result.IsSuccess())
            {
                //Attempt to create order
                MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
                try
                {
                    connection.Open();
                    string query = "CALL createOrder(@transactionID,@menuID,@tableNumber,@itemsJSON)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@transactionID", result.Target.Id);
                    command.Parameters.AddWithValue("@menuID", menuID);
                    command.Parameters.AddWithValue("@tableNumber", tableNumber);
                    command.Parameters.AddWithValue("@itemsJSON", JsonConvert.SerializeObject(trustedOrderItems));
                    newOrderID = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
                catch
                {
                    connection.Close();
                    //Attempt to create order again
                    try//retry
                    {
                        connection.Open();
                        string query = "CALL createOrder(@transactionID,@menuID,@tableNumber,@itemsJSON,@comment)";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@transactionID", result.Target.Id);
                        command.Parameters.AddWithValue("@menuID", menuID);
                        command.Parameters.AddWithValue("@tableNumber", tableNumber);
                        command.Parameters.AddWithValue("@itemsJSON", JsonConvert.SerializeObject(trustedOrderItems));
                        command.Parameters.AddWithValue("@comment", comment);                        
                        newOrderID = Convert.ToInt32(command.ExecuteScalar());
                        connection.Close();
                    }
                    catch
                    { //Could not create order
                        TempData["Error"] = $"A Serious Error has occured, a transaction of £{trustedTotal} was made but your order was unable to be created. Please provide the transaction id  {result.Target.Id} to a member of staff.";
                        return RedirectToAction("Error");
                    }
                }


               
                    //Send order to valid kitchen order displays
                    var OrderDisplayHub = GlobalHost.ConnectionManager.GetHubContext<OrderDisplayHub>();
                    
                    foreach (WebSocketClientModel client in OrderDisplayClients.WebSocketClients)
                    {
                        if (client.menuID == menuID) {//Only sent to displays of the same menuID
                            OrderDisplayHub.Clients.Client(client.connectionID).order(newOrderID, result.Target.Id, tableNumber, JsonConvert.SerializeObject(trustedOrderItems),comment);
                        }
                                             
                    }


                //Purchase successfull
                TempData["Success"] = "Transaction was successful, Transaction ID " + result.Target.Id + " Amount Charged : £" + result.Target.Amount;
                return RedirectToAction("Success");
            }


            TempData["Error"] = result.Target.ProcessorResponseText;
            return RedirectToAction("Error");
        }

        public ActionResult Success() {
            return View();
        }

        public ActionResult Alert()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

    }
}