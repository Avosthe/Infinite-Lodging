using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages.PaymentMethod
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
        public CreditCard CreditCard { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CreditCard.Add(CreditCard);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}