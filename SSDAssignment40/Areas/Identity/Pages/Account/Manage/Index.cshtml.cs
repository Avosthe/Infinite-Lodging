using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SSDAssignment40.Data;

namespace SSDAssignment40.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Lodger> _userManager;
        private readonly SignInManager<Lodger> _signInManager;
        private readonly IEmailSender _emailSender;

        public IndexModel(
            UserManager<Lodger> userManager,
            SignInManager<Lodger> signInManager,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _context = context;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string userAlertMessage { get; set; }

        public ApplicationDbContext _context { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Lodger LodgerUser = await _userManager.GetUserAsync(User);
            if (LodgerUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            UserRevert ur = new UserRevert()
            {
                UserRevertId = Guid.NewGuid().ToString(),
                FullName = LodgerUser.FullName,
                Gender = LodgerUser.Gender,
                AlternateEmail = LodgerUser.AlternateEmail,
                Country = LodgerUser.Country,
                City = LodgerUser.City,
                Occupation = LodgerUser.Occupation,
                Address = LodgerUser.Address,
                GovernmentID = LodgerUser.GovernmentID,
                Status = LodgerUser.Status,
                Biography = LodgerUser.Biography,
                Hobbies = LodgerUser.Hobbies,
                Email = LodgerUser.Email,
                PasswordHash = LodgerUser.PasswordHash,
                PhoneNumber = LodgerUser.PhoneNumber,
                PhoneNumberConfirmed = LodgerUser.PhoneNumberConfirmed,
                is3AuthEnabled = LodgerUser.is3AuthEnabled
            };
            var email = await _userManager.GetEmailAsync(LodgerUser);
            if (Input.Email != email)
            {
                //var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                LodgerUser.EmailConfirmed = false;
                var userId = await _userManager.GetUserIdAsync(LodgerUser);
                var code = await _userManager.GenerateChangeEmailTokenAsync(LodgerUser, Input.Email);
                var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code, newEmail = Input.Email},
                protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Confirm your email",
                    $"<div style=width: 70%; margin: 0 auto;'><p><img style='display: block; margin-left: auto; margin-right: auto;' src='https://image.ibb.co/dyXbEy/test.png' alt='Infinite Lodging' width='198' height='94' /></p><h3 style='text-align: center;'>For security reasons, please verify your new email address.</h3><p style='text-align: center;'><a href='{HtmlEncoder.Default.Encode(callbackUrl)}'><img src='https://image.ibb.co/gEX9mo/Logo_Makr_0k_Wnu_O.png' alt='Confirm Email' width='344' height='43' /></a></p><p style='text-align: center;'>&nbsp;</p><span style='color: #808080; font-size: small;'><em>This message was sent to {Input.Email}. You are receiving this because you're a &infin;Lodging member, or you've signed up to receive email from us. Manage your preferences or unsubscribe. </em></span></div>");
                userAlertMessage = "Please verify your new email address before logging in!";
                AuditRecord auditRecord = new AuditRecord();
                auditRecord.AuditActionType = "Changed Email";
                auditRecord.AuditRecordId = Guid.NewGuid().ToString();
                auditRecord.DateTimeStamp = DateTime.Now;
                auditRecord.PerformedBy = LodgerUser;
                auditRecord.IPAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                ur.AuditRecord = auditRecord;
                _context.UserReverts.Add(ur);
                await _context.SaveChangesAsync();
                await _context.SaveChangesAsync();
                await _signInManager.SignOutAsync();
                return RedirectToPage("/Index", new { area = ""});
                //if (!setEmailResult.Succeeded)
                //{
                //    var userId = await _userManager.GetUserIdAsync(user);
                //    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                //}
            }

            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            //if (Input.PhoneNumber != phoneNumber)
            //{
            //    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            //    if (!setPhoneResult.Succeeded)
            //    {
            //        var userId = await _userManager.GetUserIdAsync(user);
            //        throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
            //    }
            //}

            await _signInManager.RefreshSignInAsync(LodgerUser);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"<div style=width: 70%; margin: 0 auto;'><p><img style='display: block; margin-left: auto; margin-right: auto;' src='https://image.ibb.co/dyXbEy/test.png' alt='Infinite Lodging' width='198' height='94' /></p><h3 style='text-align: center;'>For security reasons, please verify your email.</h3><p style='text-align: center;'><a href='{HtmlEncoder.Default.Encode(callbackUrl)}'><img src='https://image.ibb.co/gEX9mo/Logo_Makr_0k_Wnu_O.png' alt='Confirm Email' width='344' height='43' /></a></p><p style='text-align: center;'>&nbsp;</p><span style='color: #808080; font-size: small;'><em>This message was sent to {Input.Email}. You are receiving this because you're a &infin;Lodging member, or you've signed up to receive email from us. Manage your preferences or unsubscribe. </em></span></div>");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
