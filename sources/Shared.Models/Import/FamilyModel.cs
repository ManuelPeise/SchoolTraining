namespace Shared.Models.Import
{
    public class FamilyModel
    {
        public int FamilyId { get; set; }
        public string IdExternal { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ContactMailAddress { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<FamilyMember> FamilyMembers { get; set; } = new List<FamilyMember>();
        public string LastUpdateAt { get; set; } = string.Empty;
        public string LastUpdateBy { get; set; } = string.Empty;
    }
}
