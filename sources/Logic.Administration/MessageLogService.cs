using Data.Entities;
using Logic.Administration.Extensions;
using Logic.Administration.Interfaces;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models.Administration;

namespace Logic.Administration
{
    public class MessageLogService : LogicBase, IMessageLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MessageLogService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork) : base(httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<LogMessage>> GetMessageLogs()
        {
            try
            {
                var logMessageEntities = await _unitOfWork.LogMessageRepository.GetAllAsync(true);

                if (logMessageEntities == null)
                {
                    return new List<LogMessage>();
                }

                return GetMessages(logMessageEntities);
            }
            catch (Exception exception)
            {
                await _unitOfWork.LogMessage(new LogMessageEntity
                {
                    Message = "Could not load log messages",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception.StackTrace,
                    Module = nameof(MessageLogService),
                    LogLevel = LogLevelEnum.Warning,
                    TimeStamp = DateTime.UtcNow,
                }, true);


                return new List<LogMessage>();
            }
        }

        public async Task<List<LogMessage>> DeleteLogMessage(int id)
        {
            try
            {
                var logMessageEntity = await _unitOfWork.LogMessageRepository.GetByIdAsync(id, true);

                if (logMessageEntity == null)
                {
                    await _unitOfWork.LogMessage(new LogMessageEntity
                    {
                        Message = $"Could not delete log message with id [{id}]",
                        ExeptionMessage = string.Empty,
                        StackTrace = string.Empty,
                        Module = nameof(MessageLogService),
                        LogLevel = LogLevelEnum.Error,
                        TimeStamp = DateTime.UtcNow,
                    }, true);

                    return GetMessages(new List<LogMessageEntity>());
                }

                _unitOfWork.LogMessageRepository.Remove(logMessageEntity);

                await _unitOfWork.SaveChangesAsync(CurrentUser.UserName);

                return GetMessages(await _unitOfWork.LogMessageRepository.GetAllAsync());
            }
            catch (Exception exception)
            {
                await _unitOfWork.LogMessage(new LogMessageEntity
                {
                    Message = $"Could not delete log message [{id}]",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception.StackTrace,
                    Module = nameof(MessageLogService),
                    LogLevel = LogLevelEnum.Warning,
                    TimeStamp = DateTime.UtcNow,
                }, true);

                return GetMessages(new List<LogMessageEntity>());
            }
        }

        public async Task<List<LogMessage>> DeleteLogMessages(LogLevelEnum? logLevel)
        {
            try
            {
                var isDatabaseChanged = false;
                var currentTimeStamp = DateTime.UtcNow;

                var logMessageEntities = logLevel != null ?
                    await _unitOfWork.LogMessageRepository
                    .GetAllByAsync(msg => msg.TimeStamp < currentTimeStamp.AddDays(-30) && msg.LogLevel == logLevel, true) :
                    await _unitOfWork.LogMessageRepository
                    .GetAllByAsync(msg => msg.TimeStamp < currentTimeStamp.AddDays(-30), true);

                if (logMessageEntities == null || !logMessageEntities.Any())
                {
                    return GetMessages(new List<LogMessageEntity>());
                }

                foreach (var logMessageEntity in logMessageEntities)
                {
                    _unitOfWork.LogMessageRepository.Remove(logMessageEntity);
                    isDatabaseChanged = true;
                }

                if (isDatabaseChanged)
                {
                    await _unitOfWork.SaveChangesAsync(CurrentUser.UserName);
                }

                return GetMessages(await _unitOfWork.LogMessageRepository.GetAllAsync());
            }
            catch (Exception exception)
            {
                await _unitOfWork.LogMessage(new LogMessageEntity
                {
                    Message = "Could not delete log messages",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception.StackTrace,
                    Module = nameof(MessageLogService),
                    LogLevel = LogLevelEnum.Warning,
                    TimeStamp = DateTime.UtcNow,
                }, true);

                return GetMessages(new List<LogMessageEntity>());
            }
        }

        private List<LogMessage> GetMessages(List<LogMessageEntity> entities)
        {
            return !entities.Any() ?
                new List<LogMessage>() :
                entities.Select(msg => msg.ToLogMessage()).ToList();
        }
    }
}
