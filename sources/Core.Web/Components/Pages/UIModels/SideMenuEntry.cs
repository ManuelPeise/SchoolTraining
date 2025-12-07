using Microsoft.AspNetCore.Components;
using Shared.Enums;

namespace Shared.Models.Ui
{
    public class SideMenuEntry
    {
        public NavigationManager? NavigationManager;
        public string Label { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public List<UserRoleEnum> AllowedUserRoles { get; set; } = new();
        public void Navigate()
        {
            NavigationManager?.NavigateTo(Route);
        }
    }
}
