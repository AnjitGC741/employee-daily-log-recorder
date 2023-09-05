using Microsoft.AspNetCore.Mvc;
using employeeDailyTaskRecorder.Data;
using employeeDailyTaskRecorder.Models;
using employeeDailyTaskRecorder.HelperService;
using employeeDailyTaskRecorder.CustomAttributes;

namespace employeeDailyTaskRecorder.Controllers
{
    [GeneralAuthorization]
    public class UserTaskController : Controller
    {
        //private Employee empData;
        private readonly ApplicationDbContext _db;
        private Employee _ActiveUser => SessionService.GetSession(HttpContext);
        public UserTaskController(ApplicationDbContext db) : base()
        {
            _db = db;

        }
        public IActionResult EmployeeTask()
        {
            VMAdminIndex Result = new VMAdminIndex();
            DateTime currentDateTime = DateTime.Now;
            string formattedDate = currentDateTime.ToString("MM/dd/yyyy");
            List<Record> records = _db.Records.Where(x => x.EmployeeId == _ActiveUser.Id).ToList();
            Result.CanAddTask = true;
            foreach (var data in records)
            {
                if ((data.TaskPerformedDate.ToString("MM/dd/yyyy")).ToString() == formattedDate)
                {
                    Result.CanAddTask = false;
                    break;
                }
            }
            Result.EmployeeID = _ActiveUser.Id;
            Result.EmployeeName = _ActiveUser.Name;
            Result.EmployeeEmail = _ActiveUser.Email;
            Result.TaskList = _db.Records.Where(x => x.EmployeeId == _ActiveUser.Id).ToList();
            Result.EmployeeList = _db.Employees.Where(x => x.Id == _ActiveUser.Id).ToList();
            return View(Result);
        }
        public IActionResult addTask(Record record)
        {
            //Employee empData = SessionService.GetSession(HttpContext);
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            DateTime currentDateTime = DateTime.Now;
            Record value = new Record();
            value.Task = record.Task;
            value.TaskPerformedDate = currentDateTime;
            value.EmployeeId = _ActiveUser.Id;
            value.Ipaddress = ip;
            _db.Records.Add(value);
            _db.SaveChanges();
            if (_ActiveUser.IsAdmin)
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("EmployeeTask");
        }
        public IActionResult editTask(Record record, String Type)
        {
            Employee empData = SessionService.GetSession(HttpContext);
            Record value = _db.Records.Find(record.Id);
            value.Task = record.Task;
            _db.SaveChanges();
            if (empData.IsAdmin)
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("EmployeeTask");
        }
        public IActionResult deleteTask(int taskId)
        {
            Employee empData = SessionService.GetSession(HttpContext);
            Record data = _db.Records.Find(taskId);
            _db.Records.Remove(data);
            _db.SaveChanges();
            if (empData.IsAdmin)
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("EmployeeTask");
        }
    }
}
