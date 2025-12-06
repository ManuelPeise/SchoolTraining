using Microsoft.AspNetCore.Components;

namespace Core.Web.Components.Pages.Shared
{
    public partial class SideMenuIItem
    {
        [Parameter]
        public string Label { get; set; } = string.Empty;
        [Parameter]
        public string Route { get; set; } = string.Empty;
        [Parameter]
        public string IconClass { get; set; } = string.Empty;

        private readonly NavigationManager _navigationManager;

        public SideMenuIItem(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        protected void Navigate()
        {
            _navigationManager.NavigateTo(Route);
        }

    }
}
