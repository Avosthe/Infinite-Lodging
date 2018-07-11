using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public class Review
    {
        public int ReviewID { get; set; }
        public DateTime DateTime { get; set; }
        public int Rating { get; set; }
        public string ReviewDesc { get; set; }
        public int ListingID { get; set; }
        public int LodgerID { get; set; }

        public Listing Listing { get; set; }
        public Lodger Lodger { get; set; }
    }
}
