using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankManagement.Models;

namespace BankManagement.ViewModels
{
    public class PostTransferViewModel
    {
        public Account sender { get; set; }
        public Account receiver { get; set; }
        public decimal moneyToSend {get;set;}

        public int SID { get; set; }

        public int RID { get; set; }


    }
}
