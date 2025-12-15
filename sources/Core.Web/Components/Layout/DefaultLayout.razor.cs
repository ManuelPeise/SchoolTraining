using Core.Web.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Models.Authentication;

namespace Core.Web.Components.Layout
{
    public partial class DefaultLayout
    {
        private CurrentUser? _currentUser = new();
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public CurrentUser? CurrentUser => _currentUser;
        public bool IsSidebarExpanded { get; set; } = false;
        private string UserRole { get; set; } = string.Empty;
        
        public DefaultLayout(AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;

           
        }

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await GetCurrentUser();
        }

        protected void ToggleSidebar()
        {
            IsSidebarExpanded = !IsSidebarExpanded;
        }

        protected async Task OnLogout()
        {
            await ((CustomAuthenticationStateProvider)_authenticationStateProvider).LogoutAsync();
            
            _navigationManager.NavigateTo("/authentication", true);
        }

        protected async Task OnLoginClicked()
        {
            await ((CustomAuthenticationStateProvider)_authenticationStateProvider).LogoutAsync();

            _navigationManager.NavigateTo("/authentication", true);
        }

        private async Task<CurrentUser?> GetCurrentUser()
        {
            var currentUser = await ((CustomAuthenticationStateProvider)_authenticationStateProvider).GetCurrentUser();

            if(currentUser != null)
            {
                UserRole = currentUser.UserRole.ToString() ?? string.Empty;
                return currentUser;
            }

            return new();
        }
    }
}
