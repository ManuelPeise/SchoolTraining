using Microsoft.AspNetCore.Components;

namespace Shared.Models.Ui
{
    public class SideMenuEntry
    {
        public NavigationManager? NavigationManager;
        public string Label { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;

       
        public void Navigate()
        {
            NavigationManager?.NavigateTo(Route);
        }
    }
}
