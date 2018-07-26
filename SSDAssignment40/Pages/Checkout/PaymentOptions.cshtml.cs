using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SSDAssignment40.Pages;
using SSDAssignment40.Data;
using Microsoft.EntityFrameworkCore;

namespace SSDAssignment40.Pages.Checkout
{
    public class PaymentOptionsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PaymentOptionsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<CreditCard> CreditCard { get; set; }

        public async Task OnGetAsync()
        {
            CreditCard = await _context.CreditCard.ToListAsync();
        }
    }
}
