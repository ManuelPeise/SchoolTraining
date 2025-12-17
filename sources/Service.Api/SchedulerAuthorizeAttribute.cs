using Logic.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Enums;
using System.Security.Claims;

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

                var claims = user?.Claims;

                if (claims == null || 
                    !Enum.TryParse<UserRoleEnum>(claims.FirstOrDefault(c => c.Type == UserClaimConstants.UserRoleKey)?.Value ?? "", out var userRole))
                {
                    context.Result = new ForbidResult();
                    return;
                }

                // Check for the SystemJob authentication type or the special claim/role
                if (user?.Identity?.IsAuthenticated != true ||
                    user.Identity.AuthenticationType != "ScheduleJob" || userRole != UserRoleEnum.MaintanaceUser)
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
