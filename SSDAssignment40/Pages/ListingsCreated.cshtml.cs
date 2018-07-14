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
    public class ListingsCreatedModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public ListingsCreatedModel(SSDAssignment40.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Listing> Listing { get;set; }

        public IList<Review> Review { get; set; }

        public int avgRating { get; set; }

        public async Task OnGetAsync(string location)
        {
            //search by location from home page
            var listings = from l in _context.Listing select l;

            if (!String.IsNullOrEmpty(location))
            {
                listings = listings.Where(s => s.Location.Contains(location));
            }

            Listing = await listings.ToListAsync();


            //calculating average for a specific listing 
            var average = from r in _context.Review select r;
        }
    }
}
