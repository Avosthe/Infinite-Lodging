using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public class UserRating
    {
        public string UserRatingId { get; set; }
        public virtual Lodger Rated { get; set; }
        public virtual Lodger Rater { get; set; }

    }
}
