﻿using Microsoft.AspNetCore.Mvc;
using employeeDailyTaskRecorder.Data;
using employeeDailyTaskRecorder.Models;
using Microsoft.EntityFrameworkCore;
using employeeDailyTaskRecorder.HelperService;
using employeeDailyTaskRecorder.Hash;
namespace employeeDailyTaskRecorder.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor contxt;
        public AuthController(ApplicationDbContext db)
        {

            _db = db;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult authenticateUser(String email, String password)
        {
            if (password != null && email != null)
            {
                Employee EmpData = _db.Employees.FirstOrDefault(x => x.Email == email && x.IsDeleted == false);
                if (EmpData == null)
                {

                    TempData["ErrorMessage"] = "Incorrect Email";
                    return RedirectToAction("Index", "Auth");
                }
                var passwordValue = EmpData.Password;
                if (passwordValue != PwdEncryption.HashPassword(password))
                {
                    TempData["ErrorMessage"] = "Incorrect password.";
                    return RedirectToAction("Index", "Auth");
                }
                SessionService.SetSession(EmpData, HttpContext);

                if (EmpData.IsAdmin)
                {
                    return RedirectToAction("EmployeeCompletionStatus", "User");

                }
                return RedirectToAction("EmployeeTask", "UserTask");
            }
            TempData["ErrorMessage"] = "Empty Field";
            return RedirectToAction("Index", "Auth");

        }
        public IActionResult Logout()
        {
            SessionService.ClearSession( HttpContext);
            return RedirectToAction("Index", "Auth");
        }
    }
}
