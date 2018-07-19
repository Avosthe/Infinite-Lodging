using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SSDAssignment40.Data;
using Microsoft.AspNetCore.Http;

namespace SSDAssignment40.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Lodger> _signInManager;
        private readonly UserManager<Lodger> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<Lodger> userManager,
            SignInManager<Lodger> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string userAlertMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Username")]
            [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Please enter valid username.")]
            public string UserName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public IActionResult OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("MobileNumber"))) return RedirectToPage("/Register/VerifyMobile");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            var phoneNumber = HttpContext.Session.GetString("MobileNumber");
            if (phoneNumber == null) return RedirectToPage("/Index");
            HttpContext.Session.Clear();

            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                Lodger x = await _userManager.FindByEmailAsync(Input.Email);
                if (x == null) goto code;
                bool isConfirmed = await _userManager.IsEmailConfirmedAsync(x);
                if (isConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Sorry! The email address is already registered.");
                    return Page();
                }
                code:
                var user = new Lodger { UserName = Input.UserName, Email = Input.Email, PhoneNumber = phoneNumber, PhoneNumberConfirmed = true, City = "Singapore", Country = "Singapore", Biography = "This user has not added their biography.", DateJoined = DateTime.Now, Gender = "Male", IPAddress = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()};
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"<div style=width: 70%; margin: 0 auto;'><p><img style='display: block; margin-left: auto; margin-right: auto;' src='https://image.ibb.co/dyXbEy/test.png' alt='Infinite Lodging' width='198' height='94' /></p><h3 style='text-align: center;'>For security reasons, please verify your email.</h3><p style='text-align: center;'><a href='{HtmlEncoder.Default.Encode(callbackUrl)}'><img src='https://image.ibb.co/fNSJjJ/send.png' alt='Confirm Email' width='344' height='43' /></a></p><p style='text-align: center;'>&nbsp;</p><span style='color: #808080; font-size: small;'><em>This message was sent to {Input.Email}. You are receiving this because you're a &infin;Lodging member, or you've signed up to receive email from us. Manage your preferences or unsubscribe. </em></span></div>");
                    userAlertMessage = "Please verify your email address first before logging in!";

                    return LocalRedirect(returnUrl);
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
