using Google.Protobuf.WellKnownTypes;

namespace Core.Api.Bundels
{
    internal static  class AppConfiguration
    {
        internal static void Configure(WebApplication app, string corsPolicy)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseCors(corsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            Database.Migrate(app);
            Database.SeedDefaultSystemAdminUser(app);
        }
    }
}
