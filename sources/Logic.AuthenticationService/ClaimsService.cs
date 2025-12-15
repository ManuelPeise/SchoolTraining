using Data.Entities;
using Logic.Shared;
using System.Security.Claims;


namespace Logic.AuthenticationService
{
    public static class ClaimsService
    {
        public static List<Claim> GetUserClaims(UserEntity userEntity)
        {
            return new List<Claim>
            {
                new Claim(UserClaimConstants.UserNameKey, userEntity.Username),
                new Claim(UserClaimConstants.UserIdKey, userEntity.Id.ToString()),
                new Claim(UserClaimConstants.FamilyIdKey, userEntity.FamilyId?.ToString() ?? ""),
                new Claim(UserClaimConstants.UserRoleKey, userEntity.UserRole.ToString()),
                new Claim(UserClaimConstants.SessionExpireTime, DateTime.UtcNow.AddHours(1).ToString("o"))
            };
        }
    }
}
