﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SSDAssignment40.Data;

namespace SSDAssignment40.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<Lodger> _userManager;

        public ConfirmEmailModel(UserManager<Lodger> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code, string newEmail = null)
        {
            IdentityResult result = null;
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }
            if (newEmail == null)
            {
                result = await _userManager.ConfirmEmailAsync(user, code);
            }
            else
            {
                result = await _userManager.ChangeEmailAsync(user, newEmail, code);
            }

            if (result.Succeeded)
            {
                Lodger User = await _userManager.FindByIdAsync(userId);
                string Email = await _userManager.GetEmailAsync(User);
                List<Lodger> Lodgers = _userManager.Users.Where(l => l.Email == Email).ToList<Lodger>();
                    foreach(Lodger l in Lodgers)
                    {
                        if (!(l.EmailConfirmed))
                        {
                            await _userManager.DeleteAsync(l);
                        }
                    }
            }
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");
            }

            return Page();
        }
    }
}
