using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages.Support
{
    public class CreateModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public CreateModel(SSDAssignment40.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CustomerSupport CustomerSupport { get; set; }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CustomerSupport.Add(CustomerSupport);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}