using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SSDAssignment40.Data
{
    public class ApplicationDbContext : IdentityDbContext<Lodger>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Booking> Booking { get; set; }
        public DbSet<Listing> Listing { get; set; }
        public DbSet<Review> Review { get; set; }

    }
}
