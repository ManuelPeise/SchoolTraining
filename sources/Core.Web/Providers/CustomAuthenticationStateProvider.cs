using Logic.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Shared.Enums;
using Shared.Models.Authentication;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;

namespace Core.Web.Providers
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        
        private readonly IJSRuntime _jsRuntime;
        private const string TokenKey = "authToken";
        private AuthenticationState _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        
        public CurrentUser _currentUser { get; set; } = new();

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);

            if (string.IsNullOrEmpty(token))
            {
                ResetCurrentUser();
                return _anonymous;
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
                        ResetCurrentUser();
                        return _anonymous;
                    }
                }

                // Map claims to CurrentUser
                SetCurrentUserFromClaims(identity);
                
                var user = new ClaimsPrincipal(identity);
                
                return new AuthenticationState(user);
            }
            catch
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
                ResetCurrentUser();
                return _anonymous;
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
            // Map claims to CurrentUser
            SetCurrentUserFromClaims(identity);

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);
            
            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return true;
        }

        public async Task LogoutAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            ResetCurrentUser();
           
            
            NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
        }

        public CurrentUser GetCurrentUser()
        {
            return _currentUser;
        }

        private ClaimsIdentity GetClaimsIdentity(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var claims = jwtToken.Claims;

            return new ClaimsIdentity(claims, "jwt");
        }

        private void SetCurrentUserFromClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToDictionary(c => c.Type, c => c.Value);

            if (claims.TryGetValue(UserClaimConstants.UserIdKey, out var idStr) && int.TryParse(idStr, out var id))
            {
                _currentUser.UserId = id;
            }
            else
            {
                _currentUser.UserId = 0;
            }

            _currentUser.UserName = claims.TryGetValue(UserClaimConstants.UserNameKey, out var name) ? name ?? string.Empty : string.Empty;

            if (claims.TryGetValue(UserClaimConstants.UserRoleKey, out var roleStr) && Enum.TryParse<UserRoleEnum>(roleStr, true, out var role))
            {
                _currentUser.UserRole = role;
            }
            else
            {
                _currentUser.UserRole = default;
            }
        }

        private void ResetCurrentUser()
        {
            _currentUser.UserId = 0;
            _currentUser.UserName = string.Empty;
            _currentUser.UserRole = default;
        }
    }
}
