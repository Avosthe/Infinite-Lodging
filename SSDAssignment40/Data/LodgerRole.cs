using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public class LodgerRole : IdentityRole
    {
        [Display(Name ="Role Description")]
        public string RoleDescription { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "IP Address")]
        public string IPAddress { get; set; }
        [Display(Name = "Created By")]
        public virtual Lodger CreatedBy { get; set; }
    }
}
