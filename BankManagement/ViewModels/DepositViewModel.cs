using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BankManagement.Models;

namespace BankManagement.ViewModels
{
    public class DepositViewModel
    {
        public Account Account { get; set; }
        public int amount { get; set; }
    }
}
