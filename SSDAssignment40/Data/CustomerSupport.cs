using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public class CustomerSupport
    {
        [Key]
        public int CustomerSupport_ID { get; set; }
        
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Date/Time Stamp")]
        [DataType(DataType.DateTime)]
        public DateTime DateTimeStamp { get; set; }

        [Display(Name = "Request")]
        public string Request { get; set; }

        [Display(Name = "No of Replies")]
        public int NoReplies { get; set; }

        public virtual Lodger Lodger { get; set; }

    }
}
