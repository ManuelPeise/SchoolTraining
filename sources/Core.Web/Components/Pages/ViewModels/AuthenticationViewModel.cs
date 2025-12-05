using Core.Web.Providers;
using Core.Web.ViewModels;
using Logic.Shared.Interfaces.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Models.Authentication;

namespace Core.Web.Components.Pages.ViewModels
{
    public class AuthenticationViewModel : ViewModelBase
    {
        private readonly IUserAuthenticationService _authenticationService;
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public LoginModel LoginModel { get; set; } = new LoginModel();
        public string? Error { get; set; } = "Invalid username or password.";
        public bool ShowError { get; set; }

        public AuthenticationViewModel(
            IUserAuthenticationService authenticationService,
            NavigationManager navigationManager,
            CustomAuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationService = authenticationService;
            _navigationManager = navigationManager;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SignInAsync()
        {
            var tokenResponse = await _authenticationService.SignInAsync(LoginModel);

            if (tokenResponse == null)
            {
                return;
            }

            if (await ((CustomAuthenticationStateProvider)_authenticationStateProvider).AuthenticateAsync(tokenResponse))
            {
                _navigationManager.NavigateTo("/", forceLoad: true);
            }
            else
            {
                ShowError = true;
            }
        }
    }
}
