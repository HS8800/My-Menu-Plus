using Braintree;
using MyMenuPlus.Helpers;
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
            BrainTree brain = new BrainTree();


            //Get post fields
            string nonceFromTheClient = collection["payment_method_nonce"]; 
            int menuID = Convert.ToInt32(collection["menu-id"]);
            dynamic basketItems = JsonConvert.DeserializeObject(collection["basket-items"]);          
            string untrustedBasketAmount = collection["basket-amount"];

            //Find menu prices
            var PriceDictionary = Helpers.BrainTreeHelper.getPriceDictionary(menuID);
            if (!PriceDictionary.Success) {
                TempData["Error"] = "Unable to confirm prices, we where unable to complete the translation";
                return RedirectToAction("Error");
            }

    
            //Check that pricing and item names are correct
            decimal trustedTotal = 0;          
            foreach (var item in basketItems)
            {
                var itemLookup = PriceDictionary.PriceDictionary[Convert.ToInt32(item.id)];
                if (Convert.ToDecimal(Convert.ToString(item.price).Substring(1)) == itemLookup.price && item.name == itemLookup.name)
                {
                    trustedTotal += Convert.ToDecimal(Convert.ToString(item.price).Substring(1)) * Convert.ToInt32(item.qty);
                }
                else {
                    //return error when item info don't match server info
                    TempData["Error"] = "Pricing error, we where unable to complete the translation";
                    return RedirectToAction("Error");
                }
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


            if (result.IsSuccess())
            {
                TempData["Success"] = "Transaction was successful, Transaction ID" + result.Target.Id + ", Amount Charged : £" + result.Target.Amount;
                return RedirectToAction("Success");
            }


            TempData["Error"] = result.Target.ProcessorResponseText;
            return RedirectToAction("Error");
        }

        public ActionResult Success() {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }

    }
}