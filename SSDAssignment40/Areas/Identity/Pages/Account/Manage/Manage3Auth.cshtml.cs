using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SSDAssignment40.Data;

namespace SSDAssignment40.Areas.Identity.Pages.Account.Manage
{
    public class Manage3AuthModel : PageModel
    { 
        public UserManager<Lodger> _userManager { get; set; }
        public string StatusMessage {get; set;}

        public ApplicationDbContext _dbContext { get; set; }

        public Manage3AuthModel(UserManager<Lodger> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostDisable3AuthAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            _dbContext.Update(user);
            if(user.is3AuthEnabled == "True")
            {
                user.is3AuthEnabled = "False";
                user.is3AuthPattern = "";
                await _dbContext.SaveChangesAsync();
                StatusMessage = "Profile changes updated successfully!";
            }
            AuditRecord ar = new AuditRecord();
            ar.AuditActionType = "Disabled 3-Factor-Authentication";
            ar.AuditRecordId = Guid.NewGuid().ToString();
            ar.DateTimeStamp = DateTime.Now;
            ar.PerformedBy = user;
            ar.IPAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            await _dbContext.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostEnable3AuthAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user.is3AuthEnabled == "False")
            {
                HttpContext.Session.SetString("3AuthSetup", "True");
                return RedirectToPage("../Auth3Setup");
            }
            AuditRecord ar = new AuditRecord();
            ar.AuditActionType = "Enabled 3-Factor-Authentication";
            ar.AuditRecordId = Guid.NewGuid().ToString();
            ar.DateTimeStamp = DateTime.Now;
            ar.PerformedBy = user;
            ar.IPAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            await _dbContext.SaveChangesAsync();
            return Page();
        }
    }
}