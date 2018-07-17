using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public Lodger Rater { get; set; }

        public bool isValidProfile { get; set; }

        public bool isEditing { get; set; }

        [Required]
        [BindProperty]
        public string ReviewInput { get; set; }

        public IList<Listing> Listing { get; set; }

        public IList<Review> ListingReview { get; set; }

        public List<UserReview> ThisUsersReviews { get; set; }
        private readonly ApplicationDbContext _context;

        public ProfileModel(UserManager<Lodger> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            LodgerUser = await _userManager.FindByNameAsync(Username);
            await ValidateAndInitializeAsync(LodgerUser);
            return Page();
        }

        private async Task<bool> ValidateAndInitializeAsync(Lodger l)
        {
            if (l is Lodger)
            {
                isValidProfile = true;
                var listings = from list in _context.Listing select list;
                Listing = await listings.ToListAsync();
                var reviews = from r in _context.Review select r;
                ListingReview = await reviews.ToListAsync();
                ThisUsersReviews = await _context.UserReview.Where(ur => ur.ReviewFor.Id == LodgerUser.Id).ToListAsync();
                return true;
            }
            else isValidProfile = false;
            return false;
        }

        //public async Task<IActionResult> OnPostAddRatingAsync() // add like to profile
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    var rated = await _userManager.FindByNameAsync(Username);
        //    if (rated.UserName == user.UserName)
        //    {
        //        return Page();
        //    }

        //    if (_context.UserRating.Where(r => ((r.Rater.Id == user.Id) && (r.Rated.UserName == Username))).ToList().Count > 0)
        //    {
        //        _context.Remove(_context.UserRating.Where(r => ((r.Rater.Id == user.Id) && (r.Rated.UserName == Username))).ToList()[0]);
        //        rated.Rating -= 1;
        //    }
        //    else
        //    {
        //        UserRating ur = new UserRating() { UserRatingId = Guid.NewGuid().ToString(), Rater = user, Rated = await _userManager.FindByNameAsync(Username) };
        //        rated.Rating += 1;
        //    }
        //    await _context.SaveChangesAsync();
        //    return Page();
        //}

        public async Task<IActionResult> OnPostAddRatingAsync()
        {
            ModelState.Remove("Review");
            LodgerUser = await _userManager.FindByNameAsync(Username);
            await ValidateAndInitializeAsync(LodgerUser);
            Rater = await _userManager.GetUserAsync(User);
            if (LodgerUser.Id == Rater.Id)
            {
                return Page();
            }
            if (_context.UserRating.Where(r => ((r.Rater.Id == Rater.Id) && (r.Rated.UserName == Username))).ToList().Count > 0)
            {
                _context.UserRating.Remove(_context.UserRating.Where(r => ((r.Rater.Id == Rater.Id) && (r.Rated.UserName == Username))).ToList()[0]);
                LodgerUser.Rating -= 1;
            }
            else
            {
                UserRating ur = new UserRating() { UserRatingId = Guid.NewGuid().ToString(), Rater = Rater, Rated = LodgerUser };
                LodgerUser.Rating += 1;
                _context.UserRating.Add(ur);
            }
            await _context.SaveChangesAsync();
            return RedirectToPage("/Profile/Index", new { Username = Username });
        }


        public async Task<IActionResult> OnPostAddReviewAsync()
        {
            LodgerUser = await _userManager.FindByNameAsync(Username);
            Lodger LoggedInUser = await _userManager.GetUserAsync(User);
            if (Username == LoggedInUser.UserName)
            {
                return StatusCode(403);
            }
            await ValidateAndInitializeAsync(LodgerUser);
            if (!(ModelState.IsValid))
            {
                return Page();
            }
            UserReview newReview = new UserReview()
            {
                UserReviewId = Guid.NewGuid().ToString(),
                ReviewContent = ReviewInput,
                ReviewFor = LodgerUser,
                ReviewBy = LoggedInUser,
                ReviewTimeStamp = DateTime.Now
        };
            _context.UserReview.Add(newReview);
            await _context.SaveChangesAsync();
            return RedirectToPage("/Profile/Index", new { Username = Username
    });
        }
    }
}