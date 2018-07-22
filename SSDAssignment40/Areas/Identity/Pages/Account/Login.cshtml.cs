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
using Microsoft.AspNetCore.Identity.UI.Services;

namespace SSDAssignment40.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    [ValidateRecaptcha]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Lodger> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        public IEmailSender _emailSender { get; set; }
        public ApplicationDbContext _context { get; set; }

        public LoginModel(SignInManager<Lodger> signInManager, ILogger<LoginModel> logger, UserManager<Lodger> userManager, IEmailSender emailSender, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _emailSender = emailSender;
            _context = context;
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
            [Display(Name = "Username")]
            [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Please enter valid username.")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index", new { area = "" });
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                Lodger LodgerUser = await _userManager.FindByNameAsync(Input.UserName);
                if(!(LodgerUser is Lodger))
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return Page();
                }
                if (LodgerUser.is3AuthEnabled == "True")
                {
                    HttpContext.Session.SetString("Username", Input.UserName);
                    HttpContext.Session.SetString("Password", Input.Password);
                    HttpContext.Session.SetString("RememberMe", Input.RememberMe.ToString());
                    return RedirectToPage("/3AuthVerification", new { area = "" });
                }
                if (LodgerUser.RequireAdditionalVerification)
                {
                    userAlertMessage = "Please log in to your registered email to verify your new IP Address";
                }
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                if (LodgerUser.IPAddress != Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString())
                {
                    LodgerUser.RequireAdditionalVerification = true;
                    LodgerUser.AdditionalVerificationSecret = Guid.NewGuid().ToString();
                    var callbackUrl = Url.Page(
                        "/VerifyIdentity/Index",
                        pageHandler: null,
                        values: new { userId = LodgerUser.Id, code = LodgerUser.AdditionalVerificationSecret },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(LodgerUser.Email, "Verify your Identity",
                        $"<div style=width: 70%; margin: 0 auto;'><p><img style='display: block; margin-left: auto; margin-right: auto;' src='https://image.ibb.co/dyXbEy/test.png' alt='Infinite Lodging' width='198' height='94' /></p><h3 style='text-align: center;'>For security reasons, please verify your identity.</h3><p style='text-align: center;'><a href='{callbackUrl}'><img src='https://image.ibb.co/htz0EJ/airplane.png' alt='Verify your Identity' /></a></p><p style='text-align: center;'>&nbsp;</p><span style='color: #808080; font-size: small;'><em>This message was sent to {LodgerUser.Email}. You are receiving this because you're a &infin;Lodging member, or you've signed up to receive email from us. Manage your preferences or unsubscribe. </em></span></div>"
                        );
                    userAlertMessage = "We've detected that you are logging in from a new IP Address, confirm your identity using your registered email!";
                    await _context.SaveChangesAsync();
                    return RedirectToPage("/Index");
                }
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
