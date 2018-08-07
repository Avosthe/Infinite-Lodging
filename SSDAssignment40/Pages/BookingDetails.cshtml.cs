using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages
{
    public class DetailsModelBooking : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public DetailsModelBooking(SSDAssignment40.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Booking Booking { get; set; }

        public int TotalPrice { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Booking = await _context.Booking.FirstOrDefaultAsync(m => m.BookingId == id);

            if (Booking == null)
            {
                return NotFound();
            }

            TotalPrice = Booking.Listing.Price * (int)(Booking.DateEnd - Booking.DateStart).TotalDays;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Booking = await _context.Booking.FindAsync(id);

            if (Booking != null)
            {
                _context.Booking.Remove(Booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./MyBookings");
        }
    }
}
