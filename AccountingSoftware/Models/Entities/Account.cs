using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingSoftware.Models.Entities
{
    public partial class Account
    {
        public Account()
        {
            Transaction = new HashSet<Transaction>();
        }


        [Key]
        public Guid AccountGuid { get; set; }

        [Required]
        public string UserGuid { get; set; }

        [StringLength(128)]
        public string BankName { get; set; }

        [StringLength(128)]
        public string AccountName { get; set; }

        [Required]
        [StringLength(128)]
        public string AccountNumber { get; set; }

        [StringLength(128)]
        public string CardNumber { get; set; }

        public long Credit { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsDelete { get; set; }


        [ForeignKey(nameof(UserGuid))]
        [InverseProperty(nameof(ApplicationUser.Account))]
        public virtual ApplicationUser User { get; set; }

        [InverseProperty("Account")]
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
