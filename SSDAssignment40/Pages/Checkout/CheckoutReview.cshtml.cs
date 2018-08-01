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

namespace SSDAssignment40.Pages.Checkout
{
    public class CheckoutReviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CheckoutReviewModel(ApplicationDbContext context, UserManager<Lodger> user)
        {
            _context = context;
            userManager = user;
        }
        [Required]
        [BindProperty]
        public Booking Booking { get; set; }

        public Lodger Lodger { get; set; }

        //[BindProperty]
        public Listing Listing { get; set; }

        public DateTime dateStart { get; set; }

        public DateTime dateEnd { get; set; }

        public UserManager<Lodger> userManager { get; set; }

        public async Task OnGetAsync(string id, DateTime startDate, DateTime endDate)
        {
            Listing = await _context.Listing.FirstOrDefaultAsync(m => m.ListingId == id);

            dateStart = startDate;

            dateEnd = endDate;
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
    }
}