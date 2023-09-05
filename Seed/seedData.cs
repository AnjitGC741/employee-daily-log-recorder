using Microsoft.EntityFrameworkCore;
using employeeDailyTaskRecorder.Data;
using employeeDailyTaskRecorder.Models;

namespace employeeDailyTaskRecorder.Seed
{
    public class seedData
    {
        public static void Initialize(ApplicationDbContext _db)
        {
            try
            {

                _db.Database.Migrate();

                if (!_db.Employees.Any())
                {
                    _db.Employees.AddRange(
                        new Employee { Name = "Purna", Email = "admin@gmail.com", Address = "Kathmandu", Password = "admin", EmpType = EnumEmployeeType.Admin}
                    );

                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
