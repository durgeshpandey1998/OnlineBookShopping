using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OnlineBookShopping.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            #region mail code subject and message

            var expiretime = DateTime.Now.AddMinutes(30);
            var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.From = new MailAddress("abc@gmail.com");
            message.Subject = subject;

            message.Body = htmlMessage;
           // _ = message.Headers.AllKeys;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "abc@gmail.com.com",
                    Password = "password"
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
            #endregion 

            #region try this for mail  
            //SmtpClient client = new SmtpClient
            //{
            //    Port = 587,
            //    Host = "smtp.gmail.com", //or another email sender provider
            //    EnableSsl = true,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential("abc@gmail.com", "password","smtp.gmail.com")
            //}; 
            
            //client.EnableSsl = true;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;   
            ////return client.SendMailAsync("abc@gmail.com", email, subject, htmlMessage);

            #endregion
        }
    }
}
