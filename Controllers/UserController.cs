using Microsoft.AspNetCore.Mvc;
using employeeDailyTaskRecorder.Data;
using employeeDailyTaskRecorder.Models;
using employeeDailyTaskRecorder.HelperService;
using employeeDailyTaskRecorder.CustomAttributes;
using employeeDailyTaskRecorder.Hash;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

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
        public IActionResult EmployeeCompletionStatus(int? days)
        {
            ViewBag.CompletionDays = days??15;
            VMAdminIndex Result = new VMAdminIndex();
            Result.TotalEmployee = _db.Employees.Count();
            Result.TotalIntern = _db.Employees.Where(x => x.EmpStage == EnumEmployeeStage.Internship).Count();
            Result.TotalProvisionPeriod = _db.Employees.Where(x => x.EmpStage == EnumEmployeeStage.Probation_Period).Count();
            Result.TotalContractual = _db.Employees.Where(x => x.EmpStage == EnumEmployeeStage.Contractual).Count();
            Result.TotalAdmin = _db.Employees.Where(x => x.EmpRole == EnumMajorRole.Admin).Count();
            Result.TotalDeveloper = _db.Employees.Where(x => x.EmpRole == EnumMajorRole.Developer).Count();
            Result.TotalQA = _db.Employees.Where(x => x.EmpRole == EnumMajorRole.CustomerSupport_QA).Count();
            Result.TotalMale = _db.Employees.Where(x => x.Gender == EnumEmployeeGender.Male).Count(); 
            Result.TotalFemale = _db.Employees.Where(x => x.Gender == EnumEmployeeGender.Female).Count();
            Result.EmployeeList = _db.Employees.Where(x => x.CurrentStageCompletionDate.Date >= DateTime.Now && x.CurrentStageCompletionDate <= DateTime.Now.AddDays(days??15)).ToList();
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
                value.ContactNumber = employee.ContactNumber;
                value.Gender = employee.Gender;
                value.EmpRole = employee.EmpRole;
                value.EmpStage = employee.EmpStage;
                value.JoinDate = employee.JoinDate;
                value.CurrentStageCompletionDate = employee.CurrentStageCompletionDate;
                value.Email = employee.Email;
                value.Password = PwdEncryption.HashPassword(employee.Password);
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
            _db.SaveChanges();
            return RedirectToAction("Index", "User");
        }
        public IActionResult EmployeeEditProfile(int id)
        {
            Employee empData = SessionService.GetSession(HttpContext);
            VMValidatePassword UserData = new VMValidatePassword();
            UserData.EmployeeID = empData.Id;
            UserData.EmployeeList = _db.Employees.Where(x => x.Id == id).ToList();
            return View(UserData);
        }
        public IActionResult editProfileImg(int Id, IFormFile? profileImg)
        {
            //Employee empData = SessionService.GetSession(HttpContext);
            Employee data = _db.Employees.Find(Id);
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (profileImg != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(profileImg.FileName);
                string productPath = Path.Combine(wwwRootPath, @"Img\Upload");
                string tempFilePath = Path.Combine(productPath, "temp_" + fileName);
                using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
                {
                    profileImg.CopyTo(fileStream);
                }
                using (var imageStream = new FileStream(tempFilePath, FileMode.Open))
                {
                    using (var image = Image.Load(imageStream))
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(1024, 600),
                            Mode = ResizeMode.Max
                        }));
                        image.Save(Path.Combine(productPath, fileName), new JpegEncoder
                        {
                            Quality = 70
                        });
                    }
                }

                System.IO.File.Delete(tempFilePath);
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
        public IActionResult EditEmployeePassword(VMValidatePassword UserData)
        {
            bool flag = true;
            if (ModelState.IsValid)
            {
                Employee value = _db.Employees.Find(UserData.EmployeeID);
                if (value.Password != PwdEncryption.HashPassword(UserData.OldPassword))
                {
                    TempData["EditPasswordMessage"] = "Old password doesn't match";
                    flag = false;
                }
                else if (UserData.ConfirmPassword != UserData.NewPassword)
                {

                    TempData["EditPasswordMessage"] = "New password and confirm password doesn't match";
                    flag = false;
                }
                else
                {
                    value.Password = PwdEncryption.HashPassword(UserData.ConfirmPassword);
                    _db.SaveChanges();
                    TempData["EditPasswordMessage"] = "Password changed successfully";
                    TempData["type"] = "success";
                }
            }
            if (!flag)
            {
                TempData["type"] = "danger";
            }
            return RedirectToAction("EmployeeEditProfile", "User", new { id = UserData.EmployeeID });
        }
        public IActionResult EditEmployeeData(Employee employee)
        {
            Employee value = _db.Employees.Find(employee.Id);
            value.Name = employee.Name;
            value.Address = employee.Address;
            value.Email = employee.Email;
            value.ContactNumber = employee.ContactNumber;
            value.Gender = employee.Gender;
            value.EmpType = employee.EmpType;
            value.EmpRole = employee.EmpRole;
            value.EmpStage = employee.EmpStage;
            value.JoinDate = employee.JoinDate;
            value.CurrentStageCompletionDate = employee.CurrentStageCompletionDate;
            if (employee.Password != null)
            {
                if (employee.Password.Length < 5)
                {
                    TempData["type"] = "danger";
                    TempData["EditProfileMessage"] = "Password must be atleast 5 character";
                    return RedirectToAction("EmployeeEditProfile", "User", new { id = value.Id });
                }
                if (value.Password != employee.Password)
                {
                    value.Password = PwdEncryption.HashPassword(employee.Password);
                }
            }
            value.UpdatedAt = DateTime.Now;
            _db.SaveChanges();
            TempData["type"] = "success";
            TempData["EditProfileMessage"] = "Profile Update Successful";
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
