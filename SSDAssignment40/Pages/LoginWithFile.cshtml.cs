using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages
{
    public class LoginWithFileModel : PageModel
    {
        [BindProperty]
        [Required]
        public string UserName { get; set; }
        [BindProperty]
        [Required]
        public IFormFile SecretFile { get; set; }
        public UserManager<Lodger> _userManager { get; set; }
        public SignInManager<Lodger> _signInManager { get; set; }
        public Lodger LodgerUser { get; set; }
        public string userAlertMessage { get; set; }
        public ApplicationDbContext _context { get; set; }
        public IEmailSender _emailSender { get; set; }
        public LoginWithFileModel(UserManager<Lodger> userManager, SignInManager<Lodger> signInManager, ApplicationDbContext context, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
        }
        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated) // check later
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            string verboseError = "Invalid username or file! Please also check you have file authentication enabled!";
            if (!(ModelState.IsValid))
            {
                return Page();
            }
            LodgerUser = await _userManager.FindByNameAsync(UserName);
            if (LodgerUser == null)
            {
                ModelState.AddModelError("Error!", verboseError);
                return Page();
            }
            string SecretFileHashString = "";
            using (var ms = new MemoryStream())
            {
                SecretFile.CopyTo(ms);
                byte[] SecretFileBytes = ms.ToArray();
                byte[] SecretFileHashBytes = SHA512.Create().ComputeHash(SecretFileBytes);
                SecretFileHashString = Encoding.UTF8.GetString(SecretFileHashBytes);
            };
            if (LodgerUser.secretFileVerificationHash != SecretFileHashString)
            {
                ModelState.AddModelError("Error!", verboseError);
                return Page();
            }
            if (LodgerUser.LockoutEnd != null)
            {
                ModelState.AddModelError(string.Empty, "Sorry! Account locked out!");
                return Page();
            }
            if (LodgerUser.RequireAdditionalVerification)
            {
                userAlertMessage = "Please log in to your registered email to verify your new IP Address";
                return RedirectToPage("/Index", new { area = "" });
            }
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
                return RedirectToPage("/Index", new { area = "" });
            }
            await _signInManager.SignInAsync(LodgerUser, false);
            return RedirectToPage("/Index", new { area = "" });
        }
}
}