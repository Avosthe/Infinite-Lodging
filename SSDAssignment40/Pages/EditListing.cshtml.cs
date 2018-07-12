using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages
{
    public class EditListingModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public EditListingModel(SSDAssignment40.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Listing Listing { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Listing = await _context.Listing.FirstOrDefaultAsync(m => m.ListingId == id);

            if (Listing == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Listing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListingExists(Listing.ListingId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./ListingsCreated");
        }

        private bool ListingExists(string id)
        {
            return _context.Listing.Any(e => e.ListingId == id);
        }
    }
}
