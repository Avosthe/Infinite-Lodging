using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context, UserManager<Lodger> user)
        {
            _context = context;
            userManager = user;
        }

        [BindProperty]
        public IList<Review> ReviewList { get; set; }

        [Required]
        [BindProperty]
        public Review Review { get; set; }

        [Required]
        [BindProperty]
        public Booking Booking { get; set; }

        [BindProperty]
        public Listing Listing { get; set; }

        public UserManager<Lodger> userManager { get; set; }

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

            var reviews = from r in _context.Review where r.Listing.ListingId == id select r;
            ReviewList = await reviews.ToListAsync();

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string id)
        {
            Booking.Lodger = await userManager.GetUserAsync(User);
            Listing = await _context.Listing.FirstOrDefaultAsync(m => m.ListingId == id);

            Booking.Listing = Listing;
            _context.Booking.Add(Booking);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostSubmitReviewAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();

            }
            Review.Lodger = await userManager.GetUserAsync(User);
            Review.Listing = await _context.Listing.FirstOrDefaultAsync(m => m.ListingId == id);
            Review.DateTime = DateTime.Now;
            _context.Review.Add(Review);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Listings");
        }
    }
}
