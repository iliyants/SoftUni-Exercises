using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class MyAppContext : DbContext
    {
        public MyAppContext(DbContextOptions<MyAppContext> options)
            : base(options)
        {
        }

        protected MyAppContext()
        {
        }

        public DbSet<Employee> Employees { get; set; }

    }
}
