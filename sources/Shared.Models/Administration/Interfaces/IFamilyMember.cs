using Shared.Models.Authentication.Interfaces;

namespace Shared.Models.Administration.Interfaces
{
    public interface IFamilyMember: IAppUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsActive { get; set; }
    }
}
