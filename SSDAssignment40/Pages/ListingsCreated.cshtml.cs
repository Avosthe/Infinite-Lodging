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

        public async Task OnGetAsync()
        {
            Listing = await _context.Listing.ToListAsync();
        }
    }
}
