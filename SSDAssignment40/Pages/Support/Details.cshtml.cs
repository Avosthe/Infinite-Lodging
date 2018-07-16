using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SSDAssignment40.Pages.Support
{
    public class DetailsModel : PageModel
    {
        private readonly SSDAssignment40.Data.ApplicationDbContext _context;

        public DetailsModel(SSDAssignment40.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Reply> Replies { get; set; }

        public CustomerSupport CustomerSupport { get; set; }
        [BindProperty]
        [Required]
        public string StringContent { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            HttpContext.Session.SetInt32("currentId", Convert.ToInt32(id));
            CustomerSupport = await _context.CustomerSupport.FirstOrDefaultAsync(m => m.CustomerSupport_ID == HttpContext.Session.GetInt32("currentId"));
            Replies = await _context.Reply.Where(r => r.CustomerSupport_ID == CustomerSupport.CustomerSupport_ID).ToListAsync();
            if (CustomerSupport == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            CustomerSupport = await _context.CustomerSupport.FirstOrDefaultAsync(m => m.CustomerSupport_ID == HttpContext.Session.GetInt32("currentId"));
            Reply curReply = new Reply();
            curReply.Replies = StringContent;
            curReply.CustomerSupport_ID = CustomerSupport.CustomerSupport_ID;
            _context.Reply.Add(curReply);
            HttpContext.Session.Remove("currentId");
            CustomerSupport.NoReplies += 1;
            await _context.SaveChangesAsync();
            return RedirectToPage(new { id = CustomerSupport.CustomerSupport_ID});
        }
    }
}
