using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models.Authentication;

namespace Logic.Shared
{
    public abstract class LogicBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly CurrentUser? _currentUser;
        public HttpContext HttpContext { get => _httpContextAccessor.HttpContext; }
        public CurrentUser? CurrentUser { get => _currentUser; }

        protected LogicBase(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            _currentUser = GetCurrentUserFromHttpContext();
        }

        private CurrentUser? GetCurrentUserFromHttpContext()
        {
            var context = HttpContext;
            
            if (context == null || context.User == null || context.User.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "UserId");
            var userNameClaim = context.User.Claims.FirstOrDefault(c => c.Type == "UserName");
            var userRoleClaim = context.User.Claims.FirstOrDefault(c => c.Type == "UserRole");
            
            return new CurrentUser
            {
                UserId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0,
                UserName = userNameClaim?.Value ?? string.Empty,
                UserRole = userRoleClaim != null ? Enum.Parse<UserRoleEnum>(userRoleClaim.Value) : UserRoleEnum.Guest
            };
        }
    }
}
