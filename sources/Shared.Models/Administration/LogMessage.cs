using Shared.Enums;

namespace Shared.Models.Administration
{
    public class LogMessage
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ExeptionMessage { get; set; } = string.Empty;
        public string? Module { get; set; }
        public string? StackTrace { get; set; }
        public string TimeStamp { get; set; } = string.Empty;
        public LogLevelEnum LogLevel { get; set; }
    }
}
