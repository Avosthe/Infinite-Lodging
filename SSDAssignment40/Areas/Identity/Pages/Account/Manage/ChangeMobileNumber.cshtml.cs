using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SSDAssignment40.Data;

namespace SSDAssignment40.Areas.Identity.Pages.Account.Manage
{
    public class ChangeMobileNumberModel : PageModel
    {

        [BindProperty]
        [Required]
        [Display(Name = "Mobile Number")]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "Please enter a valid mobile number!")]
        public string InputMobileNumber { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Verification Code")]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "Please enter a valid code!")]
        public string InputRandomCode { get; set; }

        public bool hasSentVerificationCode { get; set; }

        public string StatusMessage { get; set; }
        public UserManager<Lodger> _userManager { get; set; }
        public ISmsSender _smsSender { get; set; }
        public ApplicationDbContext _context { get; set; }

        public ChangeMobileNumberModel(UserManager<Lodger> userManager, ISmsSender smsSender, ApplicationDbContext context)
        {
            _userManager = userManager;
            _smsSender = smsSender;
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            InputMobileNumber = phoneNumber;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("InputRandomCode");
            if (ModelState.IsValid)
            {
                var LodgerUser = await _userManager.GetUserAsync(User);
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
                var phoneNumber = await _userManager.GetPhoneNumberAsync(LodgerUser);
                if (!(InputMobileNumber.Equals(phoneNumber)))
                {
                    HttpContext.Session.SetString("MobileNumber", "65" + InputMobileNumber);
                    Random randObj = new Random();
                    string verificationCode = randObj.Next(999999).ToString();
                    HttpContext.Session.SetString("RandomCode", verificationCode);
                    if (!(_smsSender.SendSms("65" + InputMobileNumber, $"Your Verification Code for InfiniteLodging is {verificationCode}")))
                    {
                        HttpContext.Session.Remove("MobileNumber");
                        StatusMessage = "Verification Code Sent!";
                        hasSentVerificationCode = true;
                        return Page();
                    }
                }
                StatusMessage = "Changes have been updated successfully!";
                AuditRecord auditRecord = new AuditRecord();
                auditRecord.AuditActionType = "Changed Phone Number";
                auditRecord.AuditRecordId = Guid.NewGuid().ToString();
                auditRecord.DateTimeStamp = DateTime.Now;
                auditRecord.PerformedBy = LodgerUser;
                auditRecord.IPAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                ur.AuditRecord = auditRecord;
                _context.UserReverts.Add(ur);
                await _context.SaveChangesAsync();
                return Page();
            }
            return Page();
        }
        
        public async Task<IActionResult> OnPostVerifyCodeAsync()
        {
            ModelState.Remove("InputMobileNumber");
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("RandomCode").Equals(InputRandomCode))
                {
                    HttpContext.Session.Remove("RandomCode");
                    var user = await _userManager.GetUserAsync(User);
                    await _userManager.SetPhoneNumberAsync(user, HttpContext.Session.GetString("MobileNumber"));
                    HttpContext.Session.Remove("MobileNumber");
                    StatusMessage = "Phone Number updated successfully!";
                }
                return Page();
            }
            return Page();
        }

    }
}