using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public class UserReport
    {
        public string UserReportId { get; set; }
        public virtual Lodger ReportedUser { get; set; }
        public virtual Lodger ReportingUser { get; set; }
        public string ReportedContent { get; set; }
        public string ReportFileEvidence { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool isReviewd { get; set; }
    }
}
