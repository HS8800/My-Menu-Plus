using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MyMenuPlus.Models
{
    public class LoginModels
    {
        public class LoginAttemptModel
        {
            public string email { get; set; }
            public string password { get; set; }

    
            //private string CleanString(string str)
            //{
            //    str = Regex.Replace(str, "[^A-Za-z0-9]+", "").Trim();
            //    return str;
            //}
            //public void clean()
            //{
            //    Auth = CleanString(Auth);
            //}
        }



    }
}