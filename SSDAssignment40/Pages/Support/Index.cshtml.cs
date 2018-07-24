using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;
using Microsoft.AspNetCore.Authorization;

namespace SSDAssignment40.Pages.Support
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public IndexModel(SSDAssignment40.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CustomerSupport> CustomerSupport { get;set; }

        public async Task OnGetAsync(string searchString)
        {
            CustomerSupport = await _context.CustomerSupport.ToListAsync();

            var requests = from m in _context.CustomerSupport
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                requests = requests.Where(s => s.Request.Contains(searchString));
            }

            CustomerSupport = await requests.ToListAsync();
        }
    }
}
