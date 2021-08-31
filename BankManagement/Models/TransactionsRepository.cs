using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankManagement.Models
{
    public class TransactionsRepository:ITransactionsRepository
    {
        CustomerContext customerContext;
        ErrorContext errorContext;
        AccountContext accountContext;
        TransactionsContext transactionsContext;


        public TransactionsRepository(CustomerContext customercontext, ErrorContext errorcontext, AccountContext accountcontext, TransactionsContext transactionscontext)
        {
            this.customerContext = customercontext;
            this.errorContext = errorcontext;
            this.accountContext = accountcontext;
            this.transactionsContext = transactionscontext;
        }

        public int Add(Transactions transaction)
        {
            try
            {
                transactionsContext.Add(transaction);
                transactionsContext.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public List<Transactions> GetTransByN(int n, int id)
        {
            List<Transactions> retList = transactionsContext.Transactions.Where(t => t.AID == id).ToList();
            retList.Reverse();

            if (n > retList.Count)
            {
                return retList;
            }
            else
            {
                return retList.GetRange(0, n);
            }
        }

        public List<Transactions> GetTransByDate(DateTime start, DateTime end)
        {
            List<Transactions> retList = transactionsContext.Transactions.Where(t => t.Date.Date >= start.Date && t.Date.Date <= end.Date).ToList();
            retList.Reverse();
            return retList;
        }


        public List<Transactions> GetTransBoth(int n, int id, DateTime start, DateTime end)
        {
            List<Transactions> retList = transactionsContext.Transactions.Where(t => t.Date.Date >= start.Date && t.Date.Date <= end.Date).ToList();
            retList.Reverse();

            if (n > retList.Count)
            {
                return retList;
            }
            else
            {
                return retList.GetRange(0, n);
            }
        }

    }
}
