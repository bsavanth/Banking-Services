using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankManagement.Models
{
    public class Transactions
    {
		[Key]
		[Display(Name = "Transactions ID")]
		public int TID { get; set; }

		[Display(Name = "Account ID")]
		
		public int AID { get; set; }


		[Display(Name = "Transaction Type")]
		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Transaction Type is a required Field")]
		[MaxLength(20)]
		public string TransactionType { get; set; }


		[Display(Name = "Transaction Amount")]
		[Required(ErrorMessage = "Transaction Type is a required Field")]
		public decimal TransactionAmount { get; set; }


		[Display(Name = "Description")]
		[DataType(DataType.Date)]
		[Required(ErrorMessage = "Date is a required Field")]
		public DateTime Date { get; set; }


		[Display(Name = "Description")]
		[Required(ErrorMessage = "Description is a required Field")]
		[MaxLength(200)]
		[DataType(DataType.Text)]
		public string Description { get; set; }


	}
}
