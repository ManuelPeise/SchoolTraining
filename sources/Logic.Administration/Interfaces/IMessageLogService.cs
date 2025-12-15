using Shared.Enums;
using Shared.Models;
using Shared.Models.Administration;

namespace Logic.Administration.Interfaces
{
    public interface IMessageLogService
    {
        Task<List<LogMessage>> GetMessageLogs();
        Task<NotificationDataResponse<List<LogMessage>>> DeleteLogMessage(int id);
        Task<NotificationDataResponse<List<LogMessage>>> DeleteLogMessages(LogLevelEnum? logLevel);
    }
}
