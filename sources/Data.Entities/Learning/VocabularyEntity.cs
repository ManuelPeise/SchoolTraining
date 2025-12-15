using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Learning
{
    public class VocabularyEntity : AEntityBase
    {
        /// <summary>
        /// Gets or sets the external identifier associated with the entity [data import only].
        /// </summary>
        public string IdExternal { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the value represented in Danish.
        /// </summary>
        public string DanishValue { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets an example sentence in Danish.
        /// </summary>
        public string? DanishExampleSentence { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the Danish phonetic spelling of the word or phrase.
        /// </summary>
        public string? DanishPhoneticSpelling { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the value represented in English.
        /// </summary>
        public string EnglishValue { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets an example sentence in English that illustrates the usage or meaning of a word or phrase.
        /// </summary>
        public string? EnglishExampleSentence { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the English phonetic spelling associated with the current entity.
        /// </summary>
        public string? EnglishPhoneticSpelling { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the French language value associated with this instance.
        /// </summary>
        public string FrechValue { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets an example sentence in French that demonstrates the usage of a word or phrase.
        /// </summary>
        public string? FrenchExampleSentence { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the French phonetic spelling of the word or phrase.
        /// </summary>
        public string? FrenchPhoneticSpelling { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the value represented in German.
        /// </summary>
        public string GermanValue { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets an example sentence in German.
        /// </summary>
        public string GermanExampleSentence { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the German phonetic spelling associated with the entity.
        /// </summary>
        public string? GermanPhoneticSpelling { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the unique identifier for the unit.
        /// </summary>
        public int UnitId { get; set; }
        [ForeignKey(nameof(UnitId))]
        public UnitEntity Unit { get; set; } = new();

    }
}
