using Braintree;
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
}