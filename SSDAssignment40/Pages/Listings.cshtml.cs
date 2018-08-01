using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;
using Microsoft.AspNetCore.Authorization;

namespace SSDAssignment40.Pages
{
    public class ListingsModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public ListingsModel(SSDAssignment40.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Listing> Listing { get;set; }

        public IList<Review> Review { get; set; }

        public int avgRating { get; set; }

        public async Task OnGetAsync(string location, DateTime dateStart, DateTime dateEnd, string id)
        {
            //search by location from home page
            //var result = _context.Listing.AsQueryable();
            var listings = from l in _context.Listing where l.isSuspended == false select l;

            if (!String.IsNullOrEmpty(location))
            {
                listings = listings.Where(s => s.Location.Contains(location));
            }
            if (!String.IsNullOrEmpty(id))
            {
                listings = listings.Where(s => s.Lodger.Id.Contains(id));
            }

            Listing = await listings.ToListAsync();


            //calculating average for a specific listing 
            var reviews = from r in _context.Review select r;
            Review = await reviews.ToListAsync();
        }
    }
}
