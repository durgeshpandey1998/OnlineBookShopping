using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace OnlineBookShopping.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private IHttpContextAccessor Accessor;

        public ResetPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender, IHttpContextAccessor _accessor)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            this.Accessor = _accessor;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
            public DateTime senttime { get; set; }
        }

        public IActionResult OnGet(string code = null, string senttime = null)
        {

            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                var addmiutes=Convert.ToDateTime(senttime).AddMinutes(30);
                DateTime currenttime = DateTime.Now;
                if (addmiutes < currenttime)
                {
                    return Redirect(@"../../Book/TokenInvalid");
                }
                else
                {
                 
                    Input = new InputModel
                    {
                        Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                    };
                    return Page();
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                await _emailSender.SendEmailAsync(Input.Email, "Password Changed Succesfully",
                       $"<h3>Congratulation ! Your password has been changed successfully..</h3><br>" +
                       $"<h3>Please Keep Your Credentials Confedential for future use.</h3><br>" +
                       $"<p>Username={user} <br> Password={Input.Password}</p>.");


                return RedirectToPage("./Login");
            }

            //foreach (var error in result.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, error.Description);
            //}
            return Page();
        }
    }
}
