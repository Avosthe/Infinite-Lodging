using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VirusTotalNET;
using VirusTotalNET.Results;

namespace SSDAssignment40.Pages
{
    public class EditListingModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public EditListingModel(SSDAssignment40.Data.ApplicationDbContext context, IHostingEnvironment environment, UserManager<Lodger> user, IVirusScanner virusScanner)
        {
            _context = context;
            _environment = environment;
            userManager = user;
            _virusScanner = virusScanner;
        }

        [BindProperty]
        public Listing Listing { get; set; }

        [BindProperty]
        public IFormFile Upload { get; set; }

        public Lodger Lodger { get; set; }

        private IHostingEnvironment _environment;

        public bool changePic = false;

        public string hex { get; set; }

        List<string> allowedFileTypes = new List<string>() { "FFD8", "8950" };

        public UserManager<Lodger> userManager { get; set; }

        public IVirusScanner _virusScanner { get; set; }

        private async Task<VirusReport> ScanForVirus(IFormFile hostile)
        {
            using (var ms = new MemoryStream())
            {
                hostile.CopyTo(ms);
                var HostileFileBytes = ms.ToArray();
                return await _virusScanner.ScanForVirus(HostileFileBytes);
            }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Lodger = await userManager.GetUserAsync(User);

            if (Lodger == null)
            {
                return RedirectToPage("/Error/NiceTry");
            }

            if (id == null)
            {
                return NotFound();
            }

            Listing = await _context.Listing.FirstOrDefaultAsync(m => m.ListingId == id);

            if (Lodger.Id != Listing.Lodger.Id)
            {
                return RedirectToPage("./Error/NiceTry");
            }

            if (Listing == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (Lodger.Id == Listing.Lodger.Id)
            {
                var existingPic = (from l in _context.Listing where l.ListingId == id select l.CoverPic).ToList();

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                if (Upload != null)
                {
                    VirusReport vr = await ScanForVirus(Upload);
                    if (vr.Positives > 0)
                    {
                        ModelState.AddModelError("CoverPicFailedVirusCheck", "Cover Picture failed virus scan!");
                        ModelState.AddModelError("CoverPicReportLink", vr.ReportLink);
                        return Page();
                    }

                    changePic = true;

                    string fullPath = "./wwwroot/ListingCover/" + existingPic[0];
                    fullPath = Path.GetFullPath(fullPath);

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
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


                            Listing.CoverPic = filename;
                        }
                    }
                }

                _context.Attach(Listing).State = EntityState.Modified;

                if (!changePic)
                {
                    Listing.CoverPic = existingPic[0];
                    _context.Listing.Update(Listing);
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListingExists(Listing.ListingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToPage("./Listings");
            }
            else
            {
                return RedirectToPage("./Error/NiceTry");
            }
        }

        private bool ListingExists(string id)
        {
            return _context.Listing.Any(e => e.ListingId == id);
        }

        public async Task<IActionResult> OnPostDeleteListingAsync(string id)
        {
            var existingPic = (from l in _context.Listing where l.ListingId == id select l.CoverPic).ToList();
            Listing.CoverPic = existingPic[0];
            try
            {
                if (Listing != null)
                {
                    string fullPath = "./wwwroot/ListingCover/" + Listing.CoverPic;
                    fullPath = Path.GetFullPath(fullPath);

                    _context.Listing.Remove(Listing);
                    await _context.SaveChangesAsync();

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex is SqlException || ex is DbUpdateException)
                    return RedirectToPage("./Error/deletelistingerror");
            }
            return RedirectToPage("./Listings");
        }
    }
}
