using BankManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankManagement.ViewModels
{
    public class AccountDetailsViewModel
    {
        public Account account { get; set; }
        public Customer customer { get; set; }
    }
}
