using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models.Authentication;

namespace Logic.Shared
{
    public abstract class LogicBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly CurrentUser _currentUser = new();
        public IHttpContextAccessor HttpContextAccessor { get => _httpContextAccessor; }
        public HttpContext HttpContext { get => _httpContextAccessor.HttpContext; }
        public CurrentUser CurrentUser { get => _currentUser; }

        protected LogicBase(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            _currentUser = GetCurrentUserFromHttpContext();
        }

        private CurrentUser GetCurrentUserFromHttpContext()
        {
            var context = HttpContext;
            
            if (context == null || context.User == null || context.User.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                return new CurrentUser
                {
                    UserId = 0,
                    UserName = string.Empty,
                    UserRole = UserRoleEnum.None
                };
            }

            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == UserClaimConstants.UserIdKey);
            var userNameClaim = context.User.Claims.FirstOrDefault(c => c.Type == UserClaimConstants.UserNameKey);
            var userRoleClaim = context.User.Claims.FirstOrDefault(c => c.Type == UserClaimConstants.UserRoleKey);
            var familyIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == UserClaimConstants.FamilyIdKey);

            return new CurrentUser
            {
                UserId = !string.IsNullOrEmpty(userIdClaim?.Value) ? int.Parse(userIdClaim.Value) : 0,
                FamilyId = !string.IsNullOrEmpty(familyIdClaim?.Value) ? int.Parse (familyIdClaim.Value) : null,
                UserName = !string.IsNullOrEmpty(userNameClaim?.Value) ? userNameClaim.Value : string.Empty,
                UserRole = !string.IsNullOrEmpty(userRoleClaim?.Value) ? Enum.Parse<UserRoleEnum>(userRoleClaim.Value) : UserRoleEnum.None
            };
        }
    }
}
