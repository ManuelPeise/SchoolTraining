using Logic.Shared.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Authentication;


namespace Service.Api.Authentication
{
    public class LoginController : ApiControllerBase
    {
        private readonly IUserAuthenticationService _authenticationService;

        public LoginController(IUserAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost(Name = "Authenticate")]
        public async Task<JwtTokenResponse?> Authenticate([FromBody] LoginModel model)
        {
            var response = await _authenticationService.SignInAsync(model);

            return response;
        }
    }
}
