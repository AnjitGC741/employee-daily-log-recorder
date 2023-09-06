using Microsoft.AspNetCore.Mvc;
using employeeDailyTaskRecorder.Models;
using employeeDailyTaskRecorder.Data;
using employeeDailyTaskRecorder.HelperService;
namespace employeeDailyTaskRecorder.Controllers
{
    public class ErrorHandling : Controller
    {
        private readonly ApplicationDbContext _db;
        public ErrorHandling(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            Employee empData = SessionService.GetSession(HttpContext);
           var result =  _db.Employees.Where(x => x.Id == empData.Id).ToList();
            return View(result);
        }
    }
}
