using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public DetailsModel(SSDAssignment40.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Listing Listing { get; set; }

        public IList<Lodger> Lodger { get; set; }

        public Booking Booking { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {

            var lodgers = from u in _context.Users select u;
            Lodger = await lodgers.ToListAsync();

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
    }
}
