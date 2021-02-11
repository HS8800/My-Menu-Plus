using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace My_Menu_Plus.Controllers
{
    public class AccountVerifyController : ApiController
    {
      

        // GET: api/AccountVerify/5
        public string Get(int id)
        {
            return "hello";
        }

        // POST: api/AccountVerify
        public void Post([FromBody]string value)
        {
        }

     

 
    }
}
