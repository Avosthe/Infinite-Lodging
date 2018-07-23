using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;
using Microsoft.AspNetCore.Identity;

namespace SSDAssignment40.Pages.Support
{
    public class EditModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public EditModel(SSDAssignment40.Data.ApplicationDbContext context, UserManager<Lodger> userManager)
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
                    return RedirectToPage("/Error/NiceTry");
                }

                if (id == null)
                {
                    return NotFound();
                }

                CustomerSupport = await _context.CustomerSupport.FirstOrDefaultAsync(m => m.CustomerSupport_ID == id);

                if (Lodger.Id != CustomerSupport.Lodger.Id)
                {
                    return RedirectToPage("./Error/NiceTry");
                }

                CustomerSupport = await _context.CustomerSupport.FirstOrDefaultAsync(m => m.CustomerSupport_ID == id);

            if (CustomerSupport == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //CustomerSupport.Username = await _userManager.GetUserNameAsync(await (_userManager.GetUserAsync(User)));
            //CustomerSupport.DateTimeStamp = DateTime.Now;
            var username = (from l in _context.CustomerSupport where l.CustomerSupport_ID == id select l.Username).ToList();
            var datetime= (from l in _context.CustomerSupport where l.CustomerSupport_ID == id select l.DateTimeStamp).ToList();
            var noreplies = (from l in _context.CustomerSupport where l.CustomerSupport_ID == id select l.NoReplies).ToList();
            CustomerSupport.Username = username[0];
            CustomerSupport.DateTimeStamp = datetime[0];
            CustomerSupport.NoReplies = noreplies[0];
            _context.Attach(CustomerSupport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerSupportExists(CustomerSupport.CustomerSupport_ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CustomerSupportExists(int id)
        {
            return _context.CustomerSupport.Any(e => e.CustomerSupport_ID == id);
        }
    }
}
