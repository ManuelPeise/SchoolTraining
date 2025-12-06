using Shared.Enums;
using Shared.Models.Import.Interfaces;

namespace Shared.Models.Import
{
    public class FamilyMember : IFamilyMember
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public UserRoleEnum UserRole { get; set; }
        public FamilyMemberCredentials Credentials { get; set; } = new FamilyMemberCredentials();
        public FamilyMemberSettings Settings { get; set; } = new FamilyMemberSettings();
    }
}
