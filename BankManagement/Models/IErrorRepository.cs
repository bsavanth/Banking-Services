using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankManagement.Models
{
    public interface IErrorRepository
    {
        public int AddError(Error error);
        public List<Error> ViewAll();
    }
}
