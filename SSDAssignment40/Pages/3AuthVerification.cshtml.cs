using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages
{
    public class _3AuthVerificationModel : PageModel
    {
        public UserManager<Lodger> _userManager { get; set; }
        public SignInManager<Lodger> _signInManager { get; set; }
        [Required]
        [BindProperty]
        [Display(Name = "Keystroke Pattern")]
        public string patternInput { get; set; }
        [TempData]
        public string userAlertMessage { get; set; }

        public string ErrorMessage { get; set; }

        public _3AuthVerificationModel(UserManager<Lodger> userManager, SignInManager<Lodger> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Username") == null || HttpContext.Session.GetString("Password") == null || HttpContext.Session.GetString("RememberMe") == null)
            {
                return RedirectToPage("./Error/403");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Lodger toBeVerifiedAgainst = await _userManager.FindByNameAsync(HttpContext.Session.GetString("Username"));
                if (patternInput.Equals(toBeVerifiedAgainst.is3AuthPattern))
                {
                    string username = HttpContext.Session.GetString("Username");
                    string password = HttpContext.Session.GetString("Password");
                    bool rememberMe = HttpContext.Session.GetString("RememberMe") == "True" ? true : false;
                    var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: true);
                    if (result.Succeeded)
                    {
                        HttpContext.Session.Clear();
                        userAlertMessage = "You have successfully logged in.";
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        HttpContext.Session.Clear();
                        if (result.RequiresTwoFactor)
                        {
                            return RedirectToPage("/Account/LoginWith2fa", new { area = "Identity", ReturnUrl = "./Index", RememberMe = rememberMe });
                        }
                        if (result.IsLockedOut)
                        {
                            return RedirectToPage("/Account/Lockout", new { area = "Identity" });
                        }
                        else if (result.IsNotAllowed)
                        {
                            userAlertMessage = "Please verify your email address first!";
                            return RedirectToPage("/Index");
                        }
                        else
                        {
                            ErrorMessage = "Invalid username or password.";
                            return Page();
                        }
                    }
                }
                else
                {
                    if(HttpContext.Session.GetInt32("Tries") == null)
                    {
                        HttpContext.Session.SetInt32("Tries", 1);
                    }
                    else
                    {
                        var currentTries = HttpContext.Session.GetInt32("Tries");
                        HttpContext.Session.SetInt32("Tries", currentTries.Value + 1);
                        if(HttpContext.Session.GetInt32("Tries").Value == 4)
                        {
                            toBeVerifiedAgainst.LockoutEnabled = true;
                            toBeVerifiedAgainst.LockoutEnd = DateTime.Now.AddDays(1);
                            await _userManager.UpdateAsync(toBeVerifiedAgainst);
                            HttpContext.Session.Clear();
                            return RedirectToPage("/Account/Lockout", new { area = "Identity" });
                        }
                    }
                }
            }
            return Page();
        }
    }
}