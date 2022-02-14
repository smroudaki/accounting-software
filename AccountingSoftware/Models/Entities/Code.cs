using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingSoftware.Models.Entities
{
    public partial class Code
    {
        public Code()
        {
            TransactionStateCode = new HashSet<Transaction>();
            TransactionTypeCode = new HashSet<Transaction>();
        }


        [Key]
        public Guid CodeGuid { get; set; }

        public Guid CodeGroupGuid { get; set; }

        [Required]
        [StringLength(128)]
        public string Value { get; set; }

        [Required]
        [StringLength(128)]
        public string DisplayValue { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsDelete { get; set; }


        [ForeignKey(nameof(CodeGroupGuid))]
        [InverseProperty(nameof(Entities.CodeGroup.Code))]
        public virtual CodeGroup CodeGroup { get; set; }

        [InverseProperty(nameof(Transaction.StateCode))]
        public virtual ICollection<Transaction> TransactionStateCode { get; set; }

        [InverseProperty(nameof(Transaction.TypeCode))]
        public virtual ICollection<Transaction> TransactionTypeCode { get; set; }
    }
}