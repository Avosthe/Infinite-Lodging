using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public class Listing
    {
        public string ListingId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Price { get; set; }

        [Display(Name = "Description")]
        [Required]
        public string Desc { get; set; }

        [Required]
        public string Location { get; set; }

        [Display(Name ="Cover Picture")]
        public string CoverPic { get; set; }

        public virtual Lodger Lodger { get; set; }
    }
}
