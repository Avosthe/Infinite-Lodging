using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SSDAssignment40.Pages
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string userAlertMessage { get; set; }
        public void OnGet()
        {

        }
    }
}
