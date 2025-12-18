using Data.Entities;
using Data.MySqlContext;
using Data.Testing;
using Logic.Database;
using Logic.Shared.Helpers;
using Logic.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace Core.Api.Bundels
{
    internal static class Database
    {
        internal static void RegisterDatabaseServices(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<MySqlDbContext>(opt =>
            {
                var connectionString = builder.Configuration.GetConnectionString("SchoolAppDb");

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException(nameof(connectionString));
                }

                opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            builder.Services.AddDbContext<TestMySqlDbContext>(opt =>
            {
                var connectionString = builder.Configuration.GetConnectionString("TestDb");

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException(nameof(connectionString));
                }

                opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            builder.Services.AddScoped<IDbContextFactory, DbContextFactory>();
        }

        internal static void Migrate(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MySqlDbContext>();

                if (db.Database.GetPendingMigrations().Any())
                {
                    db.Database.Migrate();
                }
            }
        }

        internal static void SeedDefaultSystemAdminUser(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MySqlDbContext>();

                if (!db.UserTable.Any(x => x.UserRole == UserRoleEnum.SystemAdmin))
                {
                    var timeStamp = DateTime.UtcNow;

                    var salt = Guid.NewGuid().ToString();

                    var defaultAdminUser = new UserEntity
                    {
                        Id = 1,
                        IdExternal = "045a9bd4-06f9-4a55-8ab3-2dc1d692706e",
                        FirstName = "System",
                        LastName = "Admin",
                        Username = "System.Admin",
                        DateOfBirth = DateTime.Parse("1980-04-20"),
                        UserRole = UserRoleEnum.SystemAdmin,
                        Credentials = new UserCredentialsEntity
                        {
                            Id = 1,
                            Salt = salt,
                            PasswordHash = SecretHelper.GetPasswordHash("Pass@word", salt),
                            CreatedAt = timeStamp,
                            CreatedBy = "System",
                        },
                        CreatedAt = timeStamp,
                        CreatedBy = "System",
                        Settings = new UserSettingsEntity
                        {
                            Id = 1,
                        }
                    };

                    db.UserTable.Add(defaultAdminUser);
                    db.SaveChanges();
                }
            }
        }
    }
}
