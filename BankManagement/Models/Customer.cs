using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BankManagement.Models
{
    public class Customer
    {
		[Key]
		[Display(Name = "Customer ID")]
		public int CID { get; set; }

		[Display(Name = "SSN")]
		[Required(ErrorMessage = "SSN is required Field")]
		[Range(100000000, 999999999,ErrorMessage = "SSN has to be a 9 digit number")]
		public int SSN { get; set; }

		[Display(Name = "Name")]
		[MaxLength(30, ErrorMessage = "Name cannot be more than 30 characters")]
		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Name is required Field")]
		public string Name { get; set; }

		
		[Display(Name = "Age")]
		[Required(ErrorMessage = "Age is required Field")]
		[Range(1, 130, ErrorMessage = "Age cannt be zero or negative?")]
		public int Age { get; set; }

		[Display(Name = "Address1")]
		[DataType(DataType.Text)]
		[MaxLength(300, ErrorMessage = "Address1 cannot be more than 300 characters")]
		[Required(ErrorMessage = "Address1  is required Field")]
		public string Address1 { get; set; }

		[Display(Name = "Address2")]
		[MaxLength(300, ErrorMessage ="Address2 cannot be more than 300 characters")]
		[DataType(DataType.Text)]
		
		public string Address2 { get; set; }

		[Display(Name = "City")]
		[MaxLength(20, ErrorMessage = "City cannot be more than 20 characters")]
		[DataType(DataType.Text)]
		[Required(ErrorMessage = "City is required Field")]
		public string City { get; set; }

		[Display(Name = "State")]
		[MaxLength(20, ErrorMessage = "State cannot be more than 20 characters")]
		[DataType(DataType.Text)]
		[Required(ErrorMessage = "State is required Field")]
		public string State { get; set; }

		[Display(Name = "Last Updated")]
		public string LastUpdated { get; set; }
	

	}
}
