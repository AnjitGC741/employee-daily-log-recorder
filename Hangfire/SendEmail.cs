using employeeDailyTaskRecorder.Data;
using employeeDailyTaskRecorder.Models;
using Microsoft.EntityFrameworkCore;
using employeeDailyTaskRecorder.Mail;
using System.Text;
namespace employeeDailyTaskRecorder.Hangfire
{
    public class SendEmail : ISendEmail
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        public DateTime currentDateTime = DateTime.Now;
        public string formattedDate;
        public SendEmail(ApplicationDbContext db, IConfiguration configuration) : base()
        {
            _db = db;
            _configuration = configuration;
        }
        public async Task SendEmailToAdmin()
        {
            int sn = 1;
            formattedDate = currentDateTime.ToString("MM/dd/yyyy");
            List<Record> records = _db.Records.Include(x => x.Employee).ToList();
            var task = records.Where(x => x.TaskPerformedDate.ToString("MM/dd/yyyy") == formattedDate).ToList();
            if (task != null)
            {
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
                await EmailService.SendEmailAsync(_configuration, emailBody, null, "Today's Employee Tasks");
            }
            throw new NotImplementedException();
        }
        public async Task SendEmailToEmployee()
        {

            formattedDate = currentDateTime.ToString("MM/dd/yyyy");
            List<Employee> employees = _db.Employees.ToList();
            List<Record> records = _db.Records.Include(x => x.Employee).ToList();
            var task = records.Where(x => x.TaskPerformedDate.ToString("MM/dd/yyyy") == formattedDate).ToList();
            foreach (var employeeList in employees)
            {
                var isTaskInserted = task.Where(x => x.EmployeeId == employeeList.Id).FirstOrDefault();
                if (isTaskInserted == null)
                {
                    StringBuilder noticeHtml = new StringBuilder();
                    noticeHtml.Append($"<p style='margin-bottom:10px;'>Dear {employeeList.Name}</p>");
                    noticeHtml.Append("<p style='margin-bottom:10px;'>Please remember to submit today's task report on time. If you face any issues, contact to Admin" +
                        ".</p>");
                    noticeHtml.Append($"<p style='margin-bottom:10px;'>Best Regards</p>");
                    noticeHtml.Append($"<p style='margin-bottom:10px;'>Ant Pvt Ltd</p>");
                    string emailBody = $"<html><body>{noticeHtml.ToString()}</body></html>";
                    await EmailService.SendEmailAsync(_configuration, emailBody ,employeeList.Email, "Today's Task Submission Reminder");
                }
            }
            throw new NotImplementedException();
        }

    }
}
