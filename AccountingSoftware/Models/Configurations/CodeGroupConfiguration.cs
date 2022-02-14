using AccountingSoftware.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Models.Configurations
{
    public class CodeGroupConfiguration : IEntityTypeConfiguration<CodeGroup>
    {
        public void Configure(EntityTypeBuilder<CodeGroup> entity)
        {
            entity.Property(e => e.CodeGroupGuid)
                .HasDefaultValueSql("(newid())");

            entity.Property(e => e.IsDelete)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())");
        }
    }
}
