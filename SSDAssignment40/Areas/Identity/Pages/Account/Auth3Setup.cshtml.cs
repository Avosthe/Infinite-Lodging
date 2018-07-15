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

namespace SSDAssignment40.Areas.Identity.Pages.Account
{
    public class Auth3SetupModel : PageModel
    {
        public string ErrorMessage { get; set; }
        [Required]
        [BindProperty]
        [Display(Name = "Keystroke Pattern")]
        public string patternInput { get; set; }

        [TempData]
        public string userAlertMessage { get; set; }
        public UserManager<Lodger> _userManager { get; set; }
        public ApplicationDbContext _dbContext { get; set; }

        public Auth3SetupModel(UserManager<Lodger> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("3AuthSetup") != "True")
            {
                return RedirectToPage("Manage/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            _dbContext.Update(user);
            user.is3AuthEnabled = "True";
            user.is3AuthPattern = patternInput;
            await _dbContext.SaveChangesAsync();
            HttpContext.Session.Clear();
            userAlertMessage = "You have successfully added 3 Factor Authentication";
            return RedirectToPage("/Index", new { area = "" });
        }
    }
}