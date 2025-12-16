using Shared.Enums;

namespace Shared.Models.Import
{
    public class FamilyMember
    {
        public string IdExternal { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }
        public UserRoleEnum UserRole { get; set; }
        public FamilyMemberCredentials Credentials { get; set; } = new FamilyMemberCredentials();
        public FamilyMemberSettings Settings { get; set; } = new FamilyMemberSettings();
    }
}
