using employeeDailyTaskRecorder.HelperService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace employeeDailyTaskRecorder.CustomAttributes
{
    public class GeneralAuthorizationAttribute : Attribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isLogged = SessionService.GetSession(context.HttpContext) != null;
            //// Check if the user is authenticated
            if (!isLogged)
            {
                //context.Result = new UnauthorizedResult(); 
                context.Result = new RedirectResult("/Auth/Index");

                return;
            }

        }
        
    }
    /*   public class GeneralAdminAuthorizationAttribute : Attribute, IAuthorizationFilter
       {

           public void OnAuthorization(AuthorizationFilterContext context)
           {
               //bool isLogged = context.HttpContext.User.Identity.IsAuthenticated;
               bool isLogged = SessionService.GetSession(context.HttpContext) != null;
               //// Check if the user is authenticated
               if (isLogged)
               {
                   context.Result = new UnauthorizedResult();
                   return;
               }


               // You can also add custom authorization logic here
               // For example, checking user roles or permissions
           }
       }*/
}
