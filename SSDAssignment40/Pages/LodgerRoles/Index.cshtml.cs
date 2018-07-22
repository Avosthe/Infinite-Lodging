using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages.LodgerRoles
{

    public class IndexModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public IndexModel(SSDAssignment40.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<LodgerRole> LodgerRole { get;set; }

        public async Task OnGetAsync()
        {
            LodgerRole = await _context.LodgerRole.ToListAsync();
        }
    }
}
