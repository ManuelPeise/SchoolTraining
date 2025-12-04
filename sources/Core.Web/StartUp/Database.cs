using Data.MySqlContext;
using Logic.Database;
using Logic.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Web.Bundles
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

