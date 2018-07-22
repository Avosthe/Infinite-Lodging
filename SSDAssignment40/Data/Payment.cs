using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public class Payment
    {
        public string PaymentId { get; set; }
        public int Amount { get; set; }

        public virtual Lodger Lodger { get; set; }


    }
}
