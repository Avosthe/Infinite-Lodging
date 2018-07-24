using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using SSDAssignment40.Data;
using System;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SSDAssignment40.Pages
{
    [Authorize]
    public class CreateListingModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public CreateListingModel(SSDAssignment40.Data.ApplicationDbContext context, IHostingEnvironment environment, UserManager<Lodger> user)
        {
            _context = context;
            _environment = environment;
            userManager = user;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        private IHostingEnvironment _environment;

        [BindProperty]
        public IFormFile Upload { get; set; }

        [BindProperty]
        public Listing Listing { get; set; }

        public string hex { get; set; }

        List<string> allowedFileTypes = new List<string>() { "FFD8", "8950" };

        public UserManager<Lodger> userManager { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Listing.Lodger = await userManager.GetUserAsync(User);

                if (!ModelState.IsValid)
                {
                    return Page();
                }   

                if (Listing.Price < 1)
                {
                    return RedirectToPage("./Error/MinPrice");
                }

                var filename = Guid.NewGuid().ToString() + Path.GetExtension(Upload.FileName);
                var file = Path.Combine(_environment.ContentRootPath, "wwwroot", "ListingCover", filename);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                    using (var ms = new MemoryStream())
                    {
                        Upload.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        hex = BitConverter.ToString(fileBytes).Replace("-", "").Substring(0, 4);

                        if (!allowedFileTypes.Contains(hex))
                        {
                            return RedirectToPage("/Error/wrongFileType");
                        }
                    }
                }

                Listing.CoverPic = filename;
                _context.Listing.Add(Listing);
                await _context.SaveChangesAsync();

                // Once a record is added, create an audit record
                if (await _context.SaveChangesAsync() > 0)
                {
                    // Create an auditrecord object
                    var auditrecord = new AuditRecord();
                    auditrecord.AuditActionType = "New Listing created with id " + Listing.ListingId;
                    auditrecord.DateTimeStamp = DateTime.Now;
                    // Get current logged-in user
                    auditrecord.PerformedBy = await userManager.GetUserAsync(User);
                    _context.AuditRecords.Add(auditrecord);
                    await _context.SaveChangesAsync();
                }
                return RedirectToPage("./Listings");
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException)
                    return RedirectToPage("./Error/NiceTry");
                return Page();
            }
        }

        public class MinValueAttribute : ValidationAttribute
        {
            private readonly int _minValue;

            public MinValueAttribute(int minValue)
            {
                _minValue = minValue;
            }

            public override bool IsValid(object value)
            {
                return (int)value <= _minValue;
            }
        }

    }
}