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
        public string Gender {get; set;}
        [Required]  
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

        public EditModel(UserManager<Lodger> userManager, IHostingEnvironment environment, ApplicationDbContext context)
        {
            _userManager = userManager;
            _environment = environment;
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            LodgerUser = await _userManager.GetUserAsync(User);

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            LodgerUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid) return Page();
            var user = await _userManager.GetUserAsync(User);
            _context.Update(user);
            if (UserInput.ProfilePicture != null)
            {
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
            if (toCheck.Contains(user.Gender))
            {
                user.Gender = (user.Gender == UserInput.Gender) ? user.Gender : UserInput.Gender;
            }
            //else return Page();
            user.Biography = (user.Biography == UserInput.Biography) ? user.Biography : UserInput.Biography;
            user.AlternateEmail = (user.AlternateEmail == UserInput.AlternateEmail) ? user.AlternateEmail : UserInput.AlternateEmail;
            user.Country = (user.Country == UserInput.Country) ? user.Country : UserInput.Country;
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