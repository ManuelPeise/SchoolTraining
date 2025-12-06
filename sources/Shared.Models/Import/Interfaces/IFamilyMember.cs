using Shared.Enums;


namespace Shared.Models.Import.Interfaces
{
    public interface IFamilyMember
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string Username { get; set; } 
        public string Email { get; set; } 
        public DateTime? DateOfBirth { get; set; }
        public UserRoleEnum UserRole { get; set; }
        public FamilyMemberCredentials Credentials { get; set; }

    }
}
