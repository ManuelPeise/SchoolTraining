using Core.App.StartUp;
using Microsoft.Extensions.Logging;

namespace Core.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            Database.RegisterDatabaseServices(builder);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            // Database.Migrate(app);

            return app;
        }
    }
}
