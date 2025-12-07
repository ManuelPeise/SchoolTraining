using Core.Web.Bundles;
using Core.Web.Components;

namespace Core.Web.StartUp
{
    public static class Configuration
    {
        public static void Configure(WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.UseStaticFiles();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapRazorComponents<App>()
               .AddInteractiveServerRenderMode();

            Database.Migrate(app);
            Database.SeedDefaultSystemAdminUser(app);
        }
    }
}
