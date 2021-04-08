using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;



namespace MyMenuPlus.Helpers
{
    public class MailHelper
    {

        public static void resetPassword(string toEmail, string resetCode)
        {



            //MailMessage message = new MailMessage();
            //message.From = new MailAddress("noreply@mymenuplus.com");
            //message.To.Add(new MailAddress("harrismith988@yahoo.co.uk"));
            //message.Subject = "MyMenuPlus Password Reset";
            //message.IsBodyHtml = true;
            //message.Body = "hello 2";

            //using (SmtpClient smtp = new SmtpClient("mail.mymenuplus.com", 25))
            //{

            //    smtp.Credentials = new System.Net.NetworkCredential("noreply@mymenuplus.com", "~7o4j9Vx");
            //    smtp.SendCompleted += (s, e) =>
            //    {
            //        smtp.Dispose();
            //        message.Dispose();
            //    };
            //    smtp.EnableSsl = false;
            //    smtp.Send(message);
            //}


            string passwordResetLink = "https://www.mymenuplus.com/Login/NewPassword?email=" + toEmail + "&code=" + Regex.Replace(resetCode, @"<[^>]+>| ", "").Trim();

            MailMessage message = new MailMessage();
            message.From = new MailAddress("noreply@mymenuplus.com");
            message.To.Add(new MailAddress(toEmail));
            message.Subject = "MyMenuPlus Password Reset";
            message.IsBodyHtml = true;
            message.Body = $@"

                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <link href='https://fonts.googleapis.com/css2?family=Poppins:wght@100;200;300;400;500;600;700&amp;display=swap' rel='stylesheet'>
                    <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.2/css/all.min.css'>

                </head>
                <body>

                    <div id='container'>
                        <div class='section-title title-small'>Reset Password</div>
                        <div class='section-title title-bold' style='padding:0px 50px'>You have requested to reset your password</div>
                        <i class='fas fa-lock-open'></i>
                        <a class='btn' id='btn-reset' rel='nofollow noopener noreferrer' target='_blank' href='{HttpUtility.UrlEncode(passwordResetLink)}'>Reset Password</a>
                        < a id='link' rel='nofollow noopener noreferrer' target='_blank' href='{HttpUtility.UrlEncode(passwordResetLink)}'>{passwordResetLink}</a>
                    </div>



                </body>
                </html>

                <style>

                    .fa-lock-open{{
                        font-size: 122px;
                        color: #a1aeb5;
                        transform: translateX(20px);

                        margin-top: 30px;
                        margin-bottom: 30px;
                    }}

                    body{{
                        background-color: #f9f9f9;
                    }}

					#container{{
                        font-family: 'Poppins', sans-serif;
                        background-color: white;
                        margin: auto;
                        max-width: 700px;
                        min-height: 100px;
                        width: 100%;
                        box-shadow: 0 0 5px 0 rgb(0 0 0 / 8%);
                        text-align: center;
                        padding-top: 30px;
                        padding-bottom: 50px;
                    }}
    
                    .section-title {{
                        font-size: 27px;
                        text-align: center;
                        color: #0c3853;
                    }}

                    .title-small {{
                        font-size: 17px;
                        color: #cc3333;
                    }}

                    .title-bold {{
                        font-weight: 700;
                    }}

                    .btn {{
                        display: block;
                        border-radius: 99px;
                        padding: 10px 20px;
                        width: 200px;
                        outline: none;
                        border: none;
                        cursor: pointer;
                        font-size: 20px;
                        font-weight: 600;
                        background: #cc3333;
                        color: white;
                    }}

                    #btn-reset{{
                        margin: 20px auto;
                        color: white;
                        text-decoration: none;

                    }}

                    #btn-reset:hover{{
                        opacity: 0.7;
                    }}

                    #link{{
                        font-size: 14px;
                        color: #5f5858;
                        word-break: break-all
                    }}

                    @media only screen and (max-width: 400px) {{
                        .title-bold {{
                            font-size: 21px;
                        }}
                    }}



                </style>
            ";


            using (SmtpClient smtp = new SmtpClient("mail.mymenuplus.com", 25))
            {

                smtp.Credentials = new System.Net.NetworkCredential("noreply@mymenuplus.com", "~7o4j9Vx");
                smtp.SendCompleted += (s, e) =>
                {
                    smtp.Dispose();
                    message.Dispose();
                };
                smtp.EnableSsl = false;
                smtp.Send(message);
            }
         



        }



    }

}
