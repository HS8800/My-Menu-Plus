using Braintree;
using MyMenuPlus.Helpers;
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


            string nonceFromTheClient = collection["payment_method_nonce"];
            // Use payment method nonce here


            Random rnd = new Random();
            var request = new TransactionRequest
            {
                Amount = 26,
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


    }
}