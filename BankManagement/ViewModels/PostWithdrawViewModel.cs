using BankManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankManagement.ViewModels
{
    public class PostWithdrawViewModel
    {
        public Account outt { get; set; }

        public decimal moneyToout { get; set; }

        public int SID { get; set; }
    }
}
