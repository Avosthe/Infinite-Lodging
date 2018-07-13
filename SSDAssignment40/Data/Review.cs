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
        public int Rating { get; set; }
        public string ReviewDesc { get; set; }
        public Listing Listing { get; set; }
        public Lodger Lodger { get; set; }
    }
}
