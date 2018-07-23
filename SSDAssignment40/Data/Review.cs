using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public class Review
    {
        public string ReviewId { get; set; }

        public DateTime DateTime { get; set; }

        [Display(Name = "Review Title")]
        [Required]
        [MaxLength(100)]
        public string ReviewTitle { get; set; }

        [Required]
        [Range(1,5)]
        public int Rating { get; set; }

        [Display(Name = "Review Description")]
        [Required]
        [MaxLength(500)]
        public string ReviewDesc { get; set; }

        public virtual Listing Listing { get; set; }
        public virtual Lodger Lodger { get; set; }
    }
}
