using Microsoft.AspNetCore.Components;
using Shared.Models.Ui;

namespace Core.Web.Components.Pages.Shared
{
    public partial class SideMenu
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public bool IsExpanded { get; set; }

        public string SideMenuCssClass => IsExpanded ? "side-menu expanded" : "side-menu";

        public List<SideMenuEntry> DefaultEntries { get; set; } = new();

        protected override void OnInitialized()
        {
            DefaultEntries = new List<SideMenuEntry>
            {
                new SideMenuEntry
                {
                    NavigationManager = NavigationManager,
                    Label = "Dashboard",
                    Route = "/counter"
                },
                new SideMenuEntry
                {
                    NavigationManager = NavigationManager,
                    Label = "Reports",
                    Route = "/counter"
                },
                new SideMenuEntry
                {
                    NavigationManager = NavigationManager,
                    Label = "Settings",
                    Route = "/counter"
                }
            };
        }
    }
}
