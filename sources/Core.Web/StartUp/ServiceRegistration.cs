using Core.Web.Bundles;
using Logic.AuthenticationService;
using Logic.Shared.Interfaces.Authentication;
using Core.Web.ViewModels;

namespace Core.Web.StartUp
{
    public static class ServiceRegistration
    {
        public static void Register(WebApplicationBuilder builder)
        {
            Database.RegisterDatabaseServices(builder);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddRazorComponents().AddInteractiveServerComponents();
            // Authentication Service
            builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();

            // ViewModels
            builder.Services.AddTransient<CounterViewModel>();
        }
    }
}
