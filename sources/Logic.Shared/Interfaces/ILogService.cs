using Data.Entities;

namespace Logic.Shared.Interfaces
{
    public interface ILogService
    {
        Task LogMessage(LogMessageEntity entity);
    }
}
