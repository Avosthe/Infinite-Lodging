using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public class Listing
    {
        public string ListingID { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Desc { get; set; }
        public string Location { get; set; }
        public byte[] CoverPic { get; set; }
        public int LodgerID { get; set; }

        public Lodger Lodger { get; set; }
    }
}
