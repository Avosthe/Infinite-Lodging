using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SSDAssignment40.Pages
{

    [Authorize]
    public class ProfileModel : PageModel
    {
        [FromRoute]
        public string Username { get; set; }
        [BindProperty]
        public string UserID { get; set; }
        public void OnGet()
        {

        }
        public void OnPostEdit()
        {
            return;
        }
    }
}