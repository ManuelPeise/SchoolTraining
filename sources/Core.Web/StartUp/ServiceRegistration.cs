using Core.Web.Bundles;
using Logic.AuthenticationService;
using Logic.Shared.Interfaces.Authentication;
using Core.Web.Components.Pages.ViewModels;

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
            // Register view models as scoped so the same instance is used for the component lifecycle
            builder.Services.AddScoped<CounterViewModel>();
            builder.Services.AddScoped<AuthenticationViewModel>();
        }
    }
}
