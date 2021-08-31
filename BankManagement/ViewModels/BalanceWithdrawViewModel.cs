using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankManagement.ViewModels
{
    public class BalanceWithdrawViewModel
    {

        [Display(Name = " Account ID")]
        [Required(ErrorMessage = "Account ID is required")]
        public int outAccountID { get; set; }

        [Display(Name = "Amount to Send")]
        [Required(ErrorMessage = "Amount to Withdraw is required")]
        public decimal moneyToout { get; set; }

        public BalanceWithdrawViewModel()
        {

            this.outAccountID = 0;
            this.moneyToout = 0;
        }
    }
}
