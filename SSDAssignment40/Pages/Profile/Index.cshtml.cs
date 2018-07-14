using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages
{

    [Authorize]
    public class ProfileModel : PageModel
    {
        [FromRoute]
        public string Username { get; set; }
        [BindProperty]
        public string UserID { get; set; }

        public string ProfilePicture { get; set; }
        public UserManager<Lodger> _userManager { get; set; }

        public Lodger LodgerUser { get; set; }

        public bool isValidProfile { get; set; }

        public bool isEditing { get; set; }

        public ProfileModel(UserManager<Lodger> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            Lodger l = await _userManager.FindByNameAsync(Username);
            if (l is Lodger)
            {
                isValidProfile = true;
                LodgerUser = l;
            }
            return Page();
        }
    }
}