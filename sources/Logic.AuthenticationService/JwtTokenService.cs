using Data.Entities;
using Logic.Database;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Logic.Shared.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Enums;
using Shared.Models.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Logic.AuthenticationService
{
    public class JwtTokenService : LogicBase, IJwtTokenService
    {
        private readonly IOptions<JwtTokenModel> _jwtOptions;
        private readonly IDbContextFactory _dbContextFactory;

        public JwtTokenService(
            IOptions<JwtTokenModel> jwtOptions, 
            IDbContextFactory dbContextFactory, 
            IHttpContextAccessor httpContextAccessor): base(httpContextAccessor)
        {
            _jwtOptions = jwtOptions;
            _dbContextFactory = dbContextFactory;
        }

        public (string Jwt, string RefreshToken) GenerateTokens(UserEntity user)
        {
            return (GenerateJwt(user), GenerateRefreshToken());
        }

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request)
        {
            var principal = GetPrincipalFromExpiredToken(request.AccessToken);
            var username = principal.Identity!.Name;

            using (var unitOfWork = new UnitOfWork(_dbContextFactory, DbContextTypeEnum.MySql))
            {
                var includes = new List<Expression<Func<UserEntity, object>>> { e => e.Credentials };

                var user = await unitOfWork.UserRepository.GetByAsync(x => x.Username == username, true, includes);
                var credentials = user?.Credentials;

                if (user == null || user?.Credentials == null || user.Credentials.RefreshToken != request.RefreshToken)
                {
                    throw new SecurityTokenException("Invalid refresh token");
                }

                var newAccessToken = GenerateJwt(user);
                var newRefreshToken = GenerateRefreshToken();

                user.Credentials.RefreshToken = newRefreshToken;

                unitOfWork.UserCredentialsRepository.Update(user.Credentials);

                await unitOfWork.SaveChangesAsync(CurrentUser?.UserName ?? "System");

                return new RefreshTokenResponse
                {
                    JwtToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                };
            }
        }

        public int GetJwtExpireSeconds()
        {
            return _jwtOptions.Value.ExpiresInSeconds;
        }

        private string GenerateJwt(UserEntity appUserEntity)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SecurityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            ClaimsService.GetUserClaims(appUserEntity);
            
            var claims = ClaimsService.GetUserClaims(appUserEntity);
            
            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Value.Issuer,
                audience: _jwtOptions.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(_jwtOptions.Value.ExpiresInSeconds),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = _jwtOptions.Value.Audience,

                ValidateIssuer = true,
                ValidIssuer = _jwtOptions.Value.Issuer,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtOptions.Value.SecurityKey)
                ),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken)
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
