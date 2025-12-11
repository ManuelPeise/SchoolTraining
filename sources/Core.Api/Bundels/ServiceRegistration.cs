using Logic.Administration.DI;
using Logic.AuthenticationService;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Logic.Shared.Interfaces.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.Authentication;
using System.Text;

namespace Core.Api.Bundels
{
    internal static class ServiceRegistration
    {
        internal static void RegisterServices(WebApplicationBuilder builder, string corsPolicy)
        {
            Database.RegisterDatabaseServices(builder);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(x =>
            {
                x.AddPolicy(corsPolicy, opt =>
                {
                    opt.AllowAnyHeader();
                    opt.AllowAnyOrigin();
                    opt.AllowAnyMethod();
                });
            });
            
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication();

            ConfigureJwt(builder);
            builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            AdministrationServiceRegistration.RegisterAdministrationServices(builder.Services);
            builder.Services.AddScoped<ILogService, LogService>();

            

           
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
