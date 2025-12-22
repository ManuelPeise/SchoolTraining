using Data.Entities;
using Logic.Administration.Interfaces;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Shared.Enums;
using Shared.Models.Administration;


namespace Logic.Administration
{
    public class UserRightService : IUserRightService
    {
        private readonly IAdministrationUnitOfWork _administrationUnitOfWork;

        public UserRightService(IAdministrationUnitOfWork administrationUnitOfWork)
        {
            _administrationUnitOfWork = administrationUnitOfWork;
        }

        public async Task<List<UserRight>> GetUserRights(int userId)
        {
            try
            {
                var userEntity = await _administrationUnitOfWork.UserRepository.GetByIdAsync(userId, false, IncludeExpressions.IncludeRights);

                await _administrationUnitOfWork.UserRightRepository.GetAllByAsync(ur => ur.UserId == userId, false, IncludeExpressions.IncludeUserRights);

                if (userEntity == null || !userEntity.UserRights.Any())
                {
                    await _administrationUnitOfWork.LogMessage(new LogMessageEntity
                    {
                        Message = "User not found or has no rights",
                        TimeStamp = DateTime.UtcNow,
                        LogLevel = LogLevelEnum.Error
                    });

                    return new List<UserRight>();
                }

                var userRights = userEntity.UserRights.Select(ur => new UserRight
                {
                    RightGuid = ur.Right.RightGuid,
                    Name = ur.Right.Name,
                    NameResourceKey = ur.Right.NameResourceKey,
                    DescriptionResourceKey = ur.Right.DescriptionResourceKey,
                    IsActive = ur.Right.IsActive,
                    Deny = ur.Deny,
                    CanView = ur.CanView,
                    CanCreate = ur.CanCreate,
                    CanEdit = ur.CanEdit,
                    CanDelete = ur.CanDelete
                }).ToList();

                return userRights;
            }
            catch (Exception exception)
            {
                await _administrationUnitOfWork.LogMessage(new LogMessageEntity
                {
                    Message = "Error in GetUserRights",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception.StackTrace,
                    TimeStamp = DateTime.UtcNow,
                    LogLevel = LogLevelEnum.Error
                });

                return new List<UserRight>();
            }
        }
    }
}
