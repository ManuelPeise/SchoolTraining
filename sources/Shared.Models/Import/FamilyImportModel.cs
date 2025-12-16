namespace Shared.Models.Import
{
    public class FamilyImportModel
    {
        public List<string> ExistingExternalIds { get; set; } = new List<string>();
        public FamilyModel Family { get; set; } = new();
    }
}
