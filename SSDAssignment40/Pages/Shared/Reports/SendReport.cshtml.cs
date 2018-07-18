using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SSDAssignment40.Pages.Reports
{
    public class SendReportModel : PageModel
    {
        public IActionResult OnGet()
        {
            string ReportedUserId = HttpContext.Session.GetString("ReportedUserId");
            string Reason = HttpContext.Session.GetString("Reason");
            string EvidenceFileName = HttpContext.Session.GetString("EvidenceFileName");
            if ( (string.IsNullOrEmpty(ReportedUserId)) || (string.IsNullOrEmpty(Reason)) || (string.IsNullOrEmpty(EvidenceFileName)))
            {
                return StatusCode(403);
            }

            HttpContext.Session.Clear();
            return Page();
            
        }
    }
}