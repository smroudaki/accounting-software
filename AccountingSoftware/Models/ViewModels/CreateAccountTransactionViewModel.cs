using AccountingSoftware.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Models.ViewModels
{
    public class CreateAccountTransactionViewModel
    {
        private string receiptDate, cost;
        private readonly string now = PersianDateExtensionMethods.ToPeString(DateTime.Now, "yyyy-MM-dd HH:mm:ss");

        [Display(Name = "حساب")]
        public Guid? AccountGuid { get; set; }

        [Display(Name = "نوع")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid TypeGuid { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid StateGuid { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "مبلغ (تومان)")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Cost
        {
            get
            {
                return cost.Replace(",", "");
            }
            set
            {
                cost = value;
            }
        }

        [Display(Name = "طرف حساب")]
        public string AccountSide { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "تاریخ وصول")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string ReceiptDate
        {
            get
            {
                return string.IsNullOrEmpty(receiptDate) ? now : receiptDate;
            }
            set
            {
                receiptDate = value;
            }
        }
    }
}
