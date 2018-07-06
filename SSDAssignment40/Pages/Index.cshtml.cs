using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages
{
    public class IndexModel : PageModel
    {

        //public IndexModel(ApplicationDbContext DBLodgers)
        //{
        //    _DBLodgers = DBLodgers;
        //}
        [TempData]
        public string userAlertMessage { get; set; }

        //public ApplicationDbContext _DBLodgers;

        public List<Lodger> Lodgers { get; set; }
        public async Task OnGetAsync()
        {
            //Lodgers = await _DBLodgers.Lodger.ToListAsync();
        }
    }
}
