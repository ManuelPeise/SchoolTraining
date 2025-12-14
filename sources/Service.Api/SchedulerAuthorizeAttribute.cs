using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Enums;

namespace Service.Api
{
    namespace Service.Api.Scheduler
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class SchedulerAuthorizeAttribute : Attribute, IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var user = context.HttpContext.User;
                // Check for the SystemJob authentication type or the special claim/role
                if (user?.Identity?.IsAuthenticated != true ||
                    user.Identity.AuthenticationType != "ScheduleJob" ||
                    !user.IsInRole(UserRoleEnum.MaintanaceUser.ToString()))
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
