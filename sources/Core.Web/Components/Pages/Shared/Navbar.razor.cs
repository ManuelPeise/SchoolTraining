using Microsoft.AspNetCore.Components;

namespace Core.Web.Components.Pages.Shared
{
    public partial class NavBar : ComponentBase
    {
        [Parameter]
        public EventCallback OnToggleSidebar { get; set; }
        [Parameter]
        public EventCallback OnLogout { get; set; }

        [Parameter]
        public string UserName { get; set; } = string.Empty;

        protected Task ToggleSidebar()
        {
            return OnToggleSidebar.HasDelegate ? OnToggleSidebar.InvokeAsync() : Task.CompletedTask;
        }

        protected Task Logout()
        {
            return OnLogout.HasDelegate ? OnLogout.InvokeAsync() : Task.CompletedTask;
        }
    }
}
