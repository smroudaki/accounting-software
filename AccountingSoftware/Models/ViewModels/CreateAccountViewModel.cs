using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Models.ViewModels
{
    public class CreateAccountViewModel
    {
        private string credit;

        [Display(Name = "نام بانک")]
        public string BankName { get; set; }

        [Display(Name = "نوع حساب")]
        public string AccountName { get; set; }

        [Display(Name = "شماره حساب")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string AccountNumber { get; set; }

        [Display(Name = "شماره کارت")]
        public string CardNumber { get; set; }

        [Display(Name = "اعتبار (تومان)")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Credit
        {
            get
            {
                return credit.Replace(",", "");
            }
            set
            {
                credit = value;
            }
        }
    }
}
