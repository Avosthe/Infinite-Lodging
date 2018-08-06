using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public class UserRevert
    {
        public string UserRevertId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string AlternateEmail { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Occupation { get; set; }
        public string Address { get; set; }
        public string GovernmentID { get; set; }
        public string Status { get; set; }
        public string Biography { get; set; }
        public string Hobbies { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string is3AuthEnabled { get; set; }
        public virtual AuditRecord AuditRecord { get; set; }

    }
}
