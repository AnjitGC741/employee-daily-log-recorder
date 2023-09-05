using employeeDailyTaskRecorder.HelperService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace employeeDailyTaskRecorder.CustomAttributes
{
    public class GeneralAuthorizationAttribute : Attribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = SessionService.GetSession(context.HttpContext);
            bool isLogged = user != null;
            if (!isLogged)
            {
                context.Result = new RedirectResult("/Auth/Index");
                return;
            }

        }
        
    }
    public class AdminAuthorizationAttribute: GeneralAuthorizationAttribute, IAuthorizationFilter
    {
        public new void OnAuthorization(AuthorizationFilterContext context)
        {
            base.OnAuthorization(context);
            var user = SessionService.GetSession(context.HttpContext);
            if (user!=null && user.IsUser)
            {
                context.Result = new RedirectResult("/ErrorHandling/Index");
                return;
            }
        }
    }
}
