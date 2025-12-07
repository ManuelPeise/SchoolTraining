using Shared.Enums;
using Shared.Models.Administration.Interfaces;

namespace Shared.Models.Administration
{
    public class FamilyMemberModel : IFamilyMember
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? Email { get; set; } = null;
        public DateTime? DateOfBirth { get; set; }
        public UserRoleEnum UserRole { get; set; }
        public bool IsActive { get; set; }
    }
}
