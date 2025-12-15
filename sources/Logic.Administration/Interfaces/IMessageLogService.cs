using Shared.Enums;
using Shared.Models.Administration;

namespace Logic.Administration.Interfaces
{
    public interface IMessageLogService
    {
        Task<List<LogMessage>> GetMessageLogs();
        Task<List<LogMessage>> DeleteLogMessage(int id);
        Task<List<LogMessage>> DeleteLogMessages(LogLevelEnum? logLevel);
    }
}
