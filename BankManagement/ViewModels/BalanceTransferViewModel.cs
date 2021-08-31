using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BankManagement.Models
{
    public class BalanceTransferViewModel
    {
        [Display(Name = "Receiever Account ID")]
        [Required(ErrorMessage ="Receiver Account ID is required")]
        public int receiverAccountID{get; set;}

        [Display(Name = "Sender Account ID")]
        [Required(ErrorMessage = "Sender Account ID is required")]
        public int senderAccountID {get; set;}

        
        [Display(Name = "Amount to Send")]
        [Required(ErrorMessage = "Amount to Send is required")]
        public decimal moneyToSend { get; set; }
        
        public BalanceTransferViewModel()
        {
            this.receiverAccountID = 0;
            this.senderAccountID = 0;
            this.moneyToSend = 0;
        }

    }


    
}
