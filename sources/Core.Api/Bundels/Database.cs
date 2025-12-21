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
    }
}
