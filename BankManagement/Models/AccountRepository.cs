using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BankManagement.Models
{
    public class AccountRepository : IAccountRepository
    {
        CustomerContext customerContext;
        ErrorContext errorContext;
        AccountContext accountContext;
        TransactionsContext transactionsContext;


        public AccountRepository(CustomerContext customercontext, ErrorContext errorcontext, AccountContext accountcontext, TransactionsContext transactionscontext)
        {
            this.customerContext = customercontext;
            this.errorContext = errorcontext;
            this.accountContext = accountcontext;
            this.transactionsContext = transactionscontext;
        }

        public List<Account> ViewAll()
        {
            return accountContext.Account.ToList();
        }


        public Account GetAccountByID(int AID)
        {
            try
            {
                return accountContext.Account.Where(account => account.AID == AID).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public Account GetAccountsByID(int CID)
        {
            try
            {
                return accountContext.Account.Where(account => account.CID == CID).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }


        public List<Account> GetAccountsByCID(int CID)
        {
            try
            {
                return accountContext.Account.Where(account => account.CID == CID).ToList();
            }

            catch
            {
                return null;
            }
        }



        public bool DuplicateAccount(int CID, string AccountType)
        {
            try
            {
                Account account = accountContext.Account.Where(entry => entry.CID == CID && entry.AccountType==AccountType).FirstOrDefault();
                if(account == null)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return true;
            }
        }



        public int Add(Account account)
        {
            try
            {
                accountContext.Account.Add(account);
                accountContext.SaveChanges();
                return 1;
            }

            catch
            {
                return 0;
            }
        }

        public int Update(Account account)
        {
            try
            {
                accountContext.Entry(account).State = EntityState.Modified;
                accountContext.SaveChanges();
                return 1;
            }

            catch
            {
                return 0;
            }
        }



        public int Delete(Account account)
        {
            try
            {
                accountContext.Account.Remove(account);
                accountContext.SaveChanges();
                return 1;
            }

            catch
            {
                return 0;
            }
        }

        public List<Account> GetAccountsByCustomer(Customer customer)
        {
            try
            {

                var account = accountContext.Account.Where(o => o.CID == customer.CID).ToList();
                return account;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public List<Account> GetAccountsByCustomer(int CID, int SSN)
        {
            throw new NotImplementedException();
        }

        public Account GetAccountsByCustomer(int iD)
        {
            throw new NotImplementedException();
        }

        public decimal Deposit(Account account)
        {
            accountContext.Entry(account).State = EntityState.Modified;
            accountContext.SaveChanges();
            return account.Balance;
        }

    }

}
