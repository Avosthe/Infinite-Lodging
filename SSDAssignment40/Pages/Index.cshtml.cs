using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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
