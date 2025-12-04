using Data.SqliteContext;
using Logic.Database;
using Microsoft.EntityFrameworkCore;
using Logic.Shared.Interfaces;

namespace Core.App.StartUp
{
    internal static class Database
    {
        internal static void RegisterDatabaseServices(MauiAppBuilder builder)
        {
            builder.Services.AddDbContext<SqLiteDbContext>(opt =>
            {
                var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var dbPath = Path.Combine(folderPath, "applicationDb.db");
                opt.UseSqlite($"Data Source={dbPath}");
            });

            builder.Services.AddScoped<IDbContextFactory, DbContextFactory>();
        }

        internal static void Migrate(MauiApp app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();

                if (db.Database.GetPendingMigrations().Any())
                {
                    db.Database.Migrate();
                }
            }
        }
    }
}
