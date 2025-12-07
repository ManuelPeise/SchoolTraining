namespace Shared.Models.Administration.Interfaces
{
    public interface IFamiy
    {
        public int FamilyId { get; set; }
        public string Name { get; set; }
        public string ContactMailAddress { get; set; }
        public bool IsActive { get; set; }
        public List<FamilyMemberModel> Members { get; set; }
    }
}
