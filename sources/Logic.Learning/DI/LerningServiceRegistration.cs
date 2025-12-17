using Logic.Learning.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Logic.Learning.DI
{
    public static class LerningServiceRegistration
    {
        public static void RegisterLearningServices(this IServiceCollection services)
        {
            services.AddTransient<IModuleConfigurationService, ModuleConfigurationService>();
        }
    }
}
