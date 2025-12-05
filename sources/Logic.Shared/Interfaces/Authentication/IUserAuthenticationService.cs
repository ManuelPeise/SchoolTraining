using Shared.Models.Authentication;

namespace Logic.Shared.Interfaces.Authentication
{
    public interface IUserAuthenticationService
    {
        Task<JwtTokenResponse?> SignInAsync(LoginModel loginModel);
        Task SignOutAsync();
    }
}
