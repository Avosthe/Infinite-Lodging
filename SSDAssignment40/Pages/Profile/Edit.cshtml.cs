using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        public EditModel(UserManager<Lodger> userManager)
        {
            _userManager = userManager;
        }
        public void OnGet()
        {

        }
    }
}