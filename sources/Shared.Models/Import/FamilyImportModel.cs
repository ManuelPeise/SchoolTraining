using Shared.Models.Import.Interfaces;

namespace Shared.Models.Import
{
    public class FamilyImportModel : IFamilyImportModel
    {
        public string Name { get; set; } = string.Empty;
        public string ContactMailAddress { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<FamilyMember> FamilyMembers { get; set; } = new List<FamilyMember>();
    }
}
