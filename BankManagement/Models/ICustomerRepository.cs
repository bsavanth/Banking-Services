using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankManagement.Models
{
    public interface ICustomerRepository
    {
        public Customer GetCustomerByID(int CID);
        public Customer GetCustomerBySSN(int SSN);
        public List<Customer> GetCustomer();
        public int Update(Customer customer);
        public int Delete(Customer customer);
        public int Add(Customer customer);
        public bool validSSN(int CID);
       



    }
}
