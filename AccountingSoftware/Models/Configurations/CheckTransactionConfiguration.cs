using AccountingSoftware.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Models.Configurations
{
    public class CheckTransactionConfiguration : IEntityTypeConfiguration<CheckTransaction>
    {
        public void Configure(EntityTypeBuilder<CheckTransaction> entity)
        {
            entity.Property(e => e.CheckTransactionGuid)
                .HasDefaultValueSql("(newid())");

            entity.Property(e => e.IssueDate)
                .HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Transaction)
                .WithMany(p => p.CheckTransaction)
                .HasForeignKey(d => d.TransactionGuid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CheckTransaction_Transaction");
        }
    }
}
