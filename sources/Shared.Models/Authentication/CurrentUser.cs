using Shared.Enums;
using Shared.Models.Authentication.Interfaces;

namespace Shared.Models.Authentication
{
    public class CurrentUser : IAppUser
    {
        public int UserId { get ; set; }
        public int? FamilyId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public UserRoleEnum UserRole { get; set; }
    }
}
