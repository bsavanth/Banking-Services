using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BankManagement.Models
{
    public class Error
    {
        [Key]
        public int ErrorID { get; set; }

        [Display(Name = "Description")]
        [MaxLength(500)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Description is required Field")]
        public string Description { get; set;}

        [Display(Name = "Page Details")]
        [Required(ErrorMessage = "Page Details is a required Field")]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string PageDetail { get; set; }
    }
}
