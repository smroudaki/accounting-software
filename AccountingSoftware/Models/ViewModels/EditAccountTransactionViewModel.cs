using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Models.ViewModels
{
    public class EditAccountTransactionViewModel : CreateAccountTransactionViewModel
    {
        public Guid TransactionGuid { get; set; }
    }
}
