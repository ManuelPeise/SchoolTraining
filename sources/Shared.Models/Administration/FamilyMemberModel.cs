using Shared.Enums;

namespace Shared.Models.Administration
{
    public class FamilyMemberModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? Email { get; set; } = null;
        public DateTime DateOfBirth { get; set; }
        public UserRoleEnum UserRole { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
    }
}

