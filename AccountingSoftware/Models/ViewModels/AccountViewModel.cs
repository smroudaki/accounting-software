using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Models.ViewModels
{
    public class AccountViewModel
    {
        public Guid AccountGuid { get; set; }

        [Display(Name = "صاحب حساب")]
        public string UserFullName { get; set; }

        [Display(Name = "نام بانک")]
        public string BankName { get; set; }

        [Display(Name = "نوع حساب")]
        public string AccountName { get; set; }

        [Display(Name = "شماره حساب")]
        public string AccountNumber { get; set; }

        [Display(Name = "شماره کارت")]
        public string CardNumber { get; set; }

        [Display(Name = "اعتبار (تومان)")]
        public long Credit { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        public string ModifiedDate { get; set; }
    }
}
