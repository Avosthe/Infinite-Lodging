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
            Lodger cu = _context.Users.First(u => (u.Id == usingAuditRecord.PerformedBy.Id));
            if(!(cu is Lodger))
            {
                ModelState.AddModelError("Error!", "User Not Found!");
                return Page();
            }
            UserRevert rt = _context.UserReverts.First(ur => (ur.AuditRecord.AuditRecordId == usingAuditRecord.AuditRecordId));
            if(rt == null)
            {
                ModelState.AddModelError("Error!", "Revert Backup not found!");
                return Page();
            }
            cu.FullName = rt.FullName;
            cu.Gender = rt.Gender;
            cu.AlternateEmail = rt.AlternateEmail;
            cu.Country = rt.Country;
            cu.City = rt.City;
            cu.Occupation = rt.Occupation;
            cu.Address = rt.Address;
            cu.GovernmentID = rt.GovernmentID;
            cu.Status = rt.Status;
            cu.Biography = rt.Biography;
            cu.Hobbies = rt.Hobbies;
            cu.Email = rt.Email;
            cu.PasswordHash = rt.PasswordHash;
            cu.PhoneNumber = rt.PhoneNumber;
            cu.PhoneNumberConfirmed = rt.PhoneNumberConfirmed;
            cu.is3AuthEnabled = rt.is3AuthEnabled;
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
