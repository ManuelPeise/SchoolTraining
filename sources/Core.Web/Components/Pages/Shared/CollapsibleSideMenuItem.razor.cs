using Microsoft.AspNetCore.Components;
using Shared.Models.Ui;


namespace Core.Web.Components.Pages.Shared
{
    public partial class CollapsibleSideMenuItem
    {
       
        [Parameter]
        public string Label { get; set; } = string.Empty;
        [Parameter]
        public string IconClass { get; set; } = string.Empty;
        [Parameter]
        public List<SideMenuEntry> SubItems { get; set; } = new();
        [Parameter]
        public bool IsDisabled { get; set; } = false;
        public bool IsExpanded { get; set; } = false;

        protected void ToggleExpanded()
        {
            IsExpanded = !IsExpanded;
        }
    }
}
