using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SSDAssignment40.Data;
using System.Security.Cryptography;
using System.Text;

namespace SSDAssignment40.Areas.Identity.Pages.Account.Manage
{
    public class PrivateKeyAuthenticationModel : PageModel
    {

        public Lodger LodgerUser { get; set; }
        public UserManager<Lodger> _userManager { get; set; }

        [BindProperty]
        [Required]
        public IFormFile PrivateKeyFile { get; set; }
        public IVirusScanner _virusScanner { get; set; }
        public string AlertMessage { get; set; }

        public PrivateKeyAuthenticationModel(UserManager<Lodger> userManager, IVirusScanner virusScanner)
        {
            _userManager = userManager;
            _virusScanner = virusScanner;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            LodgerUser = await _userManager.GetUserAsync(User);
            return Page();
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
            VirusReport vr = await ScanForVirus(PrivateKeyFile);
            if (vr.Positives > 0)
            {
                ModelState.AddModelError("PrivateKeyFileFailedVirusCheck", "ProfilePicture failed virus scan!");
                ModelState.AddModelError("PrivateKeyFileReportLink", vr.ReportLink);
                return Page();
            }
            using (var ms = new MemoryStream())
            {
                PrivateKeyFile.CopyTo(ms);
                byte[] PrivateKeyFileBytes = ms.ToArray();
                SHA512 sha512 = SHA512.Create();
                byte[] HashedPrivateKeyFileBytes = sha512.ComputeHash(PrivateKeyFileBytes);
                string HashedPrivateKeyFileString = Encoding.UTF8.GetString(HashedPrivateKeyFileBytes);
                LodgerUser.secretFileVerificationHash = HashedPrivateKeyFileString;
                AlertMessage = "Success! You can now login to your account by uploading this file at the login page!";
            }
            return Page();
        }
    }
}