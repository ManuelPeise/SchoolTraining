using Core.Web.Bundles;
using Core.Web.Components.Pages.ViewModels;
using Core.Web.Providers;
using Logic.AuthenticationService;
using Logic.Shared.Interfaces.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.Authentication;
using System.Text;

namespace Core.Web.StartUp
{
    public static class ServiceRegistration
    {
        public static void Register(WebApplicationBuilder builder)
        {
            Database.RegisterDatabaseServices(builder);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddRazorComponents().AddInteractiveServerComponents();
            
            // Add controllers for API endpoints
            builder.Services.AddControllers();

            // Authentication and Authorization
            ConfigureJwt(builder);

            // WICHTIG: CustomAuthenticationStateProvider als AuthenticationStateProvider registrieren
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication();

            // Authentication Service
            builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

            // ViewModels
            // Register view models as scoped so the same instance is used for the component lifecycle
            builder.Services.AddScoped<CounterViewModel>();
            builder.Services.AddScoped<AuthenticationViewModel>();
        }

        private static void ConfigureJwt(WebApplicationBuilder builder)
        {
            builder.Services.Configure<JwtTokenModel>(builder.Configuration.GetSection("Jwt"));

            var jwtConfig = builder.Configuration.GetSection("Jwt").Get<JwtTokenModel>();

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    if (jwtConfig == null || string.IsNullOrEmpty(jwtConfig.SecurityKey))
                    {
                        throw new ArgumentNullException("SecurityKey is not set!");
                    }

                    var key = jwtConfig?.SecurityKey ?? string.Empty;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = true,
                        ValidAudience = jwtConfig?.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(key)),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

    }
}
