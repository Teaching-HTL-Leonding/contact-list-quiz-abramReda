using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Address_Book.Modles;

namespace Address_Book.Data
{
    public class ApplicationContext:DbContext
    {

        public ApplicationContext( DbContextOptions options) : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
    }
}
