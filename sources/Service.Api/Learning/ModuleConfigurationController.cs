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

        [HttpGet(Name = "GetModuleConfiguration")]
        [JwtAuth(AllowAdmin = true, AllowSystemAdmin = true)]
        public async Task<NotificationDataResponse<ModuleInitializationModel>> GetModuleConfigurationAsync()
        {
            return await _moduleConfigurationService.GetModuleConfigurationAsync();
        }

        [HttpPost(Name = "SaveOrUpdateModule")]
        [JwtAuth(AllowAdmin = true, AllowSystemAdmin = true)]
        public async Task<NotificationDataResponse<List<Module>>> SaveOrUpdateModule([FromBody]Module module)
        {
            return await _moduleConfigurationService.SaveOrUpdateModule(module);
        }

        [HttpPost(Name = "SaveOrUpdateSubModule")]
        [JwtAuth(AllowAdmin = true, AllowSystemAdmin = true)]
        public async Task<NotificationDataResponse<List<SubModule>>> SaveOrUpdateSubModule([FromBody] SubModule subModule)
        {
            return await _moduleConfigurationService.SaveOrUpdateSubModule(subModule);
        }

        [HttpPost(Name = "DeleteModule")]
        [JwtAuth(AllowAdmin = true, AllowSystemAdmin = true)]
        public async Task<NotificationDataResponse<List<Module>>> DeleteModule([FromBody] string idExternal)
        {
            return await _moduleConfigurationService.DeleteModule(idExternal);
        }
    }
}
