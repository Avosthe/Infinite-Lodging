using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SSDAssignment40.Data;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using Microsoft.AspNetCore.Http;

namespace SSDAssignment40.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    [ValidateRecaptcha]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Lodger> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<Lodger> signInManager, ILogger<LoginModel> logger, UserManager<Lodger> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        public UserManager<Lodger> _userManager { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string userAlertMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name="Username")]
            [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Please enter valid username.")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                Lodger user = await _userManager.FindByNameAsync(Input.UserName);
                if(user != null)
                {
                    if (user.is3AuthEnabled == "True")
                    {
                        HttpContext.Session.SetString("Username", Input.UserName);
                        HttpContext.Session.SetString("Password", Input.Password);
                        HttpContext.Session.SetString("RememberMe", Input.RememberMe.ToString());
                        return RedirectToPage("/3AuthVerification", new { area = ""});
                    }
                }
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    userAlertMessage = "You have successfully logged in.";
                    return LocalRedirect(returnUrl);
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
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "Please verify your email address first!");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
