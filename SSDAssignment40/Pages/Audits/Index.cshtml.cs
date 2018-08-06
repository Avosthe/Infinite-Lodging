using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages.Audits
{

    public class IndexModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public IndexModel(SSDAssignment40.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<AuditRecord> AuditRecord { get;set; }
        [BindProperty]
        [Required]
        public string auditRecordId { get; set; }

        public async Task OnGetAsync()
        {
            AuditRecord = await (from a in _context.AuditRecords orderby a.DateTimeStamp descending select a).ToListAsync();
        }
        public async Task<IActionResult> OnPostRevertChangesAsync()
        {
            if (!(ModelState.IsValid))
            {
                return Page();
            }
            AuditRecord usingAuditRecord = _context.AuditRecords.First(ar => (ar.AuditRecordId == auditRecordId));
            if(usingAuditRecord == null)
            {
                ModelState.AddModelError("Error!", "Audit Record Not Found!");
                return Page();
            }
            Lodger currentUser = _context.Users.First(u => (u.Id == usingAuditRecord.PerformedBy.Id));
            if(!(currentUser is Lodger))
            {
                ModelState.AddModelError("Error!", "User Not Found!");
                return Page();
            }
            RevertChanges rc = _context.RevertChanges.First(rc2 => (rc2.AuditRecord.AuditRecordId == usingAuditRecord.AuditRecordId));
            currentUser = rc.OldLodgerUser;
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
