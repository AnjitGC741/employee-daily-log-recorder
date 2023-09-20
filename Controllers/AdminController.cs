using Microsoft.AspNetCore.Mvc;
using employeeDailyTaskRecorder.Data;
using employeeDailyTaskRecorder.Models;
using Microsoft.EntityFrameworkCore;
using employeeDailyTaskRecorder.HelperService;
using employeeDailyTaskRecorder.CustomAttributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using employeeDailyTaskRecorder.Mail;
using System.Text;
namespace employeeDailyTaskRecorder.Controllers
{
    //[GeneralAuthorization]
    [AdminAuthorization]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private Employee _ActiveUser => SessionService.GetSession(HttpContext);
        public AdminController(ApplicationDbContext db) : base()
        {
            _db = db;

        }
        [HttpGet]
        [HttpPost]
        //attributes so that only admin can access this page
        public async Task<IActionResult> Index(VMAdminIndex? SearchData)
        {
            VMAdminIndex Result = SearchData == null ? new VMAdminIndex() : SearchData;
            DateTime currentDateTime = DateTime.Now;
            string formattedDate = currentDateTime.ToString("MM/dd/yyyy");
            IQueryable<Record> recordList = _db.Records
             .Include(x => x.Employee).Where(x => x.TaskPerformedDate.Date >= Result.FromDate.Date && x.TaskPerformedDate.Date <= Result.ToDate.Date);
            if (SearchData.EmployeeID.HasValue)
            {
                recordList = recordList.Where(x => x.EmployeeId == SearchData.EmployeeID.Value);
            }
            if (!string.IsNullOrEmpty(SearchData.SearchTerm))
            {
                recordList = recordList.Where(x => x.Task.Contains(SearchData.SearchTerm));
            }
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
            ViewBag.EmployeeList = new SelectList(_db.Employees.ToList(), "Id", "Name");
            TempData["activeUser"] = _ActiveUser.Id;
            TempData["activeUserName"] = SearchData.EmployeeName;
            Result.EmployeeList = _db.Employees.ToList();
            Result.TaskList = await recordList.OrderByDescending(x => x.TaskPerformedDate).ToListAsync();
            return View(Result);
        }
        public IActionResult employeeList()
        {
            List<Employee> employeeList = _db.Employees.ToList();
            return View(employeeList);
        }
        public async Task<ActionResult> SendEmail()
        {
            int sn = 1;
            DateTime currentDateTime = DateTime.Now;
            string formattedDate = currentDateTime.ToString("MM/dd/yyyy");
            List<Record> records = _db.Records.Include(x => x.Employee).ToList();
            var task = records.Where(x => x.TaskPerformedDate.ToString("MM/dd/yyyy") == formattedDate).ToList();
            StringBuilder tableHtml = new StringBuilder();
            tableHtml.Append("<table border='1' cellspacing='0' cellpadding='10' id='employeeList' style='border-collapse: collapse; width: 100%;'>");
            tableHtml.Append("<thead><tr style='background-color: #37304C; color: #fff; font-weight: bold;'>");
            tableHtml.Append("<th style='text-align: left;'>SN</th>");
            tableHtml.Append("<th style='text-align: left;'>Employee Name</th>");
            tableHtml.Append("<th style='text-align: left;'>Task Description</th>");
            tableHtml.Append("</tr></thead><tbody>");
            foreach (var record in task)
            {
                tableHtml.Append("<tr>");
                tableHtml.Append($"<td style='text-align: left;'>{sn++}</td>");
                tableHtml.Append($"<td style='text-align: left;'>{record.Employee.Name}</td>");
                tableHtml.Append($"<td style='text-align: left;'>{record.Task}</td>");
                tableHtml.Append("</tr>");
            }
            tableHtml.Append("</tbody></table>");
            string emailBody = $"<html><body><h2>Today's employee tasks</h2>{tableHtml.ToString()}</body></html>";
            if (task != null)
            {
                await EmailService.SendEmailAsync("admin@antsware.com", "Todays Task",emailBody);
            }
            return RedirectToAction("Index", "Admin");
        }

    }
}
