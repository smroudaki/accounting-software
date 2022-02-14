using AccountingSoftware.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Models.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> entity)
        {
            entity.Property(e => e.Credit)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.TransactionGuid)
                .HasDefaultValueSql("(newid())");

            entity.Property(e => e.IsCheckTransaction)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.IsDelete)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Account)
                .WithMany(p => p.Transaction)
                .HasForeignKey(d => d.AccountGuid)
                .HasConstraintName("FK_Transaction_Account");

            entity.HasOne(d => d.StateCode)
                .WithMany(p => p.TransactionStateCode)
                .HasForeignKey(d => d.StateCodeGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StateTransaction_StateCode");

            entity.HasOne(d => d.TypeCode)
                .WithMany(p => p.TransactionTypeCode)
                .HasForeignKey(d => d.TypeCodeGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TypeTransaction_TypeCode");
        }
    }
}
