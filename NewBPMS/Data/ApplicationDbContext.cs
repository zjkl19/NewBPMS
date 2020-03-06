using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewBPMS.Models;

namespace NewBPMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<UserContract> UserContracts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //User-Contract
            builder.Entity<Contract>()
                .HasOne(c => c.ApplicationUser)
                .WithMany(s => s.Contracts)
                .HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Contract>()
                .HasOne<ApplicationUser>(c => c.AcceptUser)
                .WithMany(s => s.AcceptContracts)
                .HasForeignKey(c => c.AcceptUserId).OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);

        }
    }
}
