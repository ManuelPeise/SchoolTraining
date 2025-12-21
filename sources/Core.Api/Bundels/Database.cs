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

                var databaseIsChanged = false;
                var timeStamp = DateTime.UtcNow;

                if (!db.UserTable.Any(x => x.UserRole == UserRoleEnum.SystemAdmin))
                {
                    var salt = Guid.NewGuid().ToString();

                    var defaultSystemAdminUser = new UserEntity
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

                    db.UserTable.Add(defaultSystemAdminUser);

                    databaseIsChanged = true;
                }

                if (!db.UserTable.Any(x => x.UserRole == UserRoleEnum.Admin))
                {
                    var salt = Guid.NewGuid().ToString();

                    var defaultAdminUser = new UserEntity
                    {
                        Id = 2,
                        IdExternal = "18338480-8150-4f53-9358-d11b0f1fd9ee",
                        FirstName = "Family",
                        LastName = "Admin",
                        Username = "Family.Admin",
                        DateOfBirth = DateTime.Parse("1980-04-20"),
                        UserRole = UserRoleEnum.Admin,
                        Credentials = new UserCredentialsEntity
                        {
                            Id = 2,
                            Salt = salt,
                            PasswordHash = SecretHelper.GetPasswordHash("Pass@word", salt),
                            CreatedAt = timeStamp,
                            CreatedBy = "System",
                        },
                        CreatedAt = timeStamp,
                        CreatedBy = "System",
                        Family = new FamilyEntity
                        {
                            Id = 1,
                            Name = "Default Admin Family",
                            IsActive = true,
                            CreatedAt = timeStamp,
                            CreatedBy = "System",
                        },
                        Settings = new UserSettingsEntity
                        {
                            Id = 2,
                        }
                    };

                    db.UserTable.Add(defaultAdminUser);
                }

                if (databaseIsChanged)
                {
                    db.SaveChanges();
                }
            }
        }
    }
}
