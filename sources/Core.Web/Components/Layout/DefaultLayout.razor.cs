using Core.Web.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Models.Authentication;

namespace Core.Web.Components.Layout
{
    public partial class DefaultLayout
    {
        private readonly CurrentUser? _currentUser = new();
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public CurrentUser CurrentUser => _currentUser;
        public bool IsSidebarExpanded { get; set; } = false;

        public DefaultLayout(AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;

            _currentUser = GetCurrentUser();
        }

        protected void ToggleSidebar()
        {
            IsSidebarExpanded = !IsSidebarExpanded;
        }

        protected async Task OnLogout()
        {
            await ((CustomAuthenticationStateProvider)_authenticationStateProvider).LogoutAsync();
            
            _navigationManager.NavigateTo("/auth", true);
        }

        private CurrentUser? GetCurrentUser()
        {
            var currentUser = ((CustomAuthenticationStateProvider)_authenticationStateProvider).GetCurrentUser();

            if(currentUser != null)
            {
                return currentUser;
            }

            return new();
        }
    }
}
