using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SSDAssignment40.Data;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SSDAssignment40.Pages
{
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

        public UserManager<Lodger> userManager { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            Listing.Lodger = await userManager.GetUserAsync(User);

            var filename =  Guid.NewGuid().ToString() + Path.GetExtension(Upload.FileName);
            var file = Path.Combine(_environment.ContentRootPath, "wwwroot", "ListingCover", filename);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Listing.CoverPic = filename;
            _context.Listing.Add(Listing);
            await _context.SaveChangesAsync();

            return RedirectToPage("./ListingsCreated");
        }

        

    }
}