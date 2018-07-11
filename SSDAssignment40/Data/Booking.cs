using System;

namespace SSDAssignment40.Data
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int ListingID { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int LodgerID { get; set; }

        public Lodger Lodger { get; set; }
        public Listing Listing { get; set; }
    }
}
