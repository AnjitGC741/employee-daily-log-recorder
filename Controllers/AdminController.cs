using Microsoft.AspNetCore.Mvc;
using employeeDailyTaskRecorder.Data;
using employeeDailyTaskRecorder.Models;
using Microsoft.EntityFrameworkCore;
using employeeDailyTaskRecorder.HelperService;
using employeeDailyTaskRecorder.CustomAttributes;
using Microsoft.AspNetCore.Mvc.Rendering;

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

    }
}
