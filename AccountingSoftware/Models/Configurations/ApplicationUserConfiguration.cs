using AccountingSoftware.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Models.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            entity.Property(e => e.IsDelete)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.RegisteredDate)
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())");
        }
    }
}
