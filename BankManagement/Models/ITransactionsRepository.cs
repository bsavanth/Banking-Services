using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankManagement.Models
{
    public interface ITransactionsRepository
    {
        public int Add(Transactions transaction);
        public List<Transactions> GetTransByN(int n, int id);
        public List<Transactions> GetTransByDate(DateTime start, DateTime end);
        public List<Transactions> GetTransBoth(int n, int id, DateTime start, DateTime end);
    }
}
