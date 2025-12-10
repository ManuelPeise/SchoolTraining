using Shared.Enums;

namespace Data.Entities
{
    public class LogMessageEntity: AEntityBase
    {
        public string Message { get; set; } = string.Empty;
        public string ExeptionMessage { get; set; } = string.Empty;
        public string? Module { get; set; }
        public string? StackTrace { get; set; }
        public DateTime TimeStamp { get; set; }
        public LogLevelEnum LogLevel { get; set; }
    }
}
