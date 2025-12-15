using Data.Entities;
using Shared.Models.Administration;

namespace Logic.Administration.Extensions
{
    internal static class LogMessageExtensions
    {
        internal static LogMessage ToLogMessage(this LogMessageEntity entity)
        {
            return new LogMessage
            {
                Id = entity.Id,
                Message =  entity.Message,
                ExeptionMessage = entity.ExeptionMessage,
                StackTrace = entity.StackTrace,
                Module = entity.Module,
                TimeStamp = entity.TimeStamp.ToString("dd.MM.yyyy HH:mm"),
                LogLevel = entity.LogLevel
            };
        }
    }
}
