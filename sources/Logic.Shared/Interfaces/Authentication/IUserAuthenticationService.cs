using Shared.Models.Authentication;

namespace Logic.Shared.Interfaces.Authentication
{
    public interface IUserAuthenticationService
    {
        Task<bool> SignInAsync(LoginModel loginModel);
        Task SignOutAsync();
    }
}
