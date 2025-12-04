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
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Logic.AuthenticationService
{
    public class UserAuthenticationService : LogicBase, IUserAuthenticationService
    {
        private readonly IDbContextFactory _dbContextFactory;
        public UserAuthenticationService(IDbContextFactory dbContextFactory, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<bool> SignInAsync(LoginModel loginModel)
        {
            using (var unitOfWork = new UnitOfWork(_dbContextFactory, DbContextTypeEnum.MySql))
            {
                try
                {
                    var userEntity = await GetByUsernameAsync(unitOfWork.UserRepository, loginModel.UserName);

                    var credentialsEntity = userEntity?.Credentials;

                    if (userEntity == null || credentialsEntity == null)
                    {
                        return await Task.FromResult(false);
                    }

                    var hashedSecret = SecretHelper.GetHashedSecret(credentialsEntity.PasswordHash, credentialsEntity.Salt);

                    if (string.IsNullOrEmpty(hashedSecret) || hashedSecret != credentialsEntity.PasswordHash)
                    {
                        return await Task.FromResult(false);
                    }

                    var context = HttpContext;

                    if (context == null)
                    {
                        return await Task.FromResult(false);
                    }

                    var claims = GetUserClaims(userEntity);

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                    {
                        IsPersistent = false,
                        AllowRefresh = false
                    });

                    return await Task.FromResult(true);
                }
                catch (Exception exception)
                {
                    await unitOfWork.LogMessageRepository.AddAsync(new LogMessageEntity
                    {
                        Message = "Error in UserAuthenticationService.LoginAsync",
                        ExeptionMessage = exception.Message,
                        StackTrace = exception?.StackTrace ?? string.Empty,
                        LogLevel = LogLevelEnum.Error
                    });

                    await unitOfWork.SaveChangesAsync("System");

                    return await Task.FromResult(false);
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

        private List<Claim> GetUserClaims(UserEntity userEntity)
        {
            return new List<Claim>
            {
                new Claim(UserClaimConstants.UserNameKey, userEntity.Username),
                new Claim(UserClaimConstants.UserIdKey, userEntity.Id.ToString()),
                new Claim(UserClaimConstants.UserRoleKey, userEntity.UserRole.ToString()),
                new Claim(UserClaimConstants.SessionExpireTime, DateTime.UtcNow.AddHours(1).ToString("o"))
            };
        }

        private Expression<Func<UserEntity, object>> IncludeSecretExpression = e => e.Credentials;
        #region dispose

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
