using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace OnlineBookShopping.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
              RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            [Required]
            [Display(Name ="Select Role")]
            public string RoleName { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
             _userManager.Users.ToList();
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }
        //public IActionResult Test(int a)
        //{
        //    return (IActionResult)(ViewData["test"] = "d");
        //}
        public async Task<IActionResult> OnPostAsync(InputModel Input)
        {
            string returnUrl = null;
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    
                    //var defaultrole = _roleManager.FindByNameAsync("Default").Result;
            
                        IdentityResult roleresult = await _userManager.AddToRoleAsync(user, Input.RoleName);
                   
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var username = Input.Email;
                    var password = Input.Password;
                  var  callbackUrl = Url.Page(
                   "/Account/ConfirmEmail",
                   pageHandler: null,
                   values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                   protocol: Request.Scheme);
    
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"<h3>Congratulation ! You are registered.</h3><br>" +
                        $"<p>Thank you for showing your interest in my website.</p>"+
                        $"Please confirm your account by Clicking Button Below. <br> <p>_____________________________________________</p><br><a style='display: block; width: 115px; height: 25px;  background: #4E9CAF; padding: 10px;text-align: center; border-radius: 5px;color: white; font-weight: bold;line-height: 25px;' href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Registration</a><br>" +
                        $"<h3>Please Keep Your Credentials Confedential for future use.</h3><br>"+
                        $"<p>UserName={HtmlEncoder.Default.Encode(username)} <br> Password={HtmlEncoder.Default.Encode(password)}</p>.");
                    ViewData["Message"] ="Regsistration Succesfully";
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        //await _emailSender.SendEmailAsync(Input.Email, "Registation Successfull", $"<h2>Thank You For Registration Successfully.</h2>");
                        //TempData["RegistrationSuccess"] = "Registration Successfull";
                        // return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                       // return RedirectToAction("Login","Identity");
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                       // return RedirectToPage("Login");
                     //  return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
          
            return Page();
        }
    }
}
