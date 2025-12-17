namespace Data.Entities.Learning
{
    public class VocabularyUnitEntity : AEntityBase
    {
        public int UnitId { get; set; }
        public UnitEntity Unit { get; set; }
        public int VocabularyId { get; set; }
        public VocabularyEntity Vocabulary { get; set; }
    }
}
