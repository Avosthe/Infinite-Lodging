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
        [StringLength(50, ErrorMessage = "Full Name too long!")]
        [RegularExpression("^[a-zA-Z][a-zA-Z\\s]*$", ErrorMessage = "Please enter a valid Full Name!")]
        public string FullName { get; set; }
        [Required]
        public string Gender { get; set; }
        [StringLength(500, ErrorMessage = "Sorry, your Biography is too long!")]
        public string Biography { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "Sorry, your Alternate Email Address is too long!")]
        public string AlternateEmail { get; set; }
        [Required]
        [StringLength(70, ErrorMessage = "Sorry, your Country is too long!")]
        [RegularExpression("^[a-zA-Z][a-zA-Z\\s]*$", ErrorMessage = "Please enter a valid Country!")]
        public string Country { get; set; }
        [Required]
        [StringLength(60, ErrorMessage = "Sorry, your City is too long!")]
        [RegularExpression("^[a-zA-Z][a-zA-Z\\s]*$", ErrorMessage = "Please enter a valid City!")]
        public string City { get; set; }
        //[Required]
        [Display(Name = "Government Identification")]
        public IFormFile GovernmentID { get; set; }
        [StringLength(50, ErrorMessage = "Sorry your hobbies are too long!")]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9.,/\\s]*$", ErrorMessage = "Please enter valid characters only!")]
        public string Hobbies { get; set; }
        [StringLength(50, ErrorMessage = "Sorry, you status is too long!")]
        [RegularExpression("^[a-zA-Z][a-zA-Z\\s]*$", ErrorMessage = "Please enter valid characters only!")]
        public string Status { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Sorry, your address is too long!")]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9_#-@\\s]*$", ErrorMessage = "Please enter valid characters only!")]
        public string Address { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Sorry, your occupation is too long!")]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9_/,.\\s]*$", ErrorMessage = "Please enter valid characters only!")]
        public string Occupation { get; set; }
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
        public IDataProtectionProvider _provider { get; set; }
        public string Occupation { get; set; }
        public string Address { get; set; }

        public EditModel(UserManager<Lodger> userManager, IHostingEnvironment environment, ApplicationDbContext context, IDataProtectionProvider provider, IVirusScanner virusScanner)
        {
            _userManager = userManager;
            _environment = environment;
            _context = context;
            _provider = provider;
            _protector = provider.CreateProtector("SSDAssignment40.Pages.Profile.Edit");
            _virusScanner = virusScanner;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            LodgerUser = await _userManager.GetUserAsync(User);
            Occupation = "";
            Address = "";
            if(LodgerUser.Occupation != null)
            {
                Occupation = _protector.Unprotect(LodgerUser.Occupation);
            }
            if (LodgerUser.Address != null)
            {
                Address = _protector.Unprotect(LodgerUser.Address);
            }
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
            if (!(string.IsNullOrEmpty(LodgerUser.GovernmentID)))
            {
                ModelState.Remove("GovernmentID"); // remove governmentId req if already have
            }
            UserRevert ur = new UserRevert()
            {
                UserRevertId = Guid.NewGuid().ToString(),
                FullName = LodgerUser.FullName,
                Gender = LodgerUser.Gender,
                AlternateEmail = LodgerUser.AlternateEmail,
                Country = LodgerUser.Country,
                City = LodgerUser.City,
                Occupation = LodgerUser.Occupation,
                Address = LodgerUser.Address,
                GovernmentID = LodgerUser.GovernmentID,
                Status = LodgerUser.Status,
                Biography = LodgerUser.Biography,
                Hobbies = LodgerUser.Hobbies,
                Email = LodgerUser.Email,
                PasswordHash = LodgerUser.PasswordHash,
                PhoneNumber = LodgerUser.PhoneNumber,
                PhoneNumberConfirmed = LodgerUser.PhoneNumberConfirmed,
                is3AuthEnabled = LodgerUser.is3AuthEnabled
            };
            if (!ModelState.IsValid) return Page();
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
                if (!(string.IsNullOrEmpty(LodgerUser.ProfilePic)))
                {
                    var CurrentProfilePicture = LodgerUser.ProfilePic;
                    System.IO.File.Delete(Path.Combine(_environment.ContentRootPath, "wwwroot", "profile-images", CurrentProfilePicture));
                }
                LodgerUser.ProfilePic = filename;
                var file = Path.Combine(_environment.ContentRootPath, "wwwroot", "profile-images", filename);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await UserInput.ProfilePicture.CopyToAsync(fileStream);
                }
            }
            LodgerUser.FullName = (UserInput.FullName == LodgerUser.FullName) ? LodgerUser.FullName : UserInput.FullName;
            List<string> toCheck = new List<string>() { "Male", "Female", "Other" };
            if (toCheck.Contains(UserInput.Gender))
            {
                LodgerUser.Gender = (LodgerUser.Gender == UserInput.Gender) ? LodgerUser.Gender : UserInput.Gender;
            }
            else
            {
                ModelState.AddModelError("Invalid Gender", "Invalid Gender!");
                return Page();
            }
            LodgerUser.Biography = (LodgerUser.Biography == UserInput.Biography) ? LodgerUser.Biography : UserInput.Biography;
            LodgerUser.AlternateEmail = (LodgerUser.AlternateEmail == UserInput.AlternateEmail) ? LodgerUser.AlternateEmail : UserInput.AlternateEmail;
            LodgerUser.Country = (LodgerUser.Country == UserInput.Country) ? LodgerUser.Country : UserInput.Country;
            LodgerUser.City = (LodgerUser.City == UserInput.City) ? LodgerUser.City : UserInput.City;
            LodgerUser.Address = (LodgerUser.Address == _protector.Protect(UserInput.Address)) ? LodgerUser.Address : _protector.Protect(UserInput.Address);
            LodgerUser.Occupation = (LodgerUser.Occupation == _protector.Protect(UserInput.Occupation)) ? LodgerUser.Occupation : _protector.Protect(UserInput.Occupation);
            LodgerUser.Hobbies = (LodgerUser.Hobbies == UserInput.Hobbies) ? LodgerUser.Hobbies : UserInput.Hobbies;
            LodgerUser.Status = (LodgerUser.Status == UserInput.Status) ? LodgerUser.Status : UserInput.Status;
            if(UserInput.GovernmentID != null)
            {
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
                if (!(string.IsNullOrEmpty(LodgerUser.GovernmentID)))
                {
                    var CurrentGovernmentID = LodgerUser.GovernmentID;
                    System.IO.File.Delete(Path.Combine(_environment.ContentRootPath, "wwwroot", "government-ids", CurrentGovernmentID));
                }
                LodgerUser.GovernmentID = _protector.Protect(gFileName);
                var gFile = Path.Combine(_environment.ContentRootPath, "wwwroot", "government-ids", gFileName);
                using (var fileStream = new FileStream(gFile, FileMode.Create))
                {
                    await UserInput.GovernmentID.CopyToAsync(fileStream);
                }
            }
            if(await _context.SaveChangesAsync() > 0)
            {
                AuditRecord auditRecord = new AuditRecord();
                auditRecord.AuditRecordId = Guid.NewGuid().ToString();
                auditRecord.AuditActionType = "Edit Profile";
                auditRecord.PerformedBy = LodgerUser;
                auditRecord.DateTimeStamp = DateTime.Now;
                auditRecord.IPAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                _context.AuditRecords.Add(auditRecord);
                ur.AuditRecord = auditRecord;
                _context.UserReverts.Add(ur);
                await _context.SaveChangesAsync();
            }
            alertMessage = "User Profile Updated Successfully";
            return Page();
        }
    }
}