using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace MyMenuPlus.Helpers
{
    public class MailHelper
    {


        /// <summary>
        /// Send a welcome email
        /// </summary>
        /// <param name="toEmail">string based email address of recipient</param>
        /// <returns>bool success, string details</returns>
        public static (bool success, string details) welcomeEmail(string toEmail) {
            try
            {

                MailMessage message = new MailMessage();
                message.From = new MailAddress("noreply@mymenuplus.com");
                message.To.Add(new MailAddress(toEmail));
                message.Subject = "Welcome to MyMenuPlus";
                message.IsBodyHtml = true;
                message.Body = @"

                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Verification</title>
                    <link href='https://fonts.googleapis.com/css2?family=Poppins:wght@100;200;300;400;500;600;700&amp;display=swap' rel='stylesheet'>
                </head>
                <body>

                <style>
                    body{
                        background-color: #f0f2f5;
                        font-family: 'Poppins', sans-serif;
                    }
                    #main{
                        background-color: white;
                        margin: 50px;
                        text-align: center;
                        padding-bottom: 50px;
                    }
                    #top{
                        background-color: #6cc1ff;
                        height: 50px;
                        color: white;
                        line-height: 50px;
                        font-size: 20px;
                    }
                    #verify{
                        font-size: 15px;
                        background-color: #1395f1;
                        margin: auto;
                        outline: none;
                        border: none;
                        cursor: pointer;
                        border-radius: 5px;
                        color: white;
                        font-family: 'Poppins', sans-serif;
                        margin: 20px;
                        line-height: 44px;
                        padding: 10px 50px;
                    }

                    h3{
                        font-size: 30px;
                    }

                #email-img{
                    height: 100px;
                    width: 100px;
                    object-fit: contain;
                    margin: auto;
                    margin-bottom: 31px;
                }

                    #line{
                        border-top: 1px solid #e4e4e4;
                        width: 80%;
                        margin: 21px auto;
                    }
                </style>
                <div id='main'>
                    <div id='top'>My Meals Plus</div>
                    <div id='content'>
                        <h3>Thankyou For Joining!</h3>

                          <img id='email-img' src='data:image/png;base64,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'>
                        <div id='line'></div>
                        <p style='margin-top: 31px;'>Your just one step away from getting started</p>
                        <p>Please click the button bellow to verify your email</p>
                        <button id='verify'>Verify</button>
                    </div>
                </div>
                </body>
                </html>";


              

                //SmtpClient smtp = new SmtpClient("hgws5.win.hostgator.com");
                //smtp.Credentials = new System.Net.NetworkCredential("noreply@mymenuplus.com", "8_Txmy31");
                //smtp.Port = 25;
                //smtp.EnableSsl = true;
                //smtp.Send(message);

                using (var smtp = new SmtpClient("hgws5.win.hostgator.com"))
                {
                    smtp.Credentials = new System.Net.NetworkCredential("noreply@mymenuplus.com", "8_Txmy31");
                    
                    smtp.Port = 25;
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                }

                return (true, "email sent");
            }
            catch (Exception err)
            {
                Debug.WriteLine(err);
                return (false, "email failed to send");
            }


            
        }

    }
}