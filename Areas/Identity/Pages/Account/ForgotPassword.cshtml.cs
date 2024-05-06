using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Mail;
using MimeKit;
using MailKit.Security;

namespace OnlineBookShopping.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
                
                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                //DateTime senttime = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
                string senttime = DateTime.Now.ToString();
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", senttime,code },
                    protocol: Request.Scheme);

               // await _emailSender.SendEmailAsync(Input.Email, "Reset Password", callbackUrl);

                #region send email try method
                //{
                //    var emailMessage = new MimeMessage();

                //    emailMessage.From.Add(new MailboxAddress("Reset Password", "abc@gmail.com"));
                //    emailMessage.To.Add(new MailboxAddress("", Input.Email));
                //    emailMessage.Subject = "Reset Password";
                //    emailMessage.Body = new TextPart("plain") { Text = callbackUrl };
                //    SmtpClient smtpClient = new SmtpClient();
                //    using (var client = new SmtpClient())
                //    {

                //        client.Credentials = new System.Net.NetworkCredential("abc@gmail.com", "12345");

                //         //client.SendMailAsync(emailMessage).ConfigureAwait(true);
                //    }
                //}
                #endregion

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                      $"<h3>Do'nt worry ! You can reset your password.</h3><br>" +
                        $"<p>Thank you for showing your interest in my website.</p>" +
                        $"Please reset your password by Clicking Button Below. <br> <p>_____________________________________________</p><br><a style='display: block; width: 115px; height: 25px;  background: #4E9CAF; padding: 10px;text-align: center; border-radius: 5px;color: white; font-weight: bold;line-height: 25px;' href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Reset Password</a>.");

                //TempData["CheckYourEmail"] = "Check Your Email To Reset Your Password";
                ViewData["CheckYourEmail"] = "Check Your Email to reset your password.";
                return Page();
            }


            return Page();
        }

     
    }
}
