using Shared.Enums;

namespace Data.Entities.Administation
{
    public class RightEntity : AEntityBase
    {
        public Guid RightGuid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? NameResourceKey { get; set; }
        public string? DescriptionResourceKey { get; set; }
    }
}