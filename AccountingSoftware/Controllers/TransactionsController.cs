using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingSoftware.Common;
using AccountingSoftware.Models;
using AccountingSoftware.Models.Entities;
using AccountingSoftware.Models.Enums;
using AccountingSoftware.Models.Repositories;
using AccountingSoftware.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSoftware.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly AccountingSoftwareContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public TransactionsController(AccountingSoftwareContext context,
                UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var transactions = context.Transaction
                .Where(x => !x.IsDelete)
                .OrderByDescending(x => x.ReceiptDate)
                .Select(x => new TransactionViewModel()
                {
                    TransactionGuid = x.TransactionGuid,
                    AccountNumber = string.IsNullOrEmpty(x.Account.AccountNumber) ? Messages.NotSet : x.Account.AccountNumber,
                    Type = x.TypeCode.DisplayValue,
                    State = x.StateCode.DisplayValue,
                    Cost = x.Cost,
                    Credit = x.Credit,
                    AccountSide = string.IsNullOrEmpty(x.AccountSide) ? Messages.NotSet : x.AccountSide,
                    Description = string.IsNullOrEmpty(x.Description) ? Messages.NotSet : x.Description,
                    IsCheckTransaction = x.IsCheckTransaction,
                    ReceiptDate = PersianDateExtensionMethods.ToPeString(x.ReceiptDate, "yyyy/MM/dd"),
                    ModifiedDate = PersianDateExtensionMethods.ToPeString(x.ModifiedDate, "yyyy/MM/dd HH:mm")

                }).ToList();

            return View(transactions);
        }

        [HttpGet]
        public IActionResult CreateAccountTransaction()
        {
            CodesRepository codeRepository = new CodesRepository(context);

            ViewBag.Accounts = new AccountsRepository(context).GetAccounts(userManager.GetUserId(User));
            ViewBag.TransactionTypes = codeRepository.GetCodesByCodeGroupGuid(CodeGroups.TransactionType);
            ViewBag.TransactionStates = codeRepository.GetCodesByCodeGroupGuid(CodeGroups.TransactionState);

            return View();
        }

        [HttpPost]
        public IActionResult CreateAccountTransaction(CreateAccountTransactionViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.AccountGuid = Constants.DefaultAccountGuid;
                long cost = Convert.ToInt64(model.Cost);

                Transaction transaction = new Transaction
                {
                    AccountGuid = model.AccountGuid,
                    TypeCodeGuid = model.TypeGuid,
                    StateCodeGuid = model.StateGuid,
                    Title = model.Title,
                    Cost = cost,
                    AccountSide = model.AccountSide,
                    Description = model.Description,
                    ReceiptDate = PersianDateExtensionMethods.ToGeorgianDateTime(model.ReceiptDate)
                };

                if (model.AccountGuid.HasValue)
                {
                    if (model.StateGuid == Codes.PassedState)
                    {
                        var account = context.Account
                            .Where(x => x.AccountGuid == model.AccountGuid)
                            .SingleOrDefault();

                        if (account == null)
                        {
                            TempData["ToasterState"] = ToasterState.Error;
                            TempData["ToasterType"] = ToasterType.Message;
                            TempData["ToasterMessage"] = Messages.CreateTransactionFailedAccountNotValid;

                            return RedirectToAction("Index");
                        }

                        account.Credit = model.TypeGuid == Codes.CreditorType ? account.Credit + cost : account.Credit - cost;
                        account.ModifiedDate = DateTime.Now;

                        var transactionBefore = context.Transaction
                            .Where(x => !x.IsDelete && x.ReceiptDate < PersianDateExtensionMethods.ToGeorgianDateTime(model.ReceiptDate))
                            .OrderByDescending(x => x.ReceiptDate)
                            .FirstOrDefault();

                        if (transactionBefore == null)
                        {
                            transaction.Credit = model.TypeGuid == Codes.CreditorType ? transaction.Credit + cost : transaction.Credit - cost;
                        }
                        else
                        {
                            transaction.Credit = model.TypeGuid == Codes.CreditorType ? transactionBefore.Credit + cost : transactionBefore.Credit - cost;
                        }

                        var transactionsAfter = context.Transaction
                            .Where(x => !x.IsDelete && x.ReceiptDate > PersianDateExtensionMethods.ToGeorgianDateTime(model.ReceiptDate))
                            .OrderBy(x => x.ReceiptDate)
                            .ToList();

                        if (transactionsAfter.Count > 0)
                        {
                            transactionsAfter[0].Credit = transactionsAfter[0].StateCodeGuid == Codes.PassedState ?
                                (transactionsAfter[0].TypeCodeGuid == Codes.CreditorType ? transaction.Credit + transactionsAfter[0].Cost : transaction.Credit - transactionsAfter[0].Cost) :
                                (transactionsAfter[0].Credit = transaction.Credit);

                            for (int i = 1; i < transactionsAfter.Count; i++)
                            {
                                transactionsAfter[i].Credit = transactionsAfter[i].StateCodeGuid == Codes.PassedState ?
                                    (transactionsAfter[i].TypeCodeGuid == Codes.CreditorType ? transactionsAfter[i - 1].Credit + transactionsAfter[i].Cost : transactionsAfter[i - 1].Credit - transactionsAfter[i].Cost) :
                                    (transactionsAfter[i].Credit = transactionsAfter[i - 1].Credit);
                            }
                        }
                    }
                    else
                    {
                        var transactionBefore = context.Transaction
                            .Where(x => !x.IsDelete && x.ReceiptDate < PersianDateExtensionMethods.ToGeorgianDateTime(model.ReceiptDate))
                            .OrderByDescending(x => x.ReceiptDate)
                            .FirstOrDefault();

                        transaction.Credit = transactionBefore == null ? 0 : transactionBefore.Credit;
                    }
                }

                context.Transaction.Add(transaction);

                if (Convert.ToBoolean(context.SaveChanges() > 0))
                {
                    TempData["ToasterState"] = ToasterState.Success;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.CreateTransactionSuccessful;
                }
                else
                {
                    TempData["ToasterState"] = ToasterState.Error;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.CreateTransactionFailed;
                }

                return RedirectToAction("Index");
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult CreateCheckTransaction()
        {
            CodesRepository codeRepository = new CodesRepository(context);

            ViewBag.Accounts = new AccountsRepository(context).GetAccounts(userManager.GetUserId(User));
            ViewBag.TransactionTypes = codeRepository.GetCodesByCodeGroupGuid(CodeGroups.TransactionType);
            ViewBag.TransactionStates = codeRepository.GetCodesByCodeGroupGuid(CodeGroups.TransactionState);

            return View();
        }


        [HttpPost]
        public IActionResult CreateCheckTransaction(CreateCheckTransactionViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.AccountGuid = Constants.DefaultAccountGuid;
                long cost = Convert.ToInt64(model.Cost);

                Transaction transaction = new Transaction
                {
                    AccountGuid = model.AccountGuid,
                    TypeCodeGuid = model.TypeGuid,
                    StateCodeGuid = model.StateGuid,
                    Title = model.Title,
                    Cost = cost,
                    AccountSide = model.AccountSide,
                    Description = model.Description,
                    IsCheckTransaction = true,
                    ReceiptDate = PersianDateExtensionMethods.ToGeorgianDateTime(model.ReceiptDate)
                };

                CheckTransaction checkTransaction = new CheckTransaction
                {
                    Serial = model.Serial,
                    IssueDate = PersianDateExtensionMethods.ToGeorgianDateTime(model.IssueDate)
                };

                checkTransaction.Transaction = transaction;

                if (model.AccountGuid.HasValue)
                {
                    if (model.StateGuid == Codes.PassedState)
                    {
                        var account = context.Account
                            .Where(x => x.AccountGuid == model.AccountGuid)
                            .SingleOrDefault();

                        if (account == null)
                        {
                            TempData["ToasterState"] = ToasterState.Error;
                            TempData["ToasterType"] = ToasterType.Message;
                            TempData["ToasterMessage"] = Messages.CreateTransactionFailedAccountNotValid;

                            return RedirectToAction("Index");
                        }

                        account.Credit = model.TypeGuid == Codes.CreditorType ? account.Credit + cost : account.Credit - cost;
                        account.ModifiedDate = DateTime.Now;

                        var transactionBefore = context.Transaction
                            .Where(x => !x.IsDelete && x.ReceiptDate < PersianDateExtensionMethods.ToGeorgianDateTime(model.ReceiptDate))
                            .OrderByDescending(x => x.ReceiptDate)
                            .FirstOrDefault();

                        if (transactionBefore == null)
                        {
                            transaction.Credit = model.TypeGuid == Codes.CreditorType ? transaction.Credit + cost : transaction.Credit - cost;
                        }
                        else
                        {
                            transaction.Credit = model.TypeGuid == Codes.CreditorType ? transactionBefore.Credit + cost : transactionBefore.Credit - cost;
                        }

                        var transactionsAfter = context.Transaction
                            .Where(x => !x.IsDelete && x.ReceiptDate > PersianDateExtensionMethods.ToGeorgianDateTime(model.ReceiptDate))
                            .OrderBy(x => x.ReceiptDate)
                            .ToList();

                        if (transactionsAfter.Count > 0)
                        {
                            transactionsAfter[0].Credit = transactionsAfter[0].StateCodeGuid == Codes.PassedState ?
                                (transactionsAfter[0].TypeCodeGuid == Codes.CreditorType ? transaction.Credit + transactionsAfter[0].Cost : transaction.Credit - transactionsAfter[0].Cost) :
                                (transactionsAfter[0].Credit = transaction.Credit);

                            for (int i = 1; i < transactionsAfter.Count; i++)
                            {
                                transactionsAfter[i].Credit = transactionsAfter[i].StateCodeGuid == Codes.PassedState ?
                                    (transactionsAfter[i].TypeCodeGuid == Codes.CreditorType ? transactionsAfter[i - 1].Credit + transactionsAfter[i].Cost : transactionsAfter[i - 1].Credit - transactionsAfter[i].Cost) :
                                    (transactionsAfter[i].Credit = transactionsAfter[i - 1].Credit);
                            }
                        }
                    }
                    else
                    {
                        var transactionBefore = context.Transaction
                            .Where(x => !x.IsDelete && x.ReceiptDate < PersianDateExtensionMethods.ToGeorgianDateTime(model.ReceiptDate))
                            .OrderByDescending(x => x.ReceiptDate)
                            .FirstOrDefault();

                        transaction.Credit = transactionBefore == null ? 0 : transactionBefore.Credit;
                    }
                }

                context.Transaction.Add(transaction);
                context.CheckTransaction.Add(checkTransaction);

                if (Convert.ToBoolean(context.SaveChanges() > 0))
                {
                    TempData["ToasterState"] = ToasterState.Success;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.CreateTransactionSuccessful;
                }
                else
                {
                    TempData["ToasterState"] = ToasterState.Error;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.CreateTransactionFailed;
                }

                return RedirectToAction("Index");
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult EditAccountTransaction(Guid transactionGuid)
        {
            if (transactionGuid == null)
            {
                return BadRequest();
            }

            var transaction = context.Transaction
                .Where(x => x.TransactionGuid == transactionGuid)
                .SingleOrDefault();

            if (transaction == null)
            {
                return NotFound();
            }

            EditAccountTransactionViewModel model = new EditAccountTransactionViewModel()
            {
                TransactionGuid = transaction.TransactionGuid,
                AccountGuid = transaction.AccountGuid,
                TypeGuid = transaction.TypeCodeGuid,
                StateGuid = transaction.StateCodeGuid,
                Title = transaction.Title,
                Cost = transaction.Cost.ToString(),
                AccountSide = transaction.AccountSide,
                Description = transaction.Description,
                ReceiptDate = transaction.ReceiptDate.ToString()
            };

            CodesRepository codeRepository = new CodesRepository(context);

            ViewBag.Accounts = new AccountsRepository(context).GetAccounts(userManager.GetUserId(User));
            ViewBag.TransactionTypes = codeRepository.GetCodesByCodeGroupGuid(CodeGroups.TransactionType);
            ViewBag.TransactionStates = codeRepository.GetCodesByCodeGroupGuid(CodeGroups.TransactionState);

            return View(model);
        }

        [HttpPost]
        public IActionResult EditAccountTransaction(EditAccountTransactionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var transaction = context.Transaction
                    .Where(x => x.TransactionGuid == model.TransactionGuid)
                    .SingleOrDefault();

                if (transaction == null)
                {
                    NotFound();
                }

                var account = context.Account
                    .Where(x => x.AccountGuid == transaction.AccountGuid)
                    .SingleOrDefault();

                if (account == null)
                {
                    TempData["ToasterState"] = ToasterState.Error;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.CreateTransactionFailedAccountNotValid;

                    return RedirectToAction("Index");
                }

                if (transaction.AccountGuid.HasValue && transaction.StateCodeGuid == Codes.PassedState)
                {
                    account.Credit = transaction.TypeCodeGuid == Codes.CreditorType ? account.Credit - transaction.Cost : account.Credit + transaction.Cost;
                    account.ModifiedDate = DateTime.Now;
                }

                model.AccountGuid = Constants.DefaultAccountGuid;
                long cost = Convert.ToInt64(model.Cost);
                DateTime transactionOldReceiptDate = transaction.ReceiptDate;

                if (model.AccountGuid.HasValue && model.StateGuid == Codes.PassedState)
                {
                    account.Credit = model.TypeGuid == Codes.CreditorType ? account.Credit + cost : account.Credit - cost;
                    account.ModifiedDate = DateTime.Now;
                }

                transaction.AccountGuid = model.AccountGuid;
                transaction.TypeCodeGuid = model.TypeGuid;
                transaction.StateCodeGuid = model.StateGuid;
                transaction.Title = model.Title;
                transaction.Cost = cost;
                transaction.AccountSide = model.AccountSide;
                transaction.Description = model.Description;
                transaction.ReceiptDate = PersianDateExtensionMethods.ToGeorgianDateTime(model.ReceiptDate);
                transaction.ModifiedDate = DateTime.Now;

                DateTime dateToUpdateFrom = transactionOldReceiptDate < transaction.ReceiptDate ? transactionOldReceiptDate : transaction.ReceiptDate;

                var transactionBefore = context.Transaction
                    .Where(x => !x.IsDelete && x.ReceiptDate < dateToUpdateFrom)
                    .OrderByDescending(x => x.ReceiptDate)
                    .FirstOrDefault();

                long transactionBeforeCredit = transactionBefore == null ? 0 : transactionBefore.Credit;

                var transactionsAfter = context.Transaction
                    .Where(x => !x.IsDelete && x.ReceiptDate > dateToUpdateFrom && x.TransactionGuid != transaction.TransactionGuid)
                    .OrderBy(x => x.ReceiptDate)
                    .ToList();

                transactionsAfter.Add(transaction);
                transactionsAfter = transactionsAfter.OrderBy(x => x.ReceiptDate)
                    .ToList();

                if (transactionsAfter.Count > 0)
                {
                    transactionsAfter[0].Credit = transactionsAfter[0].StateCodeGuid == Codes.PassedState ?
                        (transactionsAfter[0].TypeCodeGuid == Codes.CreditorType ? transactionBeforeCredit + transactionsAfter[0].Cost : transactionBeforeCredit - transactionsAfter[0].Cost) :
                        (transactionsAfter[0].Credit = transactionBeforeCredit);

                    for (int i = 1; i < transactionsAfter.Count; i++)
                    {
                        transactionsAfter[i].Credit = transactionsAfter[i].StateCodeGuid == Codes.PassedState ?
                            (transactionsAfter[i].TypeCodeGuid == Codes.CreditorType ? transactionsAfter[i - 1].Credit + transactionsAfter[i].Cost : transactionsAfter[i - 1].Credit - transactionsAfter[i].Cost) :
                            (transactionsAfter[i].Credit = transactionsAfter[i - 1].Credit);
                    }
                }

                if (Convert.ToBoolean(context.SaveChanges() > 0))
                {
                    TempData["ToasterState"] = ToasterState.Success;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.EditTransactionSuccessful;
                }
                else
                {
                    TempData["ToasterState"] = ToasterState.Error;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.EditTransactionFailed;
                }

                return RedirectToAction("Index");
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult EditCheckTransaction(Guid transactionGuid)
        {
            if (transactionGuid == null)
            {
                return BadRequest();
            }

            var checkTransaction = context.CheckTransaction
                .Where(x => x.TransactionGuid == transactionGuid)
                .SingleOrDefault();

            if (checkTransaction == null)
            {
                return NotFound();
            }

            var transaction = context.Transaction
                .Where(x => x.TransactionGuid == transactionGuid)
                .SingleOrDefault();

            EditCheckTransactionViewModel model = new EditCheckTransactionViewModel()
            {
                TransactionGuid = transaction.TransactionGuid,
                AccountGuid = transaction.AccountGuid,
                TypeGuid = transaction.TypeCodeGuid,
                StateGuid = transaction.StateCodeGuid,
                Title = transaction.Title,
                Cost = transaction.Cost.ToString(),
                AccountSide = transaction.AccountSide,
                Serial = checkTransaction.Serial,
                Description = transaction.Description,
                IssueDate = checkTransaction.IssueDate.ToString(),
                ReceiptDate = transaction.ReceiptDate.ToString()
            };

            CodesRepository codeRepository = new CodesRepository(context);

            ViewBag.Accounts = new AccountsRepository(context).GetAccounts(userManager.GetUserId(User));
            ViewBag.TransactionTypes = codeRepository.GetCodesByCodeGroupGuid(CodeGroups.TransactionType);
            ViewBag.TransactionStates = codeRepository.GetCodesByCodeGroupGuid(CodeGroups.TransactionState);

            return View(model);
        }

        [HttpPost]
        public IActionResult EditCheckTransaction(EditCheckTransactionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var checkTransaction = context.CheckTransaction
                    .Where(x => x.TransactionGuid == model.TransactionGuid)
                    .SingleOrDefault();

                if (checkTransaction == null)
                {
                    return NotFound();
                }

                var transaction = context.Transaction
                    .Where(x => x.TransactionGuid == model.TransactionGuid)
                    .SingleOrDefault();

                var account = context.Account
                    .Where(x => x.AccountGuid == transaction.AccountGuid)
                    .SingleOrDefault();

                if (transaction.AccountGuid.HasValue && transaction.StateCodeGuid == Codes.PassedState)
                {
                    account.Credit = transaction.TypeCodeGuid == Codes.CreditorType ? account.Credit - transaction.Cost : account.Credit + transaction.Cost;
                    account.ModifiedDate = DateTime.Now;
                }

                model.AccountGuid = Constants.DefaultAccountGuid;
                long cost = Convert.ToInt64(model.Cost);
                DateTime transactionOldReceiptDate = transaction.ReceiptDate;

                if (model.AccountGuid.HasValue && model.StateGuid == Codes.PassedState)
                {
                    account.Credit = model.TypeGuid == Codes.CreditorType ? account.Credit + cost : account.Credit - cost;
                    account.ModifiedDate = DateTime.Now;
                }

                transaction.AccountGuid = model.AccountGuid;
                transaction.TypeCodeGuid = model.TypeGuid;
                transaction.StateCodeGuid = model.StateGuid;
                transaction.Title = model.Title;
                transaction.Cost = cost;
                transaction.AccountSide = model.AccountSide;
                checkTransaction.Serial = model.Serial;
                transaction.Description = model.Description;
                checkTransaction.IssueDate = PersianDateExtensionMethods.ToGeorgianDateTime(model.IssueDate);
                transaction.ReceiptDate = PersianDateExtensionMethods.ToGeorgianDateTime(model.ReceiptDate);
                transaction.ModifiedDate = DateTime.Now;

                DateTime dateToUpdateFrom = transactionOldReceiptDate < transaction.ReceiptDate ? transactionOldReceiptDate : transaction.ReceiptDate;

                var transactionBefore = context.Transaction
                    .Where(x => !x.IsDelete && x.ReceiptDate < dateToUpdateFrom)
                    .OrderByDescending(x => x.ReceiptDate)
                    .FirstOrDefault();

                long transactionBeforeCredit = transactionBefore == null ? 0 : transactionBefore.Credit;

                var transactionsAfter = context.Transaction
                    .Where(x => !x.IsDelete && x.ReceiptDate > dateToUpdateFrom && x.TransactionGuid != transaction.TransactionGuid)
                    .OrderBy(x => x.ReceiptDate)
                    .ToList();

                transactionsAfter.Add(transaction);
                transactionsAfter = transactionsAfter.OrderBy(x => x.ReceiptDate)
                    .ToList();

                if (transactionsAfter.Count > 0)
                {
                    transactionsAfter[0].Credit = transactionsAfter[0].StateCodeGuid == Codes.PassedState ?
                        (transactionsAfter[0].TypeCodeGuid == Codes.CreditorType ? transactionBeforeCredit + transactionsAfter[0].Cost : transactionBeforeCredit - transactionsAfter[0].Cost) :
                        (transactionsAfter[0].Credit = transactionBeforeCredit);

                    for (int i = 1; i < transactionsAfter.Count; i++)
                    {
                        transactionsAfter[i].Credit = transactionsAfter[i].StateCodeGuid == Codes.PassedState ?
                            (transactionsAfter[i].TypeCodeGuid == Codes.CreditorType ? transactionsAfter[i - 1].Credit + transactionsAfter[i].Cost : transactionsAfter[i - 1].Credit - transactionsAfter[i].Cost) :
                            (transactionsAfter[i].Credit = transactionsAfter[i - 1].Credit);
                    }
                }

                if (Convert.ToBoolean(context.SaveChanges() > 0))
                {
                    TempData["ToasterState"] = ToasterState.Success;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.EditTransactionSuccessful;
                }
                else
                {
                    TempData["ToasterState"] = ToasterState.Error;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.EditTransactionFailed;
                }

                return RedirectToAction("Index");
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult Delete(Guid transactionGuid)
        {
            if (transactionGuid == null)
            {
                return BadRequest();
            }

            var transaction = context.Transaction
                .Where(x => x.TransactionGuid == transactionGuid)
                .SingleOrDefault();

            if (transaction == null)
            {
                return NotFound();
            }

            DeleteViewModel model = new DeleteViewModel()
            {
                Guid = transaction.TransactionGuid,
                Message = Messages.DeleteTransactionText
            };

            return PartialView(model);
        }

        [HttpPost]
        public IActionResult Delete(DeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var transaction = context.Transaction
                    .Where(x => x.TransactionGuid == model.Guid)
                    .SingleOrDefault();

                if (transaction == null)
                {
                    NotFound();
                }

                if (transaction.AccountGuid.HasValue && transaction.StateCodeGuid == Codes.PassedState)
                {
                    var account = context.Account
                        .Where(x => x.AccountGuid == transaction.AccountGuid)
                        .SingleOrDefault();

                    if (account == null)
                    {
                        TempData["ToasterState"] = ToasterState.Error;
                        TempData["ToasterType"] = ToasterType.Message;
                        TempData["ToasterMessage"] = Messages.CreateTransactionFailedAccountNotValid;

                        return RedirectToAction("Index");
                    }

                    account.Credit = transaction.TypeCodeGuid == Codes.CreditorType ? account.Credit - transaction.Cost : account.Credit + transaction.Cost;
                    account.ModifiedDate = DateTime.Now;

                    var transactionBefore = context.Transaction
                        .Where(x => !x.IsDelete && x.ReceiptDate < transaction.ReceiptDate)
                        .OrderByDescending(x => x.ReceiptDate)
                        .FirstOrDefault();

                    long transactionBeforeCredit = transactionBefore == null ? 0 : transactionBefore.Credit;

                    var transactionsAfter = context.Transaction
                        .Where(x => !x.IsDelete && x.ReceiptDate > transaction.ReceiptDate)
                        .OrderBy(x => x.ReceiptDate)
                        .ToList();

                    if (transactionsAfter.Count > 0)
                    {
                        transactionsAfter[0].Credit = transactionsAfter[0].StateCodeGuid == Codes.PassedState ?
                                (transactionsAfter[0].TypeCodeGuid == Codes.CreditorType ? transactionBeforeCredit + transactionsAfter[0].Cost : transactionBeforeCredit - transactionsAfter[0].Cost) :
                                (transactionsAfter[0].Credit = transactionBeforeCredit);

                        for (int i = 1; i < transactionsAfter.Count; i++)
                        {
                            transactionsAfter[i].Credit = transactionsAfter[i].StateCodeGuid == Codes.PassedState ?
                                (transactionsAfter[i].TypeCodeGuid == Codes.CreditorType ? transactionsAfter[i - 1].Credit + transactionsAfter[i].Cost : transactionsAfter[i - 1].Credit - transactionsAfter[i].Cost) :
                                (transactionsAfter[i].Credit = transactionsAfter[i - 1].Credit);
                        }
                    }
                }

                transaction.IsDelete = true;
                context.Transaction.Remove(transaction);

                if (Convert.ToBoolean(context.SaveChanges() > 0))
                {
                    TempData["ToasterState"] = ToasterState.Success;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.DeleteTransactionSuccessful;
                }
                else
                {
                    TempData["ToasterState"] = ToasterState.Error;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.DeleteTransactionFailed;
                }

                return RedirectToAction("Index");
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult ChangeState(Guid transactionGuid)
        {
            if (transactionGuid == null)
            {
                return BadRequest();
            }

            var transaction = context.Transaction
                .Where(x => x.TransactionGuid == transactionGuid)
                .SingleOrDefault();

            if (transaction == null)
            {
                return NotFound();
            }

            ChangeTransactionStateViewModel model = new ChangeTransactionStateViewModel()
            {
                Guid = transaction.TransactionGuid,
                StateGuid = transaction.StateCodeGuid
            };

            ViewBag.TransactionStates = new CodesRepository(context).GetCodesByCodeGroupGuid(CodeGroups.TransactionState);

            return PartialView(model);
        }

        [HttpPost]
        public IActionResult ChangeState(ChangeTransactionStateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var transaction = context.Transaction
                    .Where(x => x.TransactionGuid == model.Guid)
                    .SingleOrDefault();

                if (transaction == null)
                {
                    NotFound();
                }

                if (transaction.AccountGuid.HasValue)
                {
                    var account = context.Account
                        .Where(x => x.AccountGuid == transaction.AccountGuid)
                        .SingleOrDefault();

                    if (account == null)
                    {
                        TempData["ToasterState"] = ToasterState.Error;
                        TempData["ToasterType"] = ToasterType.Message;
                        TempData["ToasterMessage"] = Messages.CreateTransactionFailedAccountNotValid;

                        return RedirectToAction("Index");
                    }

                    if (transaction.StateCodeGuid != model.StateGuid)
                    {
                        if ((transaction.StateCodeGuid == Codes.WaitingState || transaction.StateCodeGuid == Codes.NotPassedState) && model.StateGuid == Codes.PassedState)
                        {
                            account.Credit = transaction.TypeCodeGuid == Codes.CreditorType ? account.Credit + transaction.Cost : account.Credit - transaction.Cost;
                            account.ModifiedDate = DateTime.Now;

                            var transactionBefore = context.Transaction
                                .Where(x => !x.IsDelete && x.ReceiptDate < transaction.ReceiptDate)
                                .OrderByDescending(x => x.ReceiptDate)
                                .FirstOrDefault();

                            long transactionBeforeCredit = transactionBefore == null ? 0 : transactionBefore.Credit;
                            transaction.Credit = transaction.TypeCodeGuid == Codes.CreditorType ? transactionBeforeCredit + transaction.Cost : transactionBeforeCredit - transaction.Cost;

                            var transactionsAfter = context.Transaction
                                .Where(x => !x.IsDelete && x.ReceiptDate > transaction.ReceiptDate)
                                .OrderBy(x => x.ReceiptDate)
                                .ToList();

                            if (transactionsAfter.Count > 0)
                            {
                                transactionsAfter[0].Credit = transactionsAfter[0].StateCodeGuid == Codes.PassedState ?
                                    (transactionsAfter[0].TypeCodeGuid == Codes.CreditorType ? transaction.Credit + transactionsAfter[0].Cost : transaction.Credit - transactionsAfter[0].Cost) :
                                    (transactionsAfter[0].Credit = transaction.Credit);

                                for (int i = 1; i < transactionsAfter.Count; i++)
                                {
                                    transactionsAfter[i].Credit = transactionsAfter[i].StateCodeGuid == Codes.PassedState ?
                                        (transactionsAfter[i].TypeCodeGuid == Codes.CreditorType ? transactionsAfter[i - 1].Credit + transactionsAfter[i].Cost : transactionsAfter[i - 1].Credit - transactionsAfter[i].Cost) :
                                        (transactionsAfter[i].Credit = transactionsAfter[i - 1].Credit);
                                }
                            }
                        }
                        else if (transaction.StateCodeGuid == Codes.PassedState)
                        {
                            account.Credit = transaction.TypeCodeGuid == Codes.CreditorType ? account.Credit - transaction.Cost : account.Credit + transaction.Cost;
                            account.ModifiedDate = DateTime.Now;

                            var transactionBefore = context.Transaction
                                .Where(x => !x.IsDelete && x.ReceiptDate < transaction.ReceiptDate)
                                .OrderByDescending(x => x.ReceiptDate)
                                .FirstOrDefault();

                            long transactionBeforeCredit = transactionBefore == null ? 0 : transactionBefore.Credit;
                            transaction.Credit = transactionBeforeCredit;

                            var transactionsAfter = context.Transaction
                                .Where(x => !x.IsDelete && x.ReceiptDate > transaction.ReceiptDate)
                                .OrderBy(x => x.ReceiptDate)
                                .ToList();

                            if (transactionsAfter.Count > 0)
                            {
                                transactionsAfter[0].Credit = transactionsAfter[0].StateCodeGuid == Codes.PassedState ?
                                    (transactionsAfter[0].TypeCodeGuid == Codes.CreditorType ? transaction.Credit + transactionsAfter[0].Cost : transaction.Credit - transactionsAfter[0].Cost) :
                                    (transactionsAfter[0].Credit = transaction.Credit);

                                for (int i = 1; i < transactionsAfter.Count; i++)
                                {
                                    transactionsAfter[i].Credit = transactionsAfter[i].StateCodeGuid == Codes.PassedState ?
                                        (transactionsAfter[i].TypeCodeGuid == Codes.CreditorType ? transactionsAfter[i - 1].Credit + transactionsAfter[i].Cost : transactionsAfter[i - 1].Credit - transactionsAfter[i].Cost) :
                                        (transactionsAfter[i].Credit = transactionsAfter[i - 1].Credit);
                                }
                            }
                        }

                        transaction.StateCodeGuid = model.StateGuid;
                    }
                }
                else
                {
                    transaction.StateCodeGuid = model.StateGuid;
                }
                
                transaction.ModifiedDate = DateTime.Now;

                if (Convert.ToBoolean(context.SaveChanges() > 0))
                {
                    TempData["ToasterState"] = ToasterState.Success;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.ChangeTransactionStateSuccessful;
                }
                else
                {
                    TempData["ToasterState"] = ToasterState.Error;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.ChangeTransactionStateFailed;
                }

                return RedirectToAction("Index");
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult ShowCheckInfo(Guid transactionGuid)
        {
            if (transactionGuid == null)
            {
                return BadRequest();
            }

            var checkTransaction = context.CheckTransaction
                .Where(x => x.TransactionGuid == transactionGuid)
                .SingleOrDefault();

            if (checkTransaction == null)
            {
                return NotFound();
            }

            CheckTransitionViewModel model = new CheckTransitionViewModel()
            {
                Serial = string.IsNullOrEmpty(checkTransaction.Serial) ? Messages.NotSet : checkTransaction.Serial,
                IssueDate = PersianDateExtensionMethods.ToPeString(checkTransaction.IssueDate, "yyyy/MM/dd")
            };

            return PartialView(model);
        }
    }
}