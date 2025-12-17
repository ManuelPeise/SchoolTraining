
using Shared.Enums;

namespace Shared.Models.Learning
{
    public class SubModule
    {
        public int SubModuleId { get; set; }
        public int ModuleId { get; set; }
        public string IdExternal { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LastUpdateBy { get; set; } = string.Empty;
        public VocabularyDirectionEnum? Direction { get; set; }
        public DateTime LastUpdateAt { get; set; }
    }
}
