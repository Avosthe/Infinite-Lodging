using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context, UserManager<Lodger> user)
        {
            _context = context;
            userManager = user;
        }

        [BindProperty]
        public IList<Review> ReviewList { get; set; }

        [Required]
        [BindProperty]
        public Review Review { get; set; }

        public DateTime dateStart { get; set; }

        public DateTime dateEnd { get; set; }

        public Lodger Lodger { get; set; }

        //[BindProperty]
        public Listing Listing { get; set; }

        public UserManager<Lodger> userManager { get; set; }


        public async Task<IActionResult> OnGetAsync(string id)
        {
            Lodger = await userManager.GetUserAsync(User);

            if (id == null)
            {
                return NotFound();
            }

            Listing = await _context.Listing.FirstOrDefaultAsync(m => m.ListingId == id);

            if (Listing == null)
            {
                return NotFound();
            }

            var reviews = from r in _context.Review where r.Listing.ListingId == id select r;
            ReviewList = await reviews.ToListAsync();

            dateStart = DateTime.Now.Date;

            dateEnd = DateTime.Now.Date.AddDays(1);

            return Page();
        }

        public async Task<IActionResult> OnPostSubmitReviewAsync(string id)
        {
            Review.Lodger = await userManager.GetUserAsync(User);
            Review.Listing = await _context.Listing.FirstOrDefaultAsync(m => m.ListingId == id);
            Review.DateTime = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Review.Lodger == null)
            {
                return Redirect("./Identity/Account/Login");
            }

            _context.Review.Add(Review);
            await _context.SaveChangesAsync();

            // Create an auditrecord object
            var auditrecord = new AuditRecord();
            auditrecord.AuditActionType = "Review was created with id " + Review.ReviewId;
            auditrecord.DateTimeStamp = DateTime.Now;
            // Get current logged-in user
            auditrecord.PerformedBy = await userManager.GetUserAsync(User);
            auditrecord.IPAddress = auditrecord.PerformedBy.IPAddress;
            _context.AuditRecords.Add(auditrecord);
            await _context.SaveChangesAsync();

            return Redirect("./ListingDetails?id=" + id);
        }
    }
}
