using Shared.Models;
using Shared.Models.Learning;

namespace Logic.Learning.Interfaces
{
    public interface IModuleConfigurationService
    {
        Task<List<Module>> GetModuleConfiguration();
        Task<SubModuleConfigurationInitializationModel> GetSubModuleConfigurations();
        Task<NotificationDataResponse<List<Module>>> SaveOrUpdateModule(Module module);
        Task<NotificationDataResponse<SubModuleConfigurationInitializationModel>> SaveOrUpdateSubModule(SubModule subModule);
        Task<NotificationDataResponse<List<Module>>> DeleteModule(int id);
        Task<NotificationDataResponse<SubModuleConfigurationInitializationModel>> DeleteSubModule(int id);
    }
}
