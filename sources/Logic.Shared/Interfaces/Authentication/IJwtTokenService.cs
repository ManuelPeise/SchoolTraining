using Data.Entities;
using Shared.Models.Authentication;

namespace Logic.Shared.Interfaces.Authentication
{
    public interface IJwtTokenService
    {
        (string Jwt, string RefreshToken) GenerateTokens(UserEntity user);
        Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request);
        int GetJwtExpireSeconds();
    }
}
