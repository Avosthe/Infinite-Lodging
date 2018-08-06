using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace SSDAssignment40.Data
{
    public class Lodger : IdentityUser, ICloneable
    {
        public string FullName { get; set; }
        public bool isVerified { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
        public string AlternateEmail { get; set; }
        public string Gender { get; set; }
        public string ProfilePic { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Hobbies { get; set; }
        public string GovernmentID { get; set; }
        public string Status { get; set; }
        public string is3AuthEnabled { get; set; }
        public string is3AuthPattern {get; set;}
        public string Nric { get; set; }
        public string SiteLabel { get; set; }
        public string Address { get; set; }
        public string Occupation { get; set; }
        public int Rating { get; set; }
        public string IPAddress { get; set; }
        public bool RequireAdditionalVerification { get; set; }
        public string AdditionalVerificationSecret { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
