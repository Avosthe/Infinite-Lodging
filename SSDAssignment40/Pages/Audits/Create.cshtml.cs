using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages.Audits
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public UserManager<Lodger> _userManager { get; set; }
        public CreateModel(SSDAssignment40.Data.ApplicationDbContext context, UserManager<Lodger> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public AuditRecord AuditRecord { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            AuditRecord.IPAddress = HttpContext.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            AuditRecord.DateTimeStamp = DateTime.Now;
            AuditRecord.PerformedBy = await _userManager.GetUserAsync(User);
            _context.AuditRecords.Add(AuditRecord);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}