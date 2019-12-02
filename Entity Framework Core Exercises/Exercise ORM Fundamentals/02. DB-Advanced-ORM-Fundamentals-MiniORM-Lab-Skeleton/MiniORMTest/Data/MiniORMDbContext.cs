namespace MiniORMTest.Data
{
    using MiniORM;
    using MiniORMTest.Data.Entities;

    public class MiniORMDbContext : Dbcontext
    {
        public MiniORMDbContext(string connectionString)
            : base(connectionString) 
        {

        }

        public DbSet<Employee> Employees { get; }
        public DbSet<Project> Projects { get; }
        public DbSet<Department> Departments { get; }
        public DbSet<EmployeeProject> EmployeesProjects { get; }




    }
}
