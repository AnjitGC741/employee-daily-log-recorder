using employeeDailyTaskRecorder.Models;
using System.Text.Json;

namespace employeeDailyTaskRecorder.HelperService
{
    public class SessionService
    {
        public static string SessionKeyName = "LoggedInUser";
        internal static void SetSession(Employee empData, HttpContext httpContext)
        {
            httpContext.Session.Set<Employee>(SessionKeyName, empData);
        }

        internal static Employee GetSession(HttpContext httpContext)
        {
            return httpContext.Session.Get<Employee>(SessionKeyName);
        }
        internal static Employee ClearSession(HttpContext httpContext)
        {
             httpContext.Session.Clear();
            return null;
        }
    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
