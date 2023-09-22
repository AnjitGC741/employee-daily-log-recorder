namespace employeeDailyTaskRecorder.Hangfire
{
    public interface ISendEmail
    {
        Task SendEmailToAdmin();
        Task SendEmailToEmployee();
    }
}
