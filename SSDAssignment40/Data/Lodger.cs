using Microsoft.AspNetCore.Identity;
using System;

namespace SSDAssignment40.Data
{
    public class Lodger : IdentityUser
    {
        public string FullName { get; set; }
        public bool isVerified { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
        public string AlternateEmail { get; set; }
        public string Gender { get; set; }
        public byte[] ProfilePic { get; set; }
    }
}
