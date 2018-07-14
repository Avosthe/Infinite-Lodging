using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SSDAssignment40.Data;

namespace SSDAssignment40.Pages.Support
{
    public class RepliesModel : PageModel
    {
            public IList<CustomerSupport> CustomerSupport { get; set; }
    }
}