using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages.Profile
{
    [Authorize]

    public class Input
    {
        public IFormFile ProfilePicture { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required]
        public string Gender { get; set; }
        public string Biography { get; set; }
        [Required]
        [EmailAddress]
        public string AlternateEmail { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [Display(Name = "Government Identification")]
        public IFormFile GovernmentID { get; set; }
    }

    public class EditModel : PageModel
    {

        public UserManager<Lodger> _userManager { get; set; }

        [BindProperty]
        public Input UserInput { get; set; }

        public IHostingEnvironment _environment { get; set; }

        public string alertMessage { get; set; }

        public ApplicationDbContext _context { get; set; }

        public Lodger LodgerUser { get; set; }
        List<byte[]> allowedHeaders = new List<byte[]>() { new byte[] { 0xFF, 0xD8, 0xFF }, new byte[] { 0x89, 0x50, 0x4E } };
        public IDataProtector _protector { get; set; }
        public IVirusScanner _virusScanner { get; set; }

        public EditModel(UserManager<Lodger> userManager, IHostingEnvironment environment, ApplicationDbContext context, IDataProtectionProvider provider, IVirusScanner virusScanner)
        {
            _userManager = userManager;
            _environment = environment;
            _context = context;
            _protector = provider.CreateProtector("SSDAssignment40.Pages.Profile.Edit");
            _virusScanner = virusScanner;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            LodgerUser = await _userManager.GetUserAsync(User);

            return Page();
        }

        private bool checkPictureHeader(IFormFile File)
        {
            using (var ms = new MemoryStream())
            {
                File.CopyTo(ms);
                var ProfilePicBytes = ms.ToArray();
                for (int i = 0; i < 3; i++)
                {
                    if (ProfilePicBytes[i] != allowedHeaders[0][i] && ProfilePicBytes[i] != allowedHeaders[1][i]) return false;
                }
            }
            return true;
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
        public async Task<IActionResult> OnPostAsync()
        {
            LodgerUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid) return Page();
            var user = await _userManager.GetUserAsync(User);
            _context.Update(user);
            if (UserInput.ProfilePicture != null)
            {
                VirusReport vr = await ScanForVirus(UserInput.ProfilePicture);
                if (vr.Positives > 0)
                {
                    ModelState.AddModelError("ProfilePictureFailedVirusCheck", "ProfilePicture failed virus scan!");
                    ModelState.AddModelError("ProfilePictureReportLink", vr.ReportLink);
                    return Page();
                }
                if (!(checkPictureHeader(UserInput.ProfilePicture)))
                {
                    ModelState.AddModelError("ProfilePicInvalid", "Invalid file format for Profile Picture (Only .jpg/.jpeg/.png are accepted!)");
                    return Page();
                }
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(UserInput.ProfilePicture.FileName);
                user.ProfilePic = filename;
                var file = Path.Combine(_environment.ContentRootPath, "wwwroot", "profile-images", filename);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await UserInput.ProfilePicture.CopyToAsync(fileStream);
                }
            }
            user.FullName = (UserInput.FullName == user.FullName) ? user.FullName : UserInput.FullName;
            List<string> toCheck = new List<string>() { "Male", "Female", "Other" };
            if (toCheck.Contains(UserInput.Gender))
            {
                user.Gender = (user.Gender == UserInput.Gender) ? user.Gender : UserInput.Gender;
            }
            //else return Page();
            user.Biography = (user.Biography == UserInput.Biography) ? user.Biography : UserInput.Biography;
            user.AlternateEmail = (user.AlternateEmail == UserInput.AlternateEmail) ? user.AlternateEmail : UserInput.AlternateEmail;
            user.Country = (user.Country == UserInput.Country) ? user.Country : UserInput.Country;
            user.City = (user.City == UserInput.City) ? user.City : UserInput.City;
            VirusReport vr2 = await ScanForVirus(UserInput.GovernmentID);
            if (vr2.Positives > 0)
            {
                ModelState.AddModelError("GovernmentIDFailedVirusCheck", "GovernmentID failed virus scan!");
                ModelState.AddModelError("GovernmentIDReportLink", vr2.ReportLink);
                return Page();
            }
            if (!(checkPictureHeader(UserInput.GovernmentID)))
            {
                ModelState.AddModelError("GovernmentIDPhoto", "Invalid file format for GovernmentID (Only .jpg/.jpeg/.png are accepted!)");
                return Page();
            }
            var gFileName = Guid.NewGuid().ToString() + Path.GetExtension(UserInput.GovernmentID.FileName);
            user.GovernmentID = gFileName;
            var gFile = Path.Combine(_environment.ContentRootPath, "wwwroot", "profile-images", gFileName);
            using (var fileStream = new FileStream(gFile, FileMode.Create))
            {
                await UserInput.GovernmentID.CopyToAsync(fileStream);
            }
            await _context.SaveChangesAsync();
            alertMessage = "User Profile Updated Successfully";
            return Page();
        }
    }
}