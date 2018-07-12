using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using SSDAssignment40.Data;
using Microsoft.Extensions.Logging;

namespace SSDAssignment40.Pages
{
    public class VerifyMobileModel : PageModel
    {
        //[TempData]
        //public string RandomCode { get; set; }
        //[TempData]
        //public string MobileNumber { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Verification Code")]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "Please enter a valid code!")]
        public string InputRandomCode { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Mobile Number")]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "Please enter a valid mobile number!")]
        public string InputMobileNumber { get; set; }

        public ISmsSender _smsSender { get; set; }

        public ILogger<VerifyMobileModel> _logger { get; set; }

        public string ErrorMessage { get; set; }

        public VerifyMobileModel([FromServices] ISmsSender smsSender, ILogger<VerifyMobileModel> logger)
        {
            _smsSender = smsSender;
            _logger = logger;
        }

        public void OnGet()
        {
            foreach (var d in TempData.ToList())
            {
                _logger.LogInformation($"Key: {d.Key}, Value: {d.Value}");
            }
        }

        public IActionResult OnPostAsync()
        {
            ModelState.Remove("InputRandomCode");

            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("MobileNumber", "65" + InputMobileNumber);

                Random randObj = new Random();
                string verificationCode = randObj.Next(999999).ToString();

                HttpContext.Session.SetString("RandomCode", verificationCode);
                if(!(_smsSender.SendSms("65" + InputMobileNumber, $"Your Verification Code for InfiniteLodging is {verificationCode}")))
                {
                    HttpContext.Session.Remove("MobileNumber");
                    return RedirectToPage("/Register/VerifyMobile");
                }

                return Page();
            }
            return Page();
        }

        public IActionResult OnPostVerifyRandomCodeAsync()
        {
            ModelState.Remove("InputMobileNumber");
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("RandomCode").Equals(InputRandomCode))
                {
                    HttpContext.Session.Remove("RandomCode");
                    return RedirectToPage("/Account/Register", new { area = "Identity" });
                }
                return Page();
            }
            return Page();
        }
    }
}