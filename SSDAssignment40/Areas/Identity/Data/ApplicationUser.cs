using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SSDAssignment40.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; }
        [PersonalData]
        public DateTime DOB { get; set; }
        [PersonalData]
        public DateTime DateJoined { get; set; }
    }
}
