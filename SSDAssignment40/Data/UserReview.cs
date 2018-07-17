using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public class UserReview
    {
        public string UserReviewId { get; set; }
        public string ReviewContent { get; set; }
        public virtual Lodger ReviewFor { get; set; }
        public virtual Lodger ReviewBy { get; set; }
        public DateTime ReviewTimeStamp { get; set; }
    }
}
