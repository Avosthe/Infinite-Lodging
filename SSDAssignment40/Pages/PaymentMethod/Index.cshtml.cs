﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;


namespace SSDAssignment40.Pages.PaymentMethod
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public IndexModel(SSDAssignment40.Data.ApplicationDbContext context)
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
