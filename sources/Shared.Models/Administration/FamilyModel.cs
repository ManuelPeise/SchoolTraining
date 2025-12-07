using Shared.Models.Administration.Interfaces;

namespace Shared.Models.Administration
{
    public class FamilyModel : IFamiy
    {
        public int FamilyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactMailAddress { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<FamilyMemberModel> Members { get; set; } = new();
    }
}
