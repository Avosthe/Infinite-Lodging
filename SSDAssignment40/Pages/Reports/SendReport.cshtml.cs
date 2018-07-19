using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages.Reports
{
    [Authorize]
    public class SendReportModel : PageModel
    {
        public ApplicationDbContext _context { get; set; }
        public UserManager<Lodger> _userManager { get; set; }
        public SendReportModel(UserManager<Lodger> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            string ReportedUserId = HttpContext.Session.GetString("ReportedUserId");
            if (string.IsNullOrEmpty(ReportedUserId))
            {
                return StatusCode(403);
            }
            string Reason = HttpContext.Session.GetString("Reason");
            string EvidenceFileName = HttpContext.Session.GetString("EvidenceFileName");
            UserReport ur = new UserReport()
            {

                UserReportId = Guid.NewGuid().ToString(),
                ReportedUser = await _userManager.FindByIdAsync(ReportedUserId),
                ReportingUser = await _userManager.GetUserAsync(User),
                ReportedContent = Reason,
                ReportFileEvidence = EvidenceFileName,
                TimeStamp = DateTime.Now
            };
            _context.UserReport.Add(ur);
            await _context.SaveChangesAsync();
            HttpContext.Session.Clear();
            return Page();
        }
    }
}