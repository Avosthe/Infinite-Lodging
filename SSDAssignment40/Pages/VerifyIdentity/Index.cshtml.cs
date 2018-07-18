using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages.VerifyIdentity
{
    public class IndexModel : PageModel
    {
        public UserManager<Lodger> _userManager { get; set; }
        public SignInManager<Lodger> _signinManager { get; set; }
        [TempData]
        public string userAlertMessage {get; set;}
        public ApplicationDbContext _context { get; set; }
        public IndexModel(UserManager<Lodger> userManager, SignInManager<Lodger> signinManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            Lodger LodgerUser = await _userManager.FindByIdAsync(userId);
            if(!(LodgerUser is Lodger))
            {
                return StatusCode(403);
            }
            if(LodgerUser.AdditionalVerificationSecret == code)
            {
                LodgerUser.RequireAdditionalVerification = false;
                LodgerUser.AdditionalVerificationSecret = "";
                LodgerUser.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                //await _signinManager.SignInAsync(LodgerUser, false, null);
                //cant do the above because there are still multiple checks after sign in
                //also prevents hacker from signing in if he/she has access to the email
                userAlertMessage = $"New IP Address: {LodgerUser.IPAddress} confirmed! We can't log you in now, due to security reasons.";
                await _context.SaveChangesAsync();
                return RedirectToPage("/Index");
            }
            userAlertMessage = "Failed to verify identity!";
            return RedirectToPage("/Index");
        }
    }
}