using Logic.Administration.FileImport;
using Logic.Administration.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Logic.Administration.DI
{
    public static class AdministrationServiceRegistration
    {
        public static void RegisterAdministrationServices(this IServiceCollection services)
        {
            services.AddScoped<IFileImportFactory, FileImportFactory>();
            services.AddScoped<IFamilyAdministrationService, FamilyAdministrationService>();
        }
    }
}
