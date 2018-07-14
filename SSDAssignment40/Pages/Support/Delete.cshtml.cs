using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages.Support
{
    public class DeleteModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public DeleteModel(SSDAssignment40.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CustomerSupport CustomerSupport { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //CustomerSupport = await _context.CustomerSupport.FirstOrDefaultAsync(m => m.CustomerSupport_ID == id);

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

            //CustomerSupport = await _context.CustomerSupport.FindAsync(id);

            if (CustomerSupport != null)
            {
                //_context.CustomerSupport.Remove(CustomerSupport);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
