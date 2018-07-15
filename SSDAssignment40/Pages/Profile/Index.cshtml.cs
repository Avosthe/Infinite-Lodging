using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        public UserManager<Lodger> _userManager { get; set; }

        public Lodger LodgerUser { get; set; }

        public bool isValidProfile { get; set; }

        public bool isEditing { get; set; }


        public string Review { get; set; }

        public IList<Listing> Listing { get; set; }

        public IList<Review> ListingReview { get; set; }
        private readonly ApplicationDbContext _context;

        public ProfileModel(UserManager<Lodger> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            Lodger l = await _userManager.FindByNameAsync(Username);
            if (l is Lodger)
            {
                isValidProfile = true;
                LodgerUser = l;
                var listings = from list in _context.Listing select list;
                Listing = await listings.ToListAsync();
                var reviews = from r in _context.Review select r;
                ListingReview = await reviews.ToListAsync();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddReviewAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if(Username == user.UserName)
            {
                return StatusCode(403);
            }
            return Page();
        }
    }
}