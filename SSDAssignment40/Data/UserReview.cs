using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public class UserReview
    {
        public string ReviewId { get; set; }
        public string ReviewFor { get; set; }
        public string ReviewBy { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
