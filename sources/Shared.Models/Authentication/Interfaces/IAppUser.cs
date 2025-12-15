using Shared.Enums;

namespace Shared.Models.Authentication.Interfaces
{
    public interface IAppUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public UserRoleEnum UserRole { get; set; }
    }
}
