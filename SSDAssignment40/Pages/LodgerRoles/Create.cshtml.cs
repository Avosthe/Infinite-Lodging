using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages.LodgerRoles
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly RoleManager<LodgerRole> _roleManager;
        public UserManager<Lodger> _userManager { get; set; }
        public CreateModel(RoleManager<LodgerRole> roleManager, UserManager<Lodger> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        [BindProperty]
        public LodgerRole LodgerRole { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            LodgerRole.DateCreated = DateTime.Now;
            LodgerRole.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            LodgerRole.CreatedBy = await _userManager.GetUserAsync(User);
            IdentityResult roleResult = await _roleManager.CreateAsync(LodgerRole);
            return RedirectToPage("Index");
        }
    }
}