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
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public int Price { get; set; }

        [Display(Name = "Description")]
        [Required]
        [MaxLength(1000)]
        public string Desc { get; set; }

        [Required]
        [MaxLength(100)]
        public string Location { get; set; }

        [Display(Name ="Cover Picture")]
        public string CoverPic { get; set; }

        public virtual Lodger Lodger { get; set; }
    }
}
