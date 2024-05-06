using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace OnlineBookShopping.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        string username = null;
        string password = null;
        bool rememberme;
        private IHttpContextAccessor Accessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<IdentityUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager, IHttpContextAccessor _accessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.Accessor = _accessor;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
           
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            string username = this.Accessor.HttpContext.Request.Cookies["UserName"];
           string password = this.Accessor.HttpContext.Request.Cookies["Password"];
            string rememberme = this.Accessor.HttpContext.Request.Cookies["rememberme"];
            TempData["UserName"] = username != null ? username :null;
            TempData["Password"] = password != null ? password : null;
            TempData["RememberMe"] = rememberme != null ? rememberme : null;
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
       
            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
           
        
            if (ModelState.IsValid)
            {

                if (Input.RememberMe)
                {
                    username = Input.Email;
                    password = Input.Password;
                    rememberme = true;
                    CookieOptions option = new CookieOptions();
                    // option.Expires = DateTime.Now.AddMilliseconds(10000);
                  
                    Response.Cookies.Append("UserName", username,option);
                    Response.Cookies.Append("Password", password, option);
                    Response.Cookies.Append("rememberme", rememberme.ToString(), option);
                
                }
                else
                {
                    Response.Cookies.Delete("UserName");
                    Response.Cookies.Delete("Password");
                    Response.Cookies.Delete("rememberme");
                }
                //var UserName = _userManager.GetUserName(User).ToString();
                //var UserPassword = _userManager.FindByNameAsync(UserName);
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                var user = await _userManager.FindByEmailAsync(Input.Email);
                // Get the roles for the user
                var roles = await _userManager.GetRolesAsync(user);
                string test = null;
                if (Convert.ToBoolean(roles.Count))
                {
                    test = roles?[0].ToString();
                }

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    if (!string.IsNullOrEmpty(test) && test == "Customer")
                    {
                        //return LocalRedirect(returnUrl);
                        return Redirect(@"../../Book/BookDisplay");
                    }
                    if (!string.IsNullOrEmpty(test) && test=="Admin")
                    {
                        // return LocalRedirect(returnUrl);
                        //return RedirectToAction("Index");
                        return Redirect(@"../../Book/Index");
                    }
                    return Redirect(@"../../Book/BookDisplay");
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
