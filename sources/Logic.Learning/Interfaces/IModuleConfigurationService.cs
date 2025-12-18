using Shared.Models;
using Shared.Models.Learning;

namespace Logic.Learning.Interfaces
{
    public interface IModuleConfigurationService
    {
        Task<NotificationDataResponse<ModuleInitializationModel>> GetModuleConfigurationAsync();
        Task<NotificationDataResponse<List<Module>>> SaveOrUpdateModule(Module module);
        Task<NotificationDataResponse<List<SubModule>>> SaveOrUpdateSubModule(SubModule subModule);
        Task<NotificationDataResponse<List<Module>>> DeleteModule(int id);
    }
}
