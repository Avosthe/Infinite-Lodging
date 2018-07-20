using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public class Review
    {
        public string ReviewId { get; set; }
        public DateTime DateTime { get; set; }
        public string ReviewTitle { get; set; }
        public int Rating { get; set; }
        public string ReviewDesc { get; set; }
        public virtual Listing Listing { get; set; }
        public virtual Lodger Lodger { get; set; }
    }
}
