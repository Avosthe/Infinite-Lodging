using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public UserManager<Lodger> _userManager { get; set; }

        public Lodger LodgerUser { get; set; }

        public Lodger Rater { get; set; }

        public bool isValidProfile { get; set; }

        [Required]
        [BindProperty]
        [StringLength(500, ErrorMessage = "Sorry, your review is too long!")]
        public string ReviewInput { get; set; }
        [Required]
        [BindProperty]
        public string Reason { get; set; }
        [Required]
        [BindProperty]
        public IFormFile ReportEvidence { get; set; }

        public IList<Listing> Listing { get; set; }

        public IList<Review> ListingReview { get; set; }

        public List<UserReview> ThisUsersReviews { get; set; }
        private readonly ApplicationDbContext _context;

        public IVirusScanner _virusScanner { get; set; }
        public IHostingEnvironment _environment { get; set; }

        public bool hasRated { get; set; }

        public ProfileModel(UserManager<Lodger> userManager, ApplicationDbContext context, IVirusScanner virusScanner, IHostingEnvironment environment)
        {
            _userManager = userManager;
            _context = context;
            _virusScanner = virusScanner;
            _environment = environment;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            LodgerUser = await _userManager.FindByNameAsync(Username);
            if(!(LodgerUser is Lodger))
            {
                isValidProfile = false;
                return Page();
            }
            await ValidateAndInitializeAsync();
            Rater = await _userManager.GetUserAsync(User);
            if (_context.UserRating.Where(r => ((r.Rater.Id == Rater.Id) && (r.Rated.UserName == Username))).ToList().Count > 0)
            {
                hasRated = true;
            }
                return Page();
        }

        private async Task<bool> ValidateAndInitializeAsync()
        {
            isValidProfile = true;
            Listing = await _context.Listing.Where(l => (l.Lodger.Id == LodgerUser.Id)).ToListAsync();
            ListingReview = await _context.Review.Where(r => (r.Lodger.Id == LodgerUser.Id)).ToListAsync();
            ThisUsersReviews = await _context.UserReview.Where(ur => ur.ReviewFor.Id == LodgerUser.Id).ToListAsync();
            return true;
        }

        public async Task<IActionResult> OnPostAddRatingAsync()
        {
            ModelState.Remove("ReviewInput");
            ModelState.Remove("Reason");
            ModelState.Remove("ReportEvidence");
            LodgerUser = await _userManager.FindByNameAsync(Username);
            if (!(LodgerUser is Lodger))
            {
                isValidProfile = false;
                return Page();
            }
            await ValidateAndInitializeAsync();
            Rater = await _userManager.GetUserAsync(User);
            if (LodgerUser.Id == Rater.Id)
            {
                return RedirectToPage("/Profile/Index", new { Username = Username });
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
            if (!(LodgerUser is Lodger))
            {
                isValidProfile = false;
                return Page();
            }
            Lodger LoggedInUser = await _userManager.GetUserAsync(User);
            if (Username == LoggedInUser.UserName)
            {
                return StatusCode(403);
            }
            await ValidateAndInitializeAsync();
            ModelState.Remove("Reason");
            ModelState.Remove("ReportEvidence");
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
            return RedirectToPage("/Profile/Index", new
            {
                Username = Username
            });
        }
        private async Task<VirusReport> ScanForVirus(IFormFile hostile)
        {
            using (var ms = new MemoryStream())
            {
                hostile.CopyTo(ms);
                var HostileFileBytes = ms.ToArray();
                return await _virusScanner.ScanForVirus(HostileFileBytes);
            }
        }

        public async Task<IActionResult> OnPostSubmitReportAsync()
        {
            LodgerUser = await _userManager.FindByNameAsync(Username);
            if (!(LodgerUser is Lodger))
            {
                isValidProfile = false;
                return Page();
            }
            Lodger LoggedInUser = await _userManager.GetUserAsync(User);
            if (Username == LoggedInUser.UserName)
            {
                return StatusCode(403);
            }
            await ValidateAndInitializeAsync();
            ModelState.Remove("ReviewInput");
            if (!(ModelState.IsValid))
            {
                return Page();
            }
            VirusReport vr = await ScanForVirus(ReportEvidence);
            if (vr.Positives > 0)
            {
                ModelState.AddModelError("ReportEvidenceFileFailedVirusCheck", "ReportEvidence failed virus scan!");
                ModelState.AddModelError("ReportEvidenceFileReportLink", vr.ReportLink);
                return Page();
            }
            var filename = Guid.NewGuid().ToString() + Path.GetExtension(ReportEvidence.FileName);
            var file = Path.Combine(_environment.ContentRootPath, "wwwroot", "reports", filename);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await ReportEvidence.CopyToAsync(fileStream);
            }
            HttpContext.Session.SetString("ReportedUserId", LodgerUser.Id);
            HttpContext.Session.SetString("Reason", Reason);
            HttpContext.Session.SetString("EvidenceFileName", filename);
            return RedirectToPage("/Reports/SendReport");
        }
    }
}