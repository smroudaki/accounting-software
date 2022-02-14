using AccountingSoftware.Common;
using AccountingSoftware.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Models.Repositories
{
    public class DashboardRepository
    {
        private readonly AccountingSoftwareContext context;

        public DashboardRepository(AccountingSoftwareContext context)
        {
            this.context = context;
        }

        public int GetAccountsCount(string userGuid)
        {
            var accounts = context.Account
                .Where(x => x.UserGuid == userGuid && !x.IsDelete)
                .ToList();

            return accounts.Count;
        }

        public int GetTransactionsCount()
        {
            var transactions = context.Transaction
                .Where(x => !x.IsDelete && x.StateCodeGuid == Codes.PassedState)
                .ToList();

            return transactions.Count;
        }

        public long GetAccountsCredit(string userGuid)
        {
            var accounts = context.Account
                .Where(x => x.UserGuid == userGuid && !x.IsDelete)
                .Sum(x => x.Credit);

            return accounts;
        }

        public IEnumerable<AccountsAbstractViewModel> GetAccountsAbstract(string userGuid)
        {
            var accounts = context.Account
                .Where(x => x.UserGuid == userGuid && !x.IsDelete)
                .OrderByDescending(x => x.CreationDate)
                .Select(x => new AccountsAbstractViewModel 
                {
                    AccountNumber = x.AccountNumber,
                    Credit = x.Credit

                }).ToList().Take(9);

            return accounts;
        }
    }
}
