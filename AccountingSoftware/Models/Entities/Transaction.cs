using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingSoftware.Models.Entities
{
    public partial class Transaction
    {
        public Transaction()
        {
            CheckTransaction = new HashSet<CheckTransaction>();
        }


        [Key]
        public Guid TransactionGuid { get; set; }

        public Guid? AccountGuid { get; set; }

        public Guid TypeCodeGuid { get; set; }

        public Guid StateCodeGuid { get; set; }

        [StringLength(512)]
        public string Title { get; set; }

        public long Cost { get; set; }

        public long Credit { get; set; }

        [StringLength(128)]
        public string AccountSide { get; set; }

        public string Description { get; set; }

        public DateTime ReceiptDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsCheckTransaction { get; set; }

        public bool IsDelete { get; set; }


        [ForeignKey(nameof(AccountGuid))]
        [InverseProperty(nameof(Entities.Account.Transaction))]
        public virtual Account Account { get; set; }

        [ForeignKey(nameof(StateCodeGuid))]
        [InverseProperty(nameof(Code.TransactionStateCode))]
        public virtual Code StateCode { get; set; }

        [ForeignKey(nameof(TypeCodeGuid))]
        [InverseProperty(nameof(Code.TransactionTypeCode))]
        public virtual Code TypeCode { get; set; }

        [InverseProperty("Transaction")]
        public virtual ICollection<CheckTransaction> CheckTransaction { get; set; }
    }
}
