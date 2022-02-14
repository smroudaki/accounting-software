using System;
using System.Reflection;
using AccountingSoftware.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AccountingSoftware.Models
{
    public class AccountingSoftwareContext : IdentityDbContext<ApplicationUser>
    {
        public AccountingSoftwareContext(DbContextOptions<AccountingSoftwareContext> options)
            : base(options)
        {
        }


        public virtual DbSet<Account> Account { get; set; }

        public virtual DbSet<CheckTransaction> CheckTransaction { get; set; }

        public virtual DbSet<Code> Code { get; set; }

        public virtual DbSet<CodeGroup> CodeGroup { get; set; }

        public virtual DbSet<Transaction> Transaction { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Seed();

            base.OnModelCreating(builder);
        }
    }
}
