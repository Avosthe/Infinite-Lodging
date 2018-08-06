using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SSDAssignment40.Pages;
using SSDAssignment40.Data;
using Microsoft.EntityFrameworkCore;

namespace SSDAssignment40.Pages.Checkout
{
    public class PaymentOptionsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PaymentOptionsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Listing Listing { get; set; }

        public DateTime dateStart { get; set; }

        public DateTime dateEnd { get; set; }

        public IList<CreditCard> CreditCard { get; set; }

        public double Price { get; set; }

        public double datediff { get; set; }

        public double Cleaningfee { get; set; }

        public double TotalPrice { get; set; }

        public double BPrice { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, DateTime startDate, DateTime endDate)
        {
            if (id == null || startDate == null || endDate == null)
            {
                return NotFound();
            }

            CreditCard = await _context.CreditCard.ToListAsync();

            Listing = await _context.Listing.FirstOrDefaultAsync(m => m.ListingId == id);

            Price = Listing.Price;

            dateStart = startDate;

            dateEnd = endDate;

            datediff = (endDate - startDate).TotalDays;

            Cleaningfee = Price * 0.05;

            BPrice = (Price * datediff);

            TotalPrice = (Price * datediff) + Cleaningfee;

            return Page();
        }
    }
}
