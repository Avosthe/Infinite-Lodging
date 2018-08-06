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

        [BindProperty]
        public IList<Review> ReviewList { get; set; }

        [Required]
        [BindProperty]
        public Review Review { get; set; }

        [Required]
        [BindProperty]
        public Booking Booking { get; set; }

        public Lodger Lodger { get; set; }

        //[BindProperty]
        public Listing Listing { get; set; }

        public DateTime dateStart { get; set; }

        public DateTime dateEnd { get; set; }

        public double TotalPrice { get; set; }

        public int Price { get; set; }

        public double datediff { get; set; }
        
        public double Cleaningfee { get; set; }

        public UserManager<Lodger> userManager { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, DateTime startDate, DateTime endDate)
        {
            if (id == null || startDate == null || endDate == null)
            {
                return NotFound();
            }
            
            var reviews = from r in _context.Review where r.Listing.ListingId == id select r;
            ReviewList = await reviews.ToListAsync();

            Listing = await _context.Listing.FirstOrDefaultAsync(m => m.ListingId == id);

            Price = Listing.Price;

            dateStart = startDate;

            dateEnd = endDate;

            datediff = (endDate - startDate).TotalDays;

            Cleaningfee = Price * 0.05;

            TotalPrice = (Price * datediff) + Cleaningfee;

            return Page();
        }
    }
}