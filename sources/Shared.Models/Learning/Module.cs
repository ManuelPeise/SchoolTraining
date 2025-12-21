namespace Shared.Models.Learning
{
    public class Module
    {
        public int ModuleId { get; set; }
        public string IdExternal { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? LastUpdateBy { get; set; }
        public string? LastUpdateAt { get; set; }
        public List<SubModule> SubModules { get; set; } = new();
    }
}
