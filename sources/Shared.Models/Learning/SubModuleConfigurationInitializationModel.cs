namespace Shared.Models.Learning
{
    // keep in sync with src\pages\private\Configuration\SubModuleConfiguration\models\ISubModuleConfigurationInitializationModel.ts
    public class SubModuleConfigurationInitializationModel
    {
        public List<DropdownItem> ParentModuleDropdownItems { get; set; } = new();
        public List<Module> Modules { get; set; } = new();
        public List<SubModuleDataCollection> SubModuleDataCollection { get; set; } = new();
    }
}
