using Core.Web.Providers;
using Core.Web.ViewModels;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Models.Authentication;
using System.Text.Json;

namespace Core.Web.Components.Pages.ViewModels
{
    public class AuthenticationViewModel : ViewModelBase
    {
        private readonly IApiHttpClient _apiHttpClient;

        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public LoginModel LoginModel { get; set; } = new LoginModel();
        public string? Error { get; set; } = "Invalid username or password.";
        public bool ShowError { get; set; }

        public AuthenticationViewModel(
            IApiHttpClient apiHttpClient,
            NavigationManager navigationManager,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _apiHttpClient = apiHttpClient;
            _navigationManager = navigationManager;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SignInAsync()
        {
            try
            {
                SetIsLoading(true);

                var tokenResponse = await _apiHttpClient.PostAsync<JwtTokenResponse>("api/login/authenticate", JsonSerializer.Serialize(LoginModel));

                if (tokenResponse == null || !tokenResponse.IsSuccess)
                {
                    return;
                }

                if (await ((CustomAuthenticationStateProvider)_authenticationStateProvider).AuthenticateAsync(tokenResponse.Data))
                {
                    _navigationManager.NavigateTo("/", true);
                }
                else
                {
                    ShowError = true;
                }
            }
            finally
            {
                SetIsLoading(false);
            }
        }
    }
}
