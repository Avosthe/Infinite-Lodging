using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public DetailsModel(SSDAssignment40.Data.ApplicationDbContext context, UserManager<Lodger> user)
        {
            _context = context;
            userManager = user;
        }

        [BindProperty]
        public Review Review { get; set; }

        [BindProperty]
        public Booking Booking { get; set; }

        public Listing Listing { get; set; }

        public UserManager<Lodger> userManager { get; set; }

        public string ReviewDescr { get; set; }

        public int Rating { get; set; }

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


        public async Task<IActionResult> OnPostAsync(string id)
        {
            Booking.Lodger = await userManager.GetUserAsync(User);
            Listing = await _context.Listing.FirstOrDefaultAsync(m => m.ListingId == id);

            Booking.Listing = Listing;
            _context.Booking.Add(Booking);
            await _context.SaveChangesAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostSubmitReviewAsync(string id)
        {
            Review.Lodger = await userManager.GetUserAsync(User);
            Review.Listing = await _context.Listing.FirstOrDefaultAsync(m => m.ListingId == id);
            Review.ReviewDesc = ReviewDescr;
            Review.Rating = Rating;
            return RedirectToPage("./Listings");
        }
    }
}
