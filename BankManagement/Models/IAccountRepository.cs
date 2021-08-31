using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankManagement.Models
{
   public interface IAccountRepository
    {
        public List<Account> ViewAll();

        public List<Account> GetAccountsByCID(int CID);
     
        public Account GetAccountByID(int AID);

        public bool DuplicateAccount(int CID, string AccountType);
        public int Add(Account account);
        public int Update(Account account);
        public int Delete(Account account);
        public List<Account> GetAccountsByCustomer(Customer customer);
        public List<Account> GetAccountsByCustomer(int CID, int SSN);
        public Account GetAccountsByCustomer(int iD);

        public decimal Deposit(Account account);


    }
}
