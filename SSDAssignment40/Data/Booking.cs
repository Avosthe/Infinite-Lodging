using System;

namespace SSDAssignment40.Data
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public Lodger Lodger { get; set; }
        public Listing Listing { get; set; }
    }
}
