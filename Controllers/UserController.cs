using Microsoft.AspNetCore.Mvc;
using employeeDailyTaskRecorder.Data;
using employeeDailyTaskRecorder.Models;
using employeeDailyTaskRecorder.HelperService;

namespace employeeDailyTaskRecorder.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db)
        {

            _db = db;
        }
        public IActionResult Index()
        {
            VMAdminIndex Result = new VMAdminIndex();
            Result.EmployeeList = _db.Employees.ToList();
            return View(Result);
        }
        public IActionResult addUser(Employee employee)
        {
            Employee value = new Employee();
            value.Name = employee.Name;
            value.Address = employee.Address;
            value.Email = employee.Email;
            value.Password = employee.Password;
            value.EmpType = employee.EmpType; 
            _db.Employees.Add(value);
            _db.SaveChanges();
            return RedirectToAction("Index", "User");
        }

        public IActionResult deleteUser(int employeeId)
        {
            Employee data = _db.Employees.Find(employeeId);
            _db.Employees.Remove(data);
            _db.SaveChanges();
            return RedirectToAction("Index", "User");
        }
        public IActionResult EmployeeEditProfile()
        {
            Employee empData = SessionService.GetSession(HttpContext);
            var employee = _db.Employees.Where(x => x.Id == empData.Id).ToList();
            return View(employee);
        }
        public IActionResult editEmployee(Employee employee,String oldPassword,String confirmPassword,String newPassword)
        {
            Employee value = _db.Employees.Find(employee.Id);
            if (employee.Password == null)
            {
                value.Name = employee.Name;
                value.Address = employee.Address;
                value.Email = employee.Email;
            }
            else
            {
                if(employee.Password == null || oldPassword == null || newPassword == null)
                {
                    TempData["PasswordErrorMessage"] = "Empty Fields";
                    return RedirectToAction("EmployeeEditProfile", "User");

                }
                else if(employee.Password != oldPassword)
                {
                    TempData["PasswordErrorMessage"] = "Old password doesn't match";
                    return RedirectToAction("EmployeeEditProfile", "User");

                }
                else if(confirmPassword != newPassword)
                {
                    TempData["PasswordErrorMessage"] = "New password and confirm password must match";
                    return RedirectToAction("EmployeeEditProfile", "User");


                }
                else
                {
                value.Password = confirmPassword;

                }
            }
            _db.SaveChanges();
            return RedirectToAction ("EmployeeEditProfile", "User");
        }
    }
}
