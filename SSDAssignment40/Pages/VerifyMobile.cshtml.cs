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

namespace SSDAssignment40.Pages
{
    public class VerifyMobileModel : PageModel
    {
        [TempData]
        public string RandomCode { get; set; }
        [TempData]
        public string MobileNumber { get; set; }

        [BindProperty]
        [Required]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "Please enter a valid code!")]
        public string InputRandomCode { get; set; }

        [BindProperty]
        [Required]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "Please enter a valid phone number!")]
        public string InputMobileNumber { get; set; }

        public SmsSender _smsSender { get; set; }

        public VerifyMobileModel(SmsSender smsSender)
        {
            _smsSender = smsSender;
        }
        public async Task<IActionResult> OnPostVerifyAsync()
        {
            ModelState.Remove("InputRandomCode");
            if (ModelState.IsValid)
            {
                MobileNumber = InputMobileNumber;


                //var cookieOptions = new CookieOptions();
                //cookieOptions.Expires = DateTime.Now.AddMinutes(1);
                //cookieOptions.Secure = true;

                //Response.Cookies.Append("MobileNumber", InputMobileNumber, cookieOptions);

                // generate random code
                RandomCode = "123456";

                return Page();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostVerifyRandomCodeAsync()
        {
            ModelState.Remove("InputMobileNumber");
            if (ModelState.IsValid)
            {
                if (RandomCode.Equals(InputRandomCode))
                {
                    return RedirectToPage("/Account/Register", new { area = "Identity" });
                }
                //MobileNumber = Request.Cookies.Where(c => c.Key == "MobileNumber").First().Value;
                //Response.Cookies.Delete("MobileNumber");

                return Page();
            }
            return Page();
        }
    }
}