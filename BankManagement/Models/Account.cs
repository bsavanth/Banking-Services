using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BankManagement.Models
{
	public class Account
	{
		[Key]
		[Display(Name = "Account ID")]
		public int AID { get; set; }


		[Display(Name = "Customer ID")]
		[Required(ErrorMessage = "Customer ID is Required")]
		public int CID { get; set; }



		[Display(Name = "Account Type")]
		[Required(ErrorMessage = "Account is checkin/savings")]
		[AccountTypeValidation(ErrorMessage = "Account Type can only be either checkings or savings")]
		[DataType(DataType.Text)]
		[MaxLength(30, ErrorMessage = "AccountType cannot be more than 30 characters")]
		public string AccountType { get; set; }



		[Display(Name = "Balance")]
		[Required(ErrorMessage = "Starting balance is required")]
		public decimal Balance { get; set; }


		[Display(Name = "Last Updated")]
		public string LastUpdated { get; set; }

	}

		public sealed class AccountTypeValidationAttribute : ValidationAttribute
		{
			public override bool IsValid(object value)
			{
				string Type = Convert.ToString(value);
				bool result = Type == "Savings" || Type == "Checking";
				return result;
			}
		}
	
}

