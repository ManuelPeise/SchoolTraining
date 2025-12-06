namespace Shared.Models.Import.Interfaces
{
    public interface IFamilyImportModel
    {
        public string Name { get; set; }
        public string ContactMailAddress { get; set; }
        public bool IsActive { get; set; }
        public List<FamilyMember> FamilyMembers { get; set; }
    }
}
