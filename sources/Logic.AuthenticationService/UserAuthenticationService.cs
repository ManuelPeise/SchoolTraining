using Data.Entities;
using Logic.Database;
using Logic.Shared;
using Logic.Shared.Helpers;
using Logic.Shared.Interfaces;
using Logic.Shared.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Shared.Enums;
using Shared.Models.Authentication;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authentication;
using Shared.Models;

namespace Logic.AuthenticationService
{
    public class UserAuthenticationService : LogicBase, IUserAuthenticationService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IJwtTokenService _jwtTokenService;

        public UserAuthenticationService(
            IDbContextFactory dbContextFactory,
            IHttpContextAccessor httpContextAccessor,
            IJwtTokenService jwtTokenService) : base(httpContextAccessor)
        {
            _dbContextFactory = dbContextFactory;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<JwtTokenResponse?> SignInAsync(LoginModel loginModel)
        {
            using (var unitOfWork = new UnitOfWork(_dbContextFactory, DbContextTypeEnum.MySql))
            {
                try
                {
                    var userEntity = await GetByUsernameAsync(unitOfWork.UserRepository, loginModel.UserName);

                    var credentialsEntity = userEntity?.Credentials;

                    if (userEntity == null || credentialsEntity == null)
                    {
                        return null;
                    }

                    var hashedSecret = SecretHelper.GetHashedSecret(loginModel.Secret, credentialsEntity.Salt);

                    if (string.IsNullOrEmpty(hashedSecret) || hashedSecret != credentialsEntity.PasswordHash)
                    {
                        return null;
                    }

                    var tokenData = _jwtTokenService.GenerateTokens(userEntity);

                    credentialsEntity.RefreshToken = tokenData.RefreshToken;
                    credentialsEntity.ExpiresAt = DateTime.UtcNow.AddSeconds(3600);

                    return new JwtTokenResponse
                    {
                        UserId = userEntity.Id,
                        Jwt = tokenData.Jwt,
                        RefreshToken = tokenData.RefreshToken,
                        ExpiresAt = DateTime.UtcNow.AddSeconds(3600).ToLocalTime().ToString("o"),
                    };

                }
                catch (Exception exception)
                {
                    await unitOfWork.LogMessageRepository.AddAsync(new LogMessageEntity
                    {
                        Message = "Error in UserAuthenticationService.LoginAsync",
                        ExeptionMessage = exception.Message,
                        StackTrace = exception?.StackTrace ?? string.Empty,
                        LogLevel = LogLevelEnum.Error,
                        TimeStamp = DateTime.UtcNow,
                    });

                    await unitOfWork.SaveChangesAsync("System");

                    return null;
                }
            }
        }

        public async Task SignOutAsync()
        {
            var context = HttpContext;

            if (context != null)
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }

        private Task<UserEntity?> GetByUsernameAsync(
            IDatabaseRepositoryBase<UserEntity> repo,
            string username,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)
        {
            if (repo == null) throw new ArgumentNullException(nameof(repo));

            var normalized = (username ?? string.Empty).Trim().ToLowerInvariant();

            Expression<Func<UserEntity, bool>> predicate = e => e.Username.ToLower() == normalized;

            var includes = new List<Expression<Func<UserEntity, object>>> { e => e.Credentials };

            return repo.GetByAsync(predicate, asNoTracking, includes, cancellationToken);
        }
    }
}
