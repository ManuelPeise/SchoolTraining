namespace Shared.Models.Administration
{
    public class FamilyModel
    {
        public int FamilyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactMailAddress { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string LastUpdateBy { get; set; } = string.Empty;
        public string LastUpdateAt { get; set; } = string.Empty;
    }
}
