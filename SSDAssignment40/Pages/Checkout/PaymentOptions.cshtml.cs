using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SSDAssignment40.Pages;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages.Checkout
{
    public class PaymentOptionsModel : PageModel
    {

        public void OnGet()
        {

        }
        public IList<CreditCard> CreditCard { get; set; }

    }
}
