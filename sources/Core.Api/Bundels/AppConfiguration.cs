using Google.Protobuf.WellKnownTypes;
using ZstdSharp.Unsafe;

namespace Core.Api.Bundels
{
    internal static  class AppConfiguration
    {
        internal static async Task Configure(WebApplication app, string corsPolicy)
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

            app.UseMiddleware<SchedulerMiddleWare>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

           

            Database.Migrate(app);
            
            await Scheduler.StartScheduler(app.Services);
        }
    }
}
