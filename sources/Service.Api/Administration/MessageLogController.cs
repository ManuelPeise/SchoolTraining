using Logic.Administration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Api.Service.Api.Scheduler;
using Shared.Enums;
using Shared.Models;
using Shared.Models.Administration;

namespace Service.Api.Administration
{
    public class MessageLogController:ApiControllerBase
    {
        private readonly IMessageLogService _messageLogService;
        public MessageLogController(IMessageLogService messageLogService)
        {
            _messageLogService = messageLogService;
        }

        [HttpGet(Name = "GetMessageLogs")]
        [JwtAuth(AllowAdmin = true, AllowSystemAdmin = true)]
        public async Task<List<LogMessage>> GetMessageLogs()
        {
            return await _messageLogService.GetMessageLogs();
        }

        [HttpPost(Name = "DeleteMessage")]
        [JwtAuth(AllowAdmin = true, AllowSystemAdmin = true)]
        public async Task<NotificationDataResponse<List<LogMessage>>> DeleteMessage([FromQuery]int messageId)
        {
            return await _messageLogService.DeleteLogMessage(messageId);

        }

        [HttpPost(Name = "CleanupLogMessages")]
        [JwtAuth(AllowAdmin = true, AllowSystemAdmin = true)]
        public async Task<NotificationDataResponse<List<LogMessage>>> CleanupLogMessages([FromQuery] LogLevelEnum? logLevel)
        {
            return await _messageLogService.DeleteLogMessages(logLevel);
        }

        /// <summary>
        /// Called by scheduler to cleanup message log
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "DeleteLogMessages")]
        [SchedulerAuthorize]
        public async Task DeleteLogMessages()
        {
            await _messageLogService.DeleteLogMessages(null);
        }
    }
}
