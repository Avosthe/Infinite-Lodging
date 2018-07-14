using System;

namespace SSDAssignment40.Data
{
    public class Booking
    {
        public string BookingId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public virtual Lodger Lodger { get; set; }
        public virtual Listing Listing { get; set; }
    }
}
