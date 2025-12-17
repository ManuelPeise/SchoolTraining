namespace Shared.Models.Learning
{
    public class ModuleInitializationModel
    {
        public List<Module> Modules { get; set; } = new();
        public List<SubModule> SubModules { get; set; } = new();
    }
}
