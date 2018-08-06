using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SSDAssignment40.Data;

namespace SSDAssignment40.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<Lodger> _userManager;
        private readonly SignInManager<Lodger> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(
            UserManager<Lodger> userManager,
            SignInManager<Lodger> signInManager,
            ILogger<ChangePasswordModel> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }
        public ApplicationDbContext _context { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var LodgerUser = await _userManager.GetUserAsync(User);
            if (LodgerUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
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
            var changePasswordResult = await _userManager.ChangePasswordAsync(LodgerUser, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            AuditRecord auditRecord = new AuditRecord();
            auditRecord.AuditActionType = "Changed Password";
            auditRecord.AuditRecordId = Guid.NewGuid().ToString();
            auditRecord.DateTimeStamp = DateTime.Now;
            auditRecord.PerformedBy = LodgerUser;
            auditRecord.IPAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            ur.AuditRecord = auditRecord;
            _context.UserReverts.Add(ur);
            await _context.SaveChangesAsync();
            await _signInManager.RefreshSignInAsync(LodgerUser);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Your password has been changed.";

            return RedirectToPage();
        }
    }
}
