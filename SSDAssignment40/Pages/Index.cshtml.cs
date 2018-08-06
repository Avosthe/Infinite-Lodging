using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SSDAssignment40.Pages
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string userAlertMessage { get; set; }
        public UserManager<Lodger> _userManager { get; set; }
        public SignInManager<Lodger> _signInManager { get; set; }

        public IndexModel(UserManager<Lodger> userManager, SignInManager<Lodger> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Lodger LodgerUser = await _userManager.GetUserAsync(User);
                if(HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() != LodgerUser.IPAddress)
                {
                    await _signInManager.SignOutAsync();
                    userAlertMessage = "New IP Address detected! To preventing spoofing attacks, you have been logged out! Please check your email to verify your identity!";
                }
                return Page();
            }
            return Page();
        }

    }
}
