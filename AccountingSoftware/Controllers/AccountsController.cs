using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingSoftware.Common;
using AccountingSoftware.Models;
using AccountingSoftware.Models.Entities;
using AccountingSoftware.Models.Enums;
using AccountingSoftware.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSoftware.Controllers
{
    public class AccountsController : Controller
    {
        private readonly AccountingSoftwareContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountsController(AccountingSoftwareContext context,
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var accounts = context.Account
                .Where(x => !x.IsDelete)
                .OrderByDescending(x => x.ModifiedDate)
                .Select(x => new AccountViewModel
                {
                    AccountGuid = x.AccountGuid,
                    UserFullName = x.User.FirstName + " " + x.User.LastName,
                    BankName = string.IsNullOrEmpty(x.BankName) ? Messages.NotSet : x.BankName,
                    AccountName = string.IsNullOrEmpty(x.AccountName) ? Messages.NotSet : x.AccountName,
                    AccountNumber = x.AccountNumber,
                    CardNumber = string.IsNullOrEmpty(x.CardNumber) ? Messages.NotSet : x.CardNumber,
                    Credit = x.Credit,
                    ModifiedDate = PersianDateExtensionMethods.ToPeString(x.ModifiedDate, "yyyy/MM/dd HH:mm")

                }).ToList();

            return View(accounts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult Create(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = new Account
                {
                    UserGuid = userManager.GetUserId(User),
                    BankName = model.BankName,
                    AccountName = model.AccountName,
                    AccountNumber = model.AccountNumber,
                    CardNumber = model.CardNumber,
                    Credit = Convert.ToInt64(model.Credit)
                };

                context.Account.Add(account);

                if (Convert.ToBoolean(context.SaveChanges()))
                {
                    TempData["ToasterState"] = ToasterState.Success;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.CreateAccountSuccessful;
                }
                else
                {
                    TempData["ToasterState"] = ToasterState.Error;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.CreateAccountFailed;
                }

                return RedirectToAction("Index");
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult Edit(Guid accountGuid)
        {
            if (accountGuid == null)
            {
                return BadRequest();
            }

            var account = context.Account
                .Where(x => x.AccountGuid == accountGuid)
                .SingleOrDefault();

            if (account == null)
            {
                return NotFound();
            }

            EditAccountViewModel model = new EditAccountViewModel()
            {
                AccountGuid = account.AccountGuid,
                BankName = account.BankName,
                AccountName = account.AccountName,
                AccountNumber = account.AccountNumber,
                CardNumber = account.CardNumber,
                Credit = account.Credit.ToString()
            };

            return PartialView(model);
        }

        [HttpPost]
        public IActionResult Edit(EditAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = context.Account
                    .Where(x => x.AccountGuid == model.AccountGuid)
                    .SingleOrDefault();

                if (account == null)
                {
                    NotFound();
                }

                account.BankName = model.BankName;
                account.AccountName = model.AccountName;
                account.AccountNumber = model.AccountNumber;
                account.CardNumber = model.CardNumber;
                account.Credit = Convert.ToInt64(model.Credit);
                account.ModifiedDate = DateTime.Now;

                if (Convert.ToBoolean(context.SaveChanges() > 0))
                {
                    TempData["ToasterState"] = ToasterState.Success;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.EditAccountSuccessful;
                }
                else
                {
                    TempData["ToasterState"] = ToasterState.Error;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.EditAccountFailed;
                }

                return RedirectToAction("Index");
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult Delete(Guid accountGuid)
        {
            if (accountGuid == null)
            {
                return BadRequest();
            }

            var account = context.Account
                .Where(x => x.AccountGuid == accountGuid)
                .SingleOrDefault();

            if (account == null)
            {
                return NotFound();
            }

            DeleteViewModel model = new DeleteViewModel()
            {
                Guid = account.AccountGuid,
                Message = Messages.DeleteAccountText
            };

            return PartialView(model);
        }

        [HttpPost]
        public IActionResult Delete(DeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = context.Account
                    .Where(x => x.AccountGuid == model.Guid)
                    .SingleOrDefault();

                if (account == null)
                {
                    NotFound();
                }

                account.IsDelete = true;

                if (Convert.ToBoolean(context.SaveChanges() > 0))
                {
                    TempData["ToasterState"] = ToasterState.Success;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.DeleteAccountSuccessful;
                }
                else
                {
                    TempData["ToasterState"] = ToasterState.Error;
                    TempData["ToasterType"] = ToasterType.Message;
                    TempData["ToasterMessage"] = Messages.DeleteAccountFailed;
                }

                return RedirectToAction("Index");
            }

            return BadRequest();
        }
    }
}