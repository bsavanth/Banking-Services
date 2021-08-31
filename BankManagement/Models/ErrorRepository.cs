using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankManagement.Models
{
    public class ErrorRepository:IErrorRepository
    {
        CustomerContext customerContext;
        ErrorContext errorContext;
        AccountContext accountContext;
        TransactionsContext transactionsContext;


        public ErrorRepository(CustomerContext customercontext, ErrorContext errorcontext, AccountContext accountcontext, TransactionsContext transactionscontext)
        {
            this.customerContext = customercontext;
            this.errorContext = errorcontext;
            this.accountContext = accountcontext;
            this.transactionsContext = transactionscontext;
        }

        public int AddError(Error error)
        {
            try
            {
                errorContext.Error.Add(error);
                errorContext.SaveChanges();
                return 1;
            }

            catch 
            {
                return -1;
            }
        }
        public List<Error> ViewAll()
        {
            try
            {
                return errorContext.Error.ToList();
            }
            catch
            {
                return null;
            }
        }



    }
}
