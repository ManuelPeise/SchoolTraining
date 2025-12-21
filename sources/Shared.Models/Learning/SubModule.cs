
using Shared.Enums;

namespace Shared.Models.Learning
{
    // keep in sync with src\pages\private\Configuration\models\subModule.ts\ISubModuleBase
    public class SubModuleBase
    {
        public int SubModuleId { get; set; }
        public int ModuleId { get; set; }
        public string IdExternal { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? LastUpdateBy { get; set; }
        public string? LastUpdateAt { get; set; }
        public VocabularyDirectionEnum? Direction { get; set; }
    }

    public class SubModule: SubModuleBase
    {
        public Module? Module { get; set; }
    }

    // keep in sync with src\pages\private\Configuration\models\subModule.ts\ISubModuleDataCollection
    public class SubModuleDataCollection
    {
        public int ModuleId { get; set; }
        public List<SubModuleBase> SubModules { get; set; } = new();
        public List<DropdownItem> SubModuleDropdownItems { get; set; } = new();
    }
}
