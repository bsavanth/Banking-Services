using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace BankManagement.Models
{
    public class CustomerRepository : ICustomerRepository
    {
        CustomerContext customerContext;
        ErrorContext errorContext;
        AccountContext accountContext;
        TransactionsContext transactionsContext;


        public CustomerRepository(CustomerContext customercontext, ErrorContext errorcontext, AccountContext accountcontext, TransactionsContext transactionscontext)
        {
            this.customerContext = customercontext;
            this.errorContext = errorcontext;
            this.accountContext = accountcontext;
            this.transactionsContext = transactionscontext;
        }
       
        public List<Customer> GetCustomer()
        {
            try
            {
                return customerContext.Customer.ToList();
            }
            catch
            {

                return null;
            }
        }

        public Customer GetCustomerByID(int CID)
        {

            try
            {
               return customerContext.Customer.Where(customer => customer.CID == CID).FirstOrDefault();
            }

            catch
            {
                return null;
            }
        }

        public Customer GetCustomerBySSN(int SSN)
        {

            try
            {
                return customerContext.Customer.Where(customer => customer.SSN == SSN).FirstOrDefault();
            }

            catch
            {
                return null;
            }
        }

        public int Update(Customer customer)
        {
            try
            {
                customerContext.Entry(customer).State = EntityState.Modified;
                customerContext.SaveChanges();
                return 1;
            }
            
            catch
            {
                return 0;
            }
        }


        public int Delete(Customer customer)
        {
            try
            {
                customerContext.Customer.Remove(customer);
                customerContext.SaveChanges();
                return 1;
            }
            catch 
            {
                return 0;
            }

        }

        public int Add(Customer customer)
        {
            try
            {
                customerContext.Customer.Add(customer);
                customerContext.SaveChanges();
                return 1;
            }
            catch 
            {
                return 0;
            }
        }

        public bool validSSN(int CID)
        {
            if (customerContext.Customer.Find(CID) == null)
                return true;
            else
            return false;
        }


    }
}
