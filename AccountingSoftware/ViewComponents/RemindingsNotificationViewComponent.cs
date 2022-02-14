using AccountingSoftware.Common;
using AccountingSoftware.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.ViewComponents
{
    public class RemindingsNotificationViewComponent : ViewComponent
    {
        private readonly AccountingSoftwareContext context;

        public RemindingsNotificationViewComponent(AccountingSoftwareContext context)
        {
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            var transactions = context.Transaction
                .Where(x => !x.IsDelete && x.ReceiptDate > DateTime.Now && x.ReceiptDate < DateTime.Now.AddDays(10) && x.StateCodeGuid == Codes.WaitingState)
                .ToList();

            return View(transactions.Count);
        }
    }
}
