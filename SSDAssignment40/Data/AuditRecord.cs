using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace SSDAssignment40.Data
{
    public class AuditRecord
    {
        public string AuditRecordId { get; set; }

        [Display(Name = "Audit Action")]
        public string AuditActionType { get; set; }

        [Display(Name = "Performed By")]
        public virtual Lodger PerformedBy { get; set; }

        [Display(Name = "Date/Time Stamp")]
        [DataType(DataType.DateTime)]
        public DateTime DateTimeStamp { get; set; }
        [Display(Name = "IP Address")]
        public string IPAddress { get; set; }
    }
}
