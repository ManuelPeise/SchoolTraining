using Data.Entities;
using Data.Entities.Learning;
using Logic.Learning.Interfaces;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models;
using Shared.Models.Learning;

namespace Logic.Learning
{
    public class ModuleConfigurationService : LogicBase, IModuleConfigurationService
    {
        private readonly ILearningUnitOfWork _unitOfWork;

        public ModuleConfigurationService(
            ILearningUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Module>> GetModuleConfiguration()
        {
            try
            {
                var modules = await GetModulesAsync();

                return modules;

            }
            catch (Exception exception)
            {

                await LogError("A error occurred while loading module configuration initialization data from database.", exception);

                return new List<Module>();
            }
        }

        public async Task<SubModuleConfigurationInitializationModel> GetSubModuleConfigurations()
        {
            try
            {
                return await GetSubModuleConfigurationResponseModel();
            }
            catch (Exception exception)
            {
                await LogError("A error occurred while loading sub module initialization data from database.", exception);

                return new SubModuleConfigurationInitializationModel
                {
                    ParentModuleDropdownItems = new List<DropdownItem>(),
                    SubModuleDataCollection = new List<SubModuleDataCollection>()
                };
            }
        }

        public async Task<NotificationDataResponse<List<Module>>> SaveOrUpdateModule(Module module)
        {
            try
            {
                var isDatabaseChanged = false;

                if (IsNewModel(module.IdExternal))
                {
                    await _unitOfWork.ModuleRepository.AddAsync(new ModuleEntity
                    {
                        IdExternal = Guid.NewGuid().ToString(),
                        FamilyId = CurrentUser.FamilyId,
                        Title = module.Title,
                        Description = module.Description,
                    });

                    await LogInfo($"New module '{module.Title}' is created by user '{CurrentUser.UserName}'.");

                    isDatabaseChanged = true;
                }
                else
                {
                    var existingEntity = await _unitOfWork.ModuleRepository.GetByAsync(e => e.IdExternal == module.IdExternal);

                    if (existingEntity != null)
                    {
                        existingEntity.Title = module.Title;
                        existingEntity.Description = module.Description;
                        existingEntity.UpdatedAt = DateTime.UtcNow;
                        existingEntity.UpdatedBy = CurrentUser.UserName;

                        await LogInfo($"Module '{module.Title}' is updated by user '{CurrentUser.UserName}'.");

                        isDatabaseChanged = true;
                    }
                }

                if (isDatabaseChanged)
                {
                    await _unitOfWork.SaveChangesAsync(CurrentUser.UserName);
                }

                return new NotificationDataResponse<List<Module>>
                {
                    Success = true,
                    ResourceKey = "common.savingOrUpdatingModuleSuccess",
                    Data = await GetModulesAsync()
                };
            }
            catch (Exception exception)
            {
                await LogError("A error occurred while saving or updating a module.", exception);

                return new NotificationDataResponse<List<Module>>
                {
                    Success = false,
                    ResourceKey = "common.errorWhileSavingOrUpdatingModule",
                    Data = await GetModulesAsync()
                };
            }
        }

        public async Task<NotificationDataResponse<SubModuleConfigurationInitializationModel>> SaveOrUpdateSubModule(SubModule subModule)
        {
            try
            {
                var isDatabaseChanged = false;

                if (IsNewModel(subModule.IdExternal))
                {
                    await _unitOfWork.SubModuleRepository.AddAsync(new SubModuleEntity
                    {
                        IdExternal = Guid.NewGuid().ToString(),
                        FamilyId = CurrentUser.FamilyId,
                        ModuleId = subModule.ModuleId,
                        Title = subModule.Title,
                        Description = subModule.Description,
                        VocabularyDirection = subModule.Direction,
                        Units = new List<UnitEntity>()
                    });

                    await LogInfo($"New sub module '{subModule.Title}' is created by user '{CurrentUser.UserName}'.");

                    isDatabaseChanged = true;
                }
                else
                {
                    var existingEntity = await _unitOfWork.SubModuleRepository.GetByAsync(e => e.IdExternal == subModule.IdExternal);

                    if (existingEntity != null)
                    {
                        existingEntity.Title = subModule.Title;
                        existingEntity.Description = subModule.Description;
                        existingEntity.VocabularyDirection = subModule.Direction;
                        existingEntity.Units = existingEntity.Units ?? new List<UnitEntity>();

                        await LogInfo($"Sub module '{subModule.Title}' is updated by user '{CurrentUser.UserName}'.");

                        isDatabaseChanged = true;
                    }
                }

                if (isDatabaseChanged)
                {
                    await _unitOfWork.SaveChangesAsync(CurrentUser.UserName);
                }

                return new NotificationDataResponse<SubModuleConfigurationInitializationModel>
                {
                    Success = true,
                    ResourceKey = "common.savingOrUpdatingSubModuleSuccess",
                    Data = await GetSubModuleConfigurationResponseModel()
                };
            }
            catch (Exception exception)
            {
                await LogError("A error occurred while saving or updating a sub module.", exception);

                return new NotificationDataResponse<SubModuleConfigurationInitializationModel>
                {
                    Success = false,
                    ResourceKey = "common.errorWhileSavingOrUpdatingSubModule",
                    Data = await GetSubModuleConfigurationResponseModel()
                };
            }
        }

        public async Task<NotificationDataResponse<List<Module>>> DeleteModule(int id)
        {
            try
            {
                var existingEntity = await _unitOfWork.ModuleRepository.GetByAsync(e => e.Id == id);

                if (existingEntity != null)
                {
                    _unitOfWork.ModuleRepository.Remove(existingEntity);

                    await _unitOfWork.SaveChangesAsync(CurrentUser.UserName);

                    await LogInfo($"Module '{existingEntity.Title}' is deleted by user '{CurrentUser.UserName}'.", true);

                    return new NotificationDataResponse<List<Module>>
                    {
                        Success = true,
                        ResourceKey = "common.deleteModuleSuccess",
                        Data = await GetModulesAsync()
                    };
                }

                await LogError("A error occurred while deleting a module.", null);

                return new NotificationDataResponse<List<Module>>
                {
                    Success = false,
                    ResourceKey = "common.errorWhileDeletingModule",
                    Data = await GetModulesAsync()
                };
            }
            catch (Exception exception)
            {
                await LogError("A error occurred while deleting a module.", exception);

                return new NotificationDataResponse<List<Module>>
                {
                    Success = false,
                    ResourceKey = "common.errorWhileDeletingModule",
                    Data = await GetModulesAsync()
                };
            }
        }

        public async Task<NotificationDataResponse<SubModuleConfigurationInitializationModel>> DeleteSubModule(int id)
        {
            try
            {
                var existingEntity = await _unitOfWork.SubModuleRepository.GetByAsync(e => e.Id == id);

                if (existingEntity != null)
                {
                    _unitOfWork.SubModuleRepository.Remove(existingEntity);

                    await _unitOfWork.SaveChangesAsync(CurrentUser.UserName);

                    await LogInfo($"Sub module '{existingEntity.Title}' is deleted by user '{CurrentUser.UserName}'.", true);

                    return new NotificationDataResponse<SubModuleConfigurationInitializationModel>
                    {
                        Success = true,
                        ResourceKey = "common.deleteSubModuleSuccess",
                        Data = await GetSubModuleConfigurationResponseModel()
                    };
                }

                await LogError("A error occurred while deleting a sub module.", null);

                return new NotificationDataResponse<SubModuleConfigurationInitializationModel>
                {
                    Success = false,
                    ResourceKey = "common.errorWhileDeletingSubModule",
                    Data = await GetSubModuleConfigurationResponseModel()
                };
            }
            catch (Exception exception)
            {
                await LogError("A error occurred while deleting a sub module.", exception);

                return new NotificationDataResponse<SubModuleConfigurationInitializationModel>
                {
                    Success = false,
                    ResourceKey = "common.errorWhileDeletingSubModule",
                    Data = await GetSubModuleConfigurationResponseModel()
                };
            }
        }

        private async Task<List<Module>> GetModulesAsync()
        {
            var entities = await _unitOfWork.ModuleRepository
                .GetAllByAsync(e => e.FamilyId == CurrentUser.FamilyId, true, IncludeExpressions.IncludeSubModules);

            return entities?.Select(e => new Module
            {
                ModuleId = e.Id,
                IdExternal = e.IdExternal,
                SubModules = e.SubModules?.Select(sm => new SubModule
                {
                    SubModuleId = sm.Id,
                    IdExternal = sm.IdExternal,
                    ModuleId = sm.ModuleId,
                    Module = new(),
                    Title = sm.Title,
                    Description = sm.Description,
                    Direction = sm.VocabularyDirection,
                    LastUpdateBy = GetLastUpdateBy(sm.UpdatedBy, sm.CreatedBy),
                    LastUpdateAt = GetLastUpdateAt(sm.UpdatedAt, sm.CreatedAt)
                }).ToList() ?? new List<SubModule>(),
                Title = e.Title,
                Description = e.Description,
                LastUpdateBy = GetLastUpdateBy(e.UpdatedBy, e.CreatedBy),
                LastUpdateAt = GetLastUpdateAt(e.UpdatedAt, e.CreatedAt)
            }).ToList() ?? new List<Module>();
        }

        private async Task<List<SubModuleEntity>> GetSubModulesAsync()
        {
            var entities = await _unitOfWork.SubModuleRepository
                .GetAllByAsync(e => e.FamilyId == CurrentUser.FamilyId, false, IncludeExpressions.IncludeModule);

            return entities?.ToList() ?? new List<SubModuleEntity>();
        }

        private async Task<SubModuleConfigurationInitializationModel> GetSubModuleConfigurationResponseModel()
        {
            var modules = await GetModulesAsync();

            var parentModuleDropdownItems = modules.Select(m => new DropdownItem
            {
                Id = m.ModuleId,
                Label = m.Title
            }).ToList();

            var subModuleEntities = await GetSubModulesAsync();

            var response = new SubModuleConfigurationInitializationModel
            {
                ParentModuleDropdownItems = parentModuleDropdownItems,
                Modules = modules,
                SubModuleDataCollection = subModuleEntities != null ?
                (from entity in subModuleEntities
                 select new SubModuleDataCollection
                 {
                     ModuleId = entity.ModuleId,
                     SubModules = entity.Module != null ?
                         (from m in entity.Module.SubModules
                          select new SubModuleBase
                          {
                              ModuleId = m.ModuleId,
                              IdExternal = m.IdExternal,
                              SubModuleId = m.Id,
                              Title = m.Title,
                              Description = m.Description,
                              Direction = m.VocabularyDirection,
                              LastUpdateAt = GetLastUpdateAt(m.UpdatedAt, m.CreatedAt),
                              LastUpdateBy = GetLastUpdateBy(m.UpdatedBy, m.CreatedBy)
                          }).ToList() : new List<SubModuleBase>(),
                     SubModuleDropdownItems = entity.Module != null ?
                     (from m in entity.Module.SubModules
                      select new DropdownItem
                      {
                          Id = m.Id,
                          Label = m.Title
                      }).ToList()
                     : new List<DropdownItem>()
                 }).ToList() : new List<SubModuleDataCollection>()
            };

            return response;
        }

        private bool IsNewModel(string idExternal)
        {
            return string.IsNullOrEmpty(idExternal);
        }

        private string GetLastUpdateAt(DateTime? updetedAt, DateTime createdAt)
        {
            return updetedAt == null || updetedAt == DateTime.MinValue ? createdAt.ToString("dd.MM.yyyy HH:mm") : updetedAt.Value.ToString("dd.MM.yyyy HH:mm");
        }

        private string GetLastUpdateBy(string? updetedAt, string createdAt)
        {
            return string.IsNullOrEmpty(updetedAt) ? createdAt : updetedAt;
        }

        private async Task LogInfo(string message, bool save = false)
        {
            await _unitOfWork.LogMessage(new LogMessageEntity
            {
                Message = message,
                Module = nameof(ModuleConfigurationService),
                LogLevel = LogLevelEnum.Error,
                TimeStamp = DateTime.UtcNow
            }, save, CurrentUser.FamilyId, CurrentUser.UserName);
        }

        private async Task LogError(string message, Exception? exception, bool save = true)
        {
            await _unitOfWork.LogMessage(new LogMessageEntity
            {
                Message = message,
                ExeptionMessage = exception?.Message ?? string.Empty,
                StackTrace = exception?.StackTrace,
                Module = nameof(ModuleConfigurationService),
                LogLevel = LogLevelEnum.Error,
                TimeStamp = DateTime.UtcNow
            }, save, CurrentUser.FamilyId, CurrentUser.UserName);
        }
    }
}
