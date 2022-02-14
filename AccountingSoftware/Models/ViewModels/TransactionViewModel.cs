using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Models.ViewModels
{
    public class TransactionViewModel
    {
        public Guid TransactionGuid { get; set; }

        [Display(Name = "شماره حساب")]
        public string AccountNumber { get; set; }

        [Display(Name = "نوع")]
        public string Type { get; set; }

        [Display(Name = "وضعیت")]
        public string State { get; set; }

        [Display(Name = "مبلغ (تومان)")]
        public long Cost { get; set; }

        [Display(Name = "اعتبار (تومان)")]
        public long Credit { get; set; }

        [Display(Name = "طرف حساب")]
        public string AccountSide { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "تاریخ وصول")]
        public string ReceiptDate { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        public string ModifiedDate { get; set; }

        [Display(Name = "چک")]
        public bool IsCheckTransaction { get; set; }
    }
}
