using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;

namespace SSDAssignment40
{
    public class MyBookingsModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public UserManager<Lodger> _userManager { get; set; }

        public MyBookingsModel(SSDAssignment40.Data.ApplicationDbContext context, UserManager<Lodger> usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }

        public IList<Booking> Booking { get;set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (_userManager.GetUserId(User) == null)
            {
                return NotFound();
            }

            var bookings = from r in _context.Booking where r.Lodger.Id == _userManager.GetUserId(User) select r;
            Booking = await bookings.ToListAsync();
            return Page();
        }
    }
}
