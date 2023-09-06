using Microsoft.AspNetCore.Mvc;
using employeeDailyTaskRecorder.Data;
using employeeDailyTaskRecorder.Models;
using employeeDailyTaskRecorder.HelperService;
using employeeDailyTaskRecorder.CustomAttributes;
using employeeDailyTaskRecorder.Hash;

namespace employeeDailyTaskRecorder.Controllers
{
     [GeneralAuthorization]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _db = db;
        }
        public IActionResult Index()
        {
            VMAdminIndex Result = new VMAdminIndex();
            Result.EmployeeList = _db.Employees.Where(x => x.IsDeleted == false).ToList();
            return View(Result);
        }
        [AdminAuthorization]
        public IActionResult UserDetails(int id)
        {
            VMAdminIndex Result = new VMAdminIndex();
            Result.TaskList = _db.Records.Where(x => x.EmployeeId == id).ToList();
            Result.EmployeeList = _db.Employees.Where(x => x.Id == id).ToList();
            return View(Result);
        }
        public IActionResult addUser(Employee employee)
        {
            var emailList = _db.Employees.Where(x => x.Email == employee.Email).FirstOrDefault();
            if (emailList != null)
            {
                TempData["emailPresence"] = true;
            }
            else
            {
                Employee value = new Employee();
                value.Name = employee.Name;
                value.Address = employee.Address;
                value.Email = employee.Email;
                value.Password = new Password(employee.Password).HashPassword();
                value.EmpType = employee.EmpType;
                value.ProfileImg = "";
                _db.Employees.Add(value);
                _db.SaveChanges();
                TempData["emailPresence"] = false;

            }
            return RedirectToAction("Index", "User");
        }
        public IActionResult editUser(Employee employee)
        {
            Employee value = _db.Employees.Find(employee.Id);
            if (value == null) { throw new Exception("Cannot Find Related data in db"); }
            value.Name = employee.Name;
            value.Address = employee.Address;
            value.Email = employee.Email;
            value.Password = employee.Password;
            value.EmpType = employee.EmpType;
            _db.SaveChanges();
            return RedirectToAction("Index", "User");
        }

        public IActionResult deleteUser(int employeeId)
        {
            Employee data = _db.Employees.Find(employeeId);
            data.IsDeleted = true;
            /* _db.Employees.Remove(data);*/
            _db.SaveChanges();
            return RedirectToAction("Index", "User");
        }
        public IActionResult EmployeeEditProfile(int id)
        {
            Employee empData = SessionService.GetSession(HttpContext);
            VMAdminIndex Result = new VMAdminIndex();
            Result.EmployeeID = empData.Id;
            Result.TaskList = _db.Records.Where(x => x.EmployeeId == id).ToList();
            Result.EmployeeList = _db.Employees.Where(x => x.Id == id).ToList();
            return View(Result);

        }
        public IActionResult editProfileImg(int Id, IFormFile? profileImg)
        {
            Employee empData = SessionService.GetSession(HttpContext);
            Employee data = _db.Employees.Find(Id);
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (profileImg != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(profileImg.FileName);
                string productPath = Path.Combine(wwwRootPath, @"Img\Upload");
                if (!string.IsNullOrEmpty(data.ProfileImg))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, data.ProfileImg.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    profileImg.CopyTo(fileStream);
                }
                data.ProfileImg = @"/Img/Upload/" + fileName;
            }
            _db.SaveChanges();
            return RedirectToAction("EmployeeEditProfile", "User", new { id = Id });
        }
        public IActionResult deleteProfileImg(int Id)
        {
            Employee data = _db.Employees.Find(Id);
            data.ProfileImg = "";
            _db.SaveChanges();
            return RedirectToAction("EmployeeEditProfile", "User", new { id = Id });
        }
        public IActionResult editEmployee(Employee employee,string? changePassword, String? oldPassword, String? confirmPassword, String? newPassword)
        {
            bool flag = true;
            Employee value = _db.Employees.Find(employee.Id);
               
            if (changePassword == null)
            {
                value.Name = employee.Name;
                value.Address = employee.Address;
                value.Email = employee.Email;
                if (employee.Password != null)
                {
                    if (employee.Password.Length < 5)
                    {
                        TempData["type"] = "danger";
                        TempData["editSuccessfulMessage"] = "Password must be atleast 5 character";
                        return RedirectToAction("EmployeeEditProfile", "User", new { id = value.Id });
                    }

                    value.Password = new Password(employee.Password).HashPassword();
                }
                TempData["type"] = "success";
                TempData["editSuccessfulMessage"] = "Profile Update Successful";
            }
            else
            {
                string oldPasswordValueHash = new Password(oldPassword).HashPassword();
              
                if (confirmPassword == "" || oldPassword == "" || newPassword == "")
                {
                    TempData["PasswordErrorMessage"] = "Empty Fields";
                    flag = false;

                }
                else if (confirmPassword != newPassword)
                {
                    TempData["PasswordErrorMessage"] = "New password and confirm password must match";
                    flag = false;

                }
                else if (newPassword.Length < 5)
                {
                    TempData["PasswordErrorMessage"] = "Password must be atleast 5 character";
                    flag = false;
                }
                else if (employee.Password != oldPasswordValueHash)
                {
                    TempData["PasswordErrorMessage"] = "Old password doesn't match";
                    flag = false;

                }
              
                else
                {
                    value.Password =new Password(confirmPassword).HashPassword(); ;
                    TempData["PasswordErrorMessage"] = "Password Reset Successful";
                }
            }
            if (!flag)
            {
                TempData["type"] = "danger";
                return RedirectToAction("EmployeeEditProfile", "User", new { id = value.Id });
            }
            value.UpdatedAt = DateTime.Now;
            _db.SaveChanges();
            TempData["type"] = "success";
            return RedirectToAction("EmployeeEditProfile", "User", new { id = value.Id });
        }
        public IActionResult redirectPage()
        {
            Employee empData = SessionService.GetSession(HttpContext);
            if (empData.IsAdmin)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("EmployeeTask", "UserTask");
        }
     
    }
}
