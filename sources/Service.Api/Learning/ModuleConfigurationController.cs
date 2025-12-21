using Logic.Learning.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Models.Learning;

namespace Service.Api.Learning
{
    public class ModuleConfigurationController : ApiControllerBase
    {
        private readonly IModuleConfigurationService _moduleConfigurationService;

        public ModuleConfigurationController(IModuleConfigurationService moduleConfigurationService)
        {
            _moduleConfigurationService = moduleConfigurationService;
        }

        [HttpGet(Name = "GetModuleConfigurations")]
        [JwtAuth(AllowAdmin = true, AllowSystemAdmin = true)]
        public async Task<List<Module>> GetModuleConfigurations()
        {
            return await _moduleConfigurationService.GetModuleConfiguration();
        }

        [HttpGet(Name = "GetSubModuleConfigurations")]
        [JwtAuth(AllowAdmin = true, AllowSystemAdmin = true)]
        public async Task<SubModuleConfigurationInitializationModel> GetSubModuleConfigurations()
        {
            return await _moduleConfigurationService.GetSubModuleConfigurations();
        }

        [HttpPost(Name = "SaveOrUpdateModule")]
        [JwtAuth(AllowAdmin = true, AllowSystemAdmin = true)]
        public async Task<NotificationDataResponse<List<Module>>> SaveOrUpdateModule([FromBody]Module module)
        {
            return await _moduleConfigurationService.SaveOrUpdateModule(module);
        }

        [HttpPost(Name = "SaveOrUpdateSubModule")]
        [JwtAuth(AllowAdmin = true, AllowSystemAdmin = true)]
        public async Task<NotificationDataResponse<SubModuleConfigurationInitializationModel>> SaveOrUpdateSubModule([FromBody] SubModule subModule)
        {
            return await _moduleConfigurationService.SaveOrUpdateSubModule(subModule);
        }

        [HttpPost(Name = "DeleteModule")]
        [JwtAuth(AllowAdmin = true, AllowSystemAdmin = true)]
        public async Task<NotificationDataResponse<List<Module>>> DeleteModule([FromQuery] int moduleId)
        {
            return await _moduleConfigurationService.DeleteModule(moduleId);
        }

        [HttpPost(Name = "DeleteSubModule")]
        [JwtAuth(AllowAdmin = true, AllowSystemAdmin = true)]
        public async Task<NotificationDataResponse<SubModuleConfigurationInitializationModel>> DeleteSubModule([FromQuery] int subModuleId)
        {
            return await _moduleConfigurationService.DeleteSubModule(subModuleId);
        }
    }
}
