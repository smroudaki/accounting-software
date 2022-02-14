using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Models.ViewModels
{
    public class ChangeTransactionStateViewModel
    {
        public Guid Guid { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid StateGuid { get; set; }
    }
}
