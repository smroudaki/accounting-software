using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Models.Entities
{
    public partial class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Account = new HashSet<Account>();
        }


        [Required]
        [StringLength(128)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(128)]
        public string LastName { get; set; }

        [StringLength(128)]
        public string NationalCode { get; set; }

        public string Address { get; set; }

        public DateTime RegisteredDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsDelete { get; set; }


        [InverseProperty("User")]
        public virtual ICollection<Account> Account { get; set; }
    }
}
