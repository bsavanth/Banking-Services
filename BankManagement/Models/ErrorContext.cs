using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BankManagement.Models
{
    public class ErrorContext:DbContext
    {
        public ErrorContext(DbContextOptions<ErrorContext> db):base(db)
        { }

        public DbSet<Error> Error { get; set; }
    }
}
