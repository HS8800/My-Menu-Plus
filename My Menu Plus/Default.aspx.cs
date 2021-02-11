using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace My_Menu_Plus
{
    public partial class _Default : Page
    {

        //get request https://stackoverflow.com/questions/34240187/call-server-method-without-refresh-page
        //post request https://stackoverflow.com/questions/29817796/how-to-pass-json-string-to-webmethod-c-sharp-asp-net
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = Session["Name"] as string;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Session["Name"] = TextBox1.Text;
            Label1.Text = Session["Name"] as string;
        }
    }
}