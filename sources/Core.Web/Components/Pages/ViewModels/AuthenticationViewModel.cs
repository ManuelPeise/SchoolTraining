using Core.Web.ViewModels;
using Logic.Shared.Interfaces.Authentication;
using Microsoft.AspNetCore.Components;
using Shared.Models.Authentication;

namespace Core.Web.Components.Pages.ViewModels
{
    public class AuthenticationViewModel : ViewModelBase
    {
        private readonly IUserAuthenticationService _authenticationService;
        private readonly NavigationManager _navigationManager;

        public LoginModel LoginModel { get; set; } = new LoginModel();


        public AuthenticationViewModel(IUserAuthenticationService authenticationService, NavigationManager navigationManager)
        {
            _authenticationService = authenticationService;
            _navigationManager = navigationManager;
        }

        public async Task SignInAsync()
        {
            var success = await _authenticationService.SignInAsync(LoginModel);

            if (success)
            {
               _navigationManager.NavigateTo("/home", forceLoad: true);
    
            }
            else
            {
                _navigationManager.NavigateTo("/home", forceLoad: true);
            }
        }
    }
}
