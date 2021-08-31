using BankManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankManagement.ViewModels
{
    public class TransStatementViewModel
    {
        public StatementFormModel formModel { get; set; }
        public List<Account> accountList { get; set; }
        public List<Transactions> transList { get; set; }
    }
}
