using Shared.Models.Import.Interfaces;

namespace Shared.Models.Import
{
    public class FamilyMemberCredentials : IFamilyMemberCredentials
    {
        public string Password { get; set; } = string.Empty;
    }
}
