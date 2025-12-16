namespace Shared.Models.Import
{
    public class VocabularyModel
    {
        public string IdExternal { get; set; } = string.Empty;
        public string DanishValue { get; set; } = string.Empty;
        public string? DanishExampleSentence { get; set; } = string.Empty;
        public string? DanishPhoneticSpelling { get; set; } = string.Empty;
        public string EnglishValue { get; set; } = string.Empty;
        public string? EnglishExampleSentence { get; set; } = string.Empty;
        public string? EnglishPhoneticSpelling { get; set; } = string.Empty;
        public string FrechValue { get; set; } = string.Empty;
        public string? FrenchExampleSentence { get; set; } = string.Empty;
        public string? FrenchPhoneticSpelling { get; set; } = string.Empty;
        public string GermanValue { get; set; } = string.Empty;
        public string GermanExampleSentence { get; set; } = string.Empty;
        public string? GermanPhoneticSpelling { get; set; } = string.Empty;
    }
}
