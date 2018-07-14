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

        public ChangeMobileNumberModel(UserManager<Lodger> userManager, ISmsSender smsSender)
        {
            _userManager = userManager;
            _smsSender = smsSender;
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
                var user = await _userManager.GetUserAsync(User);
                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
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