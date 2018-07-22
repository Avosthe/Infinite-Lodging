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

    public class DetailsModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public DetailsModel(SSDAssignment40.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public LodgerRole LodgerRole { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LodgerRole = await _context.LodgerRole.FirstOrDefaultAsync(m => m.Id == id);

            if (LodgerRole == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
