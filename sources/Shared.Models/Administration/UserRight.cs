namespace Shared.Models.Administration
{
    public class UserRight
    {
        public Guid RightGuid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? NameResourceKey { get; set; }
        public string? DescriptionResourceKey { get; set; }
        public bool Deny { get; set; }
        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}
