using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SSDAssignment40.Data;

namespace SSDAssignment40
{
    public class EditReviewModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public EditReviewModel(SSDAssignment40.Data.ApplicationDbContext context, UserManager<Lodger> user)
        {
            _context = context;
            userManager = user;
        }

        [BindProperty]
        public Review Review { get; set; }

        public Listing Listing { get; set; }

        public UserManager<Lodger> userManager { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, string listingid)
        {
            if (id == null)
            {
                return NotFound();
            }

            Review = await _context.Review.FirstOrDefaultAsync(m => m.ReviewId == id);

            Listing = await _context.Listing.FirstOrDefaultAsync(m => m.ListingId == listingid);

            if (Review == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string listingid)
        {
            EntityEntry entry = _context.Entry(Review);

            Listing = await _context.Listing.FirstOrDefaultAsync(m => m.ListingId == listingid);

            Review.Listing = Listing;
            Review.Lodger = await userManager.GetUserAsync(User);
            Review.DateTime = DateTime.Now;

            var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                // Create an auditrecord object
                var auditrecord = new AuditRecord();
                auditrecord.AuditActionType = "Review " + Review.ReviewId + " was edited";
                auditrecord.DateTimeStamp = DateTime.Now;
                // Get current logged-in user
                auditrecord.PerformedBy = await userManager.GetUserAsync(User);
                auditrecord.IPAddress = auditrecord.PerformedBy.IPAddress;
                _context.AuditRecords.Add(auditrecord);
                await _context.SaveChangesAsync();

                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(Review.ReviewId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Redirect("./ListingDetails?id=" + Listing.ListingId);
        }

        private bool ReviewExists(string id)
        {
            return _context.Review.Any(e => e.ReviewId == id);
        }

        public async Task<IActionResult> OnPostDeleteReviewAsync(string reviewid, string listingid)
        {
            Listing = await _context.Listing.FirstOrDefaultAsync(m => m.ListingId == listingid);

            if (reviewid == null)
            {
                return NotFound();
            }

            Review = await _context.Review.FindAsync(reviewid);

            if (Review != null)
            {
                _context.ChangeTracker.DetectChanges();

                _context.Review.Remove(Review);
                await _context.SaveChangesAsync();

                // Create an auditrecord object
                var auditrecord = new AuditRecord();
                auditrecord.AuditActionType = "Review " + Review.ReviewId + " was deleted";
                auditrecord.DateTimeStamp = DateTime.Now;
                // Get current logged-in user
                auditrecord.PerformedBy = await userManager.GetUserAsync(User);
                auditrecord.IPAddress = auditrecord.PerformedBy.IPAddress;
                _context.AuditRecords.Add(auditrecord);
                await _context.SaveChangesAsync();
            }

            return Redirect("./ListingDetails?id=" + Listing.ListingId);
        }

    }
}
