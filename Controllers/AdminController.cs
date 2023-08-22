using Microsoft.AspNetCore.Mvc;
using employeeDailyTaskRecorder.Data;
using employeeDailyTaskRecorder.Models;
using Microsoft.EntityFrameworkCore;
using employeeDailyTaskRecorder.HelperService;
using employeeDailyTaskRecorder.CustomAttributes;

namespace employeeDailyTaskRecorder.Controllers
{
    [GeneralAuthorization]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private Employee _ActiveUser => SessionService.GetSession(HttpContext);
        public AdminController(ApplicationDbContext db) : base()
        {
            _db = db;
          

        }
        //attributes so that only admin can access this page
        public async Task<IActionResult> Index()
        {
            VMAdminIndex Result = new VMAdminIndex();
            DateTime currentDateTime = DateTime.Now;
            string formattedDate = currentDateTime.ToString("MM/dd/yyyy");
            List<Record> records = _db.Records.Where(x => x.EmployeeId == _ActiveUser.Id).ToList();
            Result.CanAddTask = true;
            foreach(var data in records)
            {
                if((data.TaskPerformedDate.ToString("MM/dd/yyyy")).ToString() == formattedDate)
                {
                    Result.CanAddTask = false;
                    break;
                }
            }
            Result.EmployeeID = _ActiveUser.Id;
            Result.EmployeeList = _db.Employees.ToList();
            Result.TaskList = await _db.Records.ToListAsync();
            return View(Result);
        }
        public async Task<IActionResult> filterData(VMAdminIndex? SearchData)
        {
            VMAdminIndex Result = SearchData == null ? new VMAdminIndex() : SearchData;
            IQueryable<Record> recordList = _db.Records
                .Include(x => x.Employee)
                .Where(x => x.TaskPerformedDate.Date >= Result.FromDate && x.TaskPerformedDate <= Result.ToDate);
            if (SearchData.EmployeeID.HasValue)
            {
                recordList = recordList.Where(x => x.EmployeeId == SearchData.EmployeeID.Value);
            }
            if (!string.IsNullOrEmpty(SearchData.SearchTerm))
            {
                recordList = recordList.Where(x => x.Task.Contains(SearchData.SearchTerm));
            }
            Result.EmployeeList = _db.Employees.ToList();
            Result.TaskList = await recordList.ToListAsync();
            return View("Index", Result);
        }
        public IActionResult employeeList()
        {
            List<Employee> employeeList = _db.Employees.ToList();
            return View(employeeList);
        }

    }
}
