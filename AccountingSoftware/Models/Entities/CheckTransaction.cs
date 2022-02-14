using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingSoftware.Models.Entities
{
    public partial class CheckTransaction
    {
        [Key]
        public Guid CheckTransactionGuid { get; set; }

        public Guid TransactionGuid { get; set; }

        [StringLength(128)]
        public string Serial { get; set; }

        public DateTime IssueDate { get; set; }

        [ForeignKey(nameof(TransactionGuid))]
        [InverseProperty(nameof(Entities.Transaction.CheckTransaction))]
        public virtual Transaction Transaction { get; set; }
    }
}
