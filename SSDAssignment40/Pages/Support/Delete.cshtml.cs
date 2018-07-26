using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages.Support
{
    public class DeleteModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public DeleteModel(SSDAssignment40.Data.ApplicationDbContext context, UserManager<Lodger> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public CustomerSupport CustomerSupport { get; set; }
        public UserManager<Lodger> _userManager { get; set; }
        public Lodger Lodger { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Lodger = await _userManager.GetUserAsync(User);

            if (Lodger == null)
            {
                var auditrecord = new AuditRecord();
                auditrecord.AuditActionType = "Anonymous User Tried To Delete Customer Support id:"+id+ " Record";
                auditrecord.DateTimeStamp = DateTime.Now;
                auditrecord.PerformedBy = null;
                auditrecord.AuditRecordId = Guid.NewGuid().ToString();
                auditrecord.IPAddress = HttpContext.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                _context.AuditRecords.Add(auditrecord);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Error/NiceTry");
            }

            if (id == null)
            {
                return NotFound();
            }

            CustomerSupport = await _context.CustomerSupport.FirstOrDefaultAsync(m => m.CustomerSupport_ID == id);

            if (Lodger.Id != CustomerSupport.Lodger.Id)
            {
                var user = await _userManager.GetUserAsync(User);
                var auditrecord = new AuditRecord();
                auditrecord.AuditActionType = "User Tried To Delete Another User's Customer Support" + id + " Record";
                auditrecord.DateTimeStamp = DateTime.Now;
                auditrecord.PerformedBy = user;
                auditrecord.AuditRecordId = Guid.NewGuid().ToString();
                auditrecord.IPAddress = HttpContext.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                _context.AuditRecords.Add(auditrecord);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Error/NiceTry");
            }

            CustomerSupport = await _context.CustomerSupport.FirstOrDefaultAsync(m => m.CustomerSupport_ID == id);

            if (CustomerSupport == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CustomerSupport = await _context.CustomerSupport.FindAsync(id);

            if (CustomerSupport != null)
            {
                _context.CustomerSupport.Remove(CustomerSupport);
                //await _context.SaveChangesAsync();

                if (await _context.SaveChangesAsync() > 0)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var auditrecord = new AuditRecord();
                    auditrecord.AuditActionType = "Delete Customer Support Record";
                    auditrecord.DateTimeStamp = DateTime.Now;
                    auditrecord.PerformedBy = user;
                    auditrecord.AuditRecordId = Guid.NewGuid().ToString();
                    auditrecord.IPAddress = HttpContext.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                    _context.AuditRecords.Add(auditrecord);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
