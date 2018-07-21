using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SSDAssignment40.Data;


namespace SSDAssignment40.Data
{
    public class ApplicationDbContext : IdentityDbContext<Lodger, LodgerRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Booking> Booking { get; set; }
        public DbSet<Listing> Listing { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<CustomerSupport> CustomerSupport { get; set; }
        public DbSet<Reply> Reply { get; set; }
        public DbSet<CreditCard> CreditCard { get; set; }
        public DbSet<UserRating> UserRating { get; set; }
        public DbSet<UserReview> UserReview { get; set; }
        public DbSet<UserReport> UserReport { get; set; }
        public DbSet<AuditRecord> AuditRecords { get; set; }
    }
}
