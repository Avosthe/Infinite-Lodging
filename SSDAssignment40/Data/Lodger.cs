﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public string ProfilePic { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int ThumbsUp { get; set; }
        public int ThumbsDown { get; set; }

        public static implicit operator Lodger(List<Lodger> v)
        {
            throw new NotImplementedException();
        }
        //public string Hobbies { get; set; }
        //public string GovernmentID { get; set; }
    }
}
