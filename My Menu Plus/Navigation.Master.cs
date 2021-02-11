using My_Menu_Plus.Helpers;
using MyMenuPlus.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace My_Menu_Plus
{
    public partial class Navigation : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] != null) 
            {
                loginText.Text = Session["email"].ToString();
            }

            //MailHelper.verificationEmail("harrismith988@yahoo.co.uk");

        }
        
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = loginEmail.Text;
            string password = loginPassword.Text;

            if (LoginHelper.Login(email, password))
            {
                Debug.WriteLine("logged in");
                Session["email"] = email;
                Response.Redirect("./Menu?login=success");
            }
            else 
            {
                Response.Redirect("./Menu?login=failed");
            }
            

        }

    }
}