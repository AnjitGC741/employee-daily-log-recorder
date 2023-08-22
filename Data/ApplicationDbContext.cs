using employeeDailyTaskRecorder.Models;
using Microsoft.EntityFrameworkCore;
namespace employeeDailyTaskRecorder.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Record> Records { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasMany(a => a.Records)
                .WithOne(b => b.Employee)
                .HasForeignKey(b => b.EmployeeId);

            modelBuilder.Entity<Employee>().HasData(
               new Employee { Id = 1, Name = "Purna", Email = "admin@gmail.com", Address = "Kathmandu", Password = "admin", EmpType = EnumEmployeeType.Admin }
               );
        }
      
    }
}
