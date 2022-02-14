using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingSoftware.Common;
using AccountingSoftware.Models;
using AccountingSoftware.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSoftware.Controllers
{
    public class ReportsController : Controller
    {
        private readonly AccountingSoftwareContext context;

        public ReportsController(AccountingSoftwareContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Remindings()
        {
            var transactions = context.Transaction
                .Where(x => !x.IsDelete && x.ReceiptDate > DateTime.Now && x.ReceiptDate < DateTime.Now.AddDays(10) && x.StateCodeGuid == Codes.WaitingState)
                .OrderByDescending(x => x.ReceiptDate)
                .Select(x => new TransactionViewModel
                {
                    TransactionGuid = x.TransactionGuid,
                    AccountNumber = string.IsNullOrEmpty(x.Account.AccountNumber) ? Messages.NotSet : x.Account.AccountNumber,
                    Type = x.TypeCode.DisplayValue,
                    State = x.StateCode.DisplayValue,
                    Cost = x.Cost,
                    AccountSide = x.AccountSide,
                    Description = string.IsNullOrEmpty(x.Description) ? Messages.NotSet : x.Description,
                    IsCheckTransaction = x.IsCheckTransaction,
                    ReceiptDate = PersianDateExtensionMethods.ToPeString(x.ReceiptDate, "yyyy/MM/dd"),
                    ModifiedDate = PersianDateExtensionMethods.ToPeString(x.ModifiedDate, "yyyy/MM/dd HH:mm")

                }).ToList();

            return View(transactions);
        }

        [HttpGet]
        public IActionResult PassedEvents()
        {
            var transactions = context.Transaction
                .Where(x => !x.IsDelete && x.ReceiptDate < DateTime.Now && x.StateCodeGuid == Codes.WaitingState)
                .OrderByDescending(x => x.ReceiptDate)
                .Select(x => new TransactionViewModel
                {
                    TransactionGuid = x.TransactionGuid,
                    AccountNumber = string.IsNullOrEmpty(x.Account.AccountNumber) ? Messages.NotSet : x.Account.AccountNumber,
                    Type = x.TypeCode.DisplayValue,
                    State = x.StateCode.DisplayValue,
                    Cost = x.Cost,
                    AccountSide = x.AccountSide,
                    Description = string.IsNullOrEmpty(x.Description) ? Messages.NotSet : x.Description,
                    IsCheckTransaction = x.IsCheckTransaction,
                    ReceiptDate = PersianDateExtensionMethods.ToPeString(x.ReceiptDate, "yyyy/MM/dd"),
                    ModifiedDate = PersianDateExtensionMethods.ToPeString(x.ModifiedDate, "yyyy/MM/dd HH:mm")

                }).ToList();

            return View(transactions);
        }

        [HttpGet]
        public IActionResult PassedStateTransactions()
        {
            var transactions = context.Transaction
                .Where(x => !x.IsDelete && x.StateCodeGuid == Codes.PassedState)
                .OrderByDescending(x => x.ReceiptDate)
                .Select(x => new TransactionViewModel
                {
                    TransactionGuid = x.TransactionGuid,
                    AccountNumber = string.IsNullOrEmpty(x.Account.AccountNumber) ? Messages.NotSet : x.Account.AccountNumber,
                    Type = x.TypeCode.DisplayValue,
                    State = x.StateCode.DisplayValue,
                    Cost = x.Cost,
                    AccountSide = x.AccountSide,
                    Description = string.IsNullOrEmpty(x.Description) ? Messages.NotSet : x.Description,
                    IsCheckTransaction = x.IsCheckTransaction,
                    ReceiptDate = PersianDateExtensionMethods.ToPeString(x.ReceiptDate, "yyyy/MM/dd"),
                    ModifiedDate = PersianDateExtensionMethods.ToPeString(x.ModifiedDate, "yyyy/MM/dd HH:mm")

                }).ToList();

            return View(transactions);
        }

        [HttpGet]
        public IActionResult NotPassedStateTransactions()
        {
            var transactions = context.Transaction
                .Where(x => !x.IsDelete && x.StateCodeGuid == Codes.NotPassedState)
                .OrderByDescending(x => x.ReceiptDate)
                .Select(x => new TransactionViewModel
                {
                    TransactionGuid = x.TransactionGuid,
                    AccountNumber = string.IsNullOrEmpty(x.Account.AccountNumber) ? Messages.NotSet : x.Account.AccountNumber,
                    Type = x.TypeCode.DisplayValue,
                    State = x.StateCode.DisplayValue,
                    Cost = x.Cost,
                    AccountSide = x.AccountSide,
                    Description = string.IsNullOrEmpty(x.Description) ? Messages.NotSet : x.Description,
                    IsCheckTransaction = x.IsCheckTransaction,
                    ReceiptDate = PersianDateExtensionMethods.ToPeString(x.ReceiptDate, "yyyy/MM/dd"),
                    ModifiedDate = PersianDateExtensionMethods.ToPeString(x.ModifiedDate, "yyyy/MM/dd HH:mm")

                }).ToList();

            return View(transactions);
        }

        [HttpGet]
        public IActionResult WaitingStateTransactions()
        {
            var transactions = context.Transaction
                .Where(x => !x.IsDelete && x.StateCodeGuid == Codes.WaitingState)
                .OrderByDescending(x => x.ReceiptDate)
                .Select(x => new TransactionViewModel
                {
                    TransactionGuid = x.TransactionGuid,
                    AccountNumber = string.IsNullOrEmpty(x.Account.AccountNumber) ? Messages.NotSet : x.Account.AccountNumber,
                    Type = x.TypeCode.DisplayValue,
                    State = x.StateCode.DisplayValue,
                    Cost = x.Cost,
                    AccountSide = x.AccountSide,
                    Description = string.IsNullOrEmpty(x.Description) ? Messages.NotSet : x.Description,
                    IsCheckTransaction = x.IsCheckTransaction,
                    ReceiptDate = PersianDateExtensionMethods.ToPeString(x.ReceiptDate, "yyyy/MM/dd"),
                    ModifiedDate = PersianDateExtensionMethods.ToPeString(x.ModifiedDate, "yyyy/MM/dd HH:mm")

                }).ToList();

            return View(transactions);
        }
    }
}