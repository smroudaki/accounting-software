using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "رمز فعلی")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string CurrentPassword { get; set; }

        [Display(Name = "رمز جدید")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string NewPassword { get; set; }

        [Display(Name = "تایید رمز جدید")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "رمز جدید و تایید رمز جدید مطابق نیست")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string ConfirmNewPassword { get; set; }
    }
}
