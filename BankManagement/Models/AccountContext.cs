using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BankManagement.Models
{
    public class AccountContext:DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> db) : base(db)
        { }

        public DbSet<Account> Account { get; set; }
    }
}
