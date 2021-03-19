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
        BrainTree brain = new BrainTree();

        // GET: BrainTree
        public ActionResult Index()
        {      
            ViewData["ClientToken"] = brain.CreateClientToken(); ;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePurchase(FormCollection collection)
        {       
            string nonceFromTheClient = collection["payment_method_nonce"];
            // Use payment method nonce here


            Random rnd = new Random();
            var request = new TransactionRequest
            {
                Amount = rnd.Next(1, 100),
                PaymentMethodNonce = nonceFromTheClient,
                OrderId = "1",
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            var gateway = brain.CreateGateway();
            Result<Transaction> result = gateway.Transaction.Sale(request);

            
            if (result.Target.ProcessorResponseText == "Approved") {
                TempData["Success"] = "Transaction was successful, Transaction ID" + result.Target.Id + ", Amount Charged : £" + result.Target.Amount;
            }
            return RedirectToAction("Index");


        }

    }
}