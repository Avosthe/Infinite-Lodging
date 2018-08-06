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
            if (LodgerUser.is3AuthEnabled == "True")
            {
                LodgerUser.is3AuthEnabled = "False";
                LodgerUser.is3AuthPattern = "";
                await _dbContext.SaveChangesAsync();
                StatusMessage = "Profile changes updated successfully!";
            }
            AuditRecord auditRecord = new AuditRecord();
            auditRecord.AuditActionType = "Disabled 3-Factor-Authentication";
            auditRecord.AuditRecordId = Guid.NewGuid().ToString();
            auditRecord.DateTimeStamp = DateTime.Now;
            auditRecord.PerformedBy = LodgerUser;
            auditRecord.IPAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            ur.AuditRecord = auditRecord;
            _dbContext.UserReverts.Add(ur);
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