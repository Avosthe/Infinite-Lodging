using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages.LodgerRoles
{

    public class EditModel : PageModel
    {
        private readonly RoleManager<LodgerRole> _roleManager;
        public EditModel(RoleManager<LodgerRole> roleManager)
        {
            _roleManager = roleManager;
        }
        [BindProperty]
        public LodgerRole LodgerRole { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            LodgerRole = await _roleManager.FindByIdAsync(id);
            if (LodgerRole == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            LodgerRole appRole = await _roleManager.FindByIdAsync(LodgerRole.Id);
            appRole.Name = LodgerRole.Name;
            appRole.RoleDescription = LodgerRole.RoleDescription;
            IdentityResult roleRuslt = await _roleManager.UpdateAsync(appRole);
            if (roleRuslt.Succeeded)
            {
                return RedirectToPage("./Index");
            }
            return RedirectToPage("./Index");
        }
    }
}
