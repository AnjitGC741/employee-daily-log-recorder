using Microsoft.AspNetCore.Mvc;

namespace employeeDailyTaskRecorder.Controllers
{
    public class LeaveRequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
