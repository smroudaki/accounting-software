using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingSoftware.Models.Entities
{
    public partial class CodeGroup
    {
        public CodeGroup()
        {
            Code = new HashSet<Code>();
        }


        [Key]
        public Guid CodeGroupGuid { get; set; }

        [Required]
        [StringLength(128)]
        public string Key { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsDelete { get; set; }


        [InverseProperty("CodeGroup")]
        public virtual ICollection<Code> Code { get; set; }
    }
}
