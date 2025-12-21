using Logic.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Service.Api
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class JwtAuthAttribute : Attribute, IAuthorizationFilter
    {
        public string UserRoleString { get; set; } = string.Empty;
        public bool AllowAdmin { get; set; } = false;
        public bool AllowSystemAdmin { get; set; } = false;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            JwtSecurityToken? jwtToken = null;

            if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    jwtToken = handler.ReadJwtToken(token);
                }
            }

            var user = GetClaimsIdentity(jwtToken?.RawData ?? string.Empty);

            if (user?.IsAuthenticated != true)
            {
                context.Result = new ForbidResult();
                return;
            }

            var roleClaimValue = user.Claims.FirstOrDefault(x => x.Type == UserClaimConstants.UserRoleKey)?.Value
                                ?? jwtToken?.Claims.FirstOrDefault(c => c.Type == UserClaimConstants.UserRoleKey)?.Value;

            var userRoles = GetUserRoles();

            var isAuthenticated = false;

            if (!string.IsNullOrEmpty(roleClaimValue))
            {
                var role = (UserRoleEnum)Enum.Parse(typeof(UserRoleEnum), roleClaimValue);

                if(AllowSystemAdmin && role == UserRoleEnum.SystemAdmin || AllowAdmin && role == UserRoleEnum.LocalAdmin || userRoles.Contains(role))
                {
                    isAuthenticated = true;
                    context.HttpContext.User = new ClaimsPrincipal(user);
                }
            }

            if (!isAuthenticated)
            {
                context.Result = new ForbidResult();
            }
        }

        private List<UserRoleEnum> GetUserRoles()
        {
            var userRoles = new List<UserRoleEnum>();

            if (!string.IsNullOrEmpty(UserRoleString))
            {
                var roles = UserRoleString.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var role in roles)
                {
                    if (Enum.TryParse<UserRoleEnum>(role.Trim(), out var parsedRole))
                    {
                        userRoles.Add(parsedRole);
                    }
                }
            }

            return userRoles;
        }

        private ClaimsIdentity GetClaimsIdentity(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var claims = jwtToken.Claims;

            return new ClaimsIdentity(claims, "jwt");
        }
    }
}
