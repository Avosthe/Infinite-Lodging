using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages.PaymentMethod
{
    public class DetailsModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public DetailsModel(SSDAssignment40.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public CreditCard CreditCard { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CreditCard = await _context.CreditCard.FirstOrDefaultAsync(m => m.CreditCardId == id);

            if (CreditCard == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
