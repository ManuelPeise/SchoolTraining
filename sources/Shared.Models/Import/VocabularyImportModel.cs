namespace Shared.Models.Import
{
    public class VocabularyImportModel
    {
        public List<string> ExistingExternalIds { get; set; } = new List<string>();
        public List<VocabularyModel> Vocabularies { get; set; } = new List<VocabularyModel>();
    }
}
