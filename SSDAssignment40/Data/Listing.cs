using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public class Listing
    {
        public string ListingId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Desc { get; set; }
        public string Location { get; set; }
        public string CoverPic { get; set; }
        public virtual Lodger Lodger { get; set; }
    }
}
