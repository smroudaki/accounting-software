using AccountingSoftware.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Models.Configurations
{
    public class CodeConfiguration : IEntityTypeConfiguration<Code>
    {
        public void Configure(EntityTypeBuilder<Code> entity)
        {
            entity.Property(e => e.CodeGuid)
                .HasDefaultValueSql("(newid())");

            entity.Property(e => e.IsDelete)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.CodeGroup)
                .WithMany(p => p.Code)
                .HasForeignKey(d => d.CodeGroupGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Code_CodeGroup");
        }
    }
}
