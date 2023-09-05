using Microsoft.AspNetCore.Mvc;

namespace employeeDailyTaskRecorder.Controllers
{
    public class ErrorHandling : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
