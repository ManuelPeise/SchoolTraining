using Logic.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using Shared.Models.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Core.Web.Providers
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        
        private readonly IJSRuntime _jsRuntime;
        private const string TokenKey = "authToken";

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            try
            {
                var identity = GetClaimsIdentity(token);
                var claims = identity.Claims;

                var expClaim = claims.FirstOrDefault(c => c.Type == UserClaimConstants.SessionExpireTime);
                if (expClaim != null && long.TryParse(expClaim.Value, out long exp))
                {
                    var expirationTime = DateTimeOffset.FromUnixTimeSeconds(exp);
                    
                    if (expirationTime < DateTimeOffset.UtcNow)
                    {
                        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
                        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                    }
                }

               
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task<bool> AuthenticateAsync(JwtTokenResponse? response)
        {
            if (response == null || string.IsNullOrEmpty(response.Jwt))
            {
                return false;
            }

            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, response.Jwt);

            var identity = GetClaimsIdentity(response.Jwt);
            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);
            
            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return true;
        }

        public async Task LogoutAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            var anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            NotifyAuthenticationStateChanged(Task.FromResult(anonymous));
        }

        private ClaimsIdentity GetClaimsIdentity(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var claims = jwtToken.Claims;

            return new ClaimsIdentity(claims, "jwt");
        }
    }
}
