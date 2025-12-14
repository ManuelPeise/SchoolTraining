using Data.Entities;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Quartz;
using Shared.Enums;

namespace Core.Api.Bundels
{
    public class Scheduler
    {
        public static async Task StartScheduler(IServiceProvider services)
        {
            var schedulerFactory = services.GetRequiredService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler();

            var configuration = services.GetRequiredService<IConfiguration>();
            var apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");

            var currentDateTime = DateTime.UtcNow;

            await AddJob(scheduler, "My Job", "My Job Description",
                new JobDataMap
                {
                    { "Url", $"{apiBaseUrl}/testschedule/test" }
                },
                GetNextInterval(currentDateTime, 47),
                "0 0/15 * * * ?");

            await scheduler.Start();
        }

        private static async Task AddJob(
            IScheduler scheduler,
            string key,
            string description,
            JobDataMap jobDataMap,
            DateTimeOffset start,
            string cronScheduleExpression)
        {
            var jobKey = new JobKey(key);
            var jobDetail = JobBuilder.Create<WebJob>()
                .WithIdentity(jobKey)
                .WithDescription(description)
                .SetJobData(jobDataMap)
                .Build();

            var trigger = GetJobTrigger(key, start, jobKey, cronScheduleExpression);

            // Job und Trigger gemeinsam planen (empfohlen, kein StoreDurably/AddJob nötig)
            await scheduler.ScheduleJob(jobDetail, trigger);
        }

        private static ITrigger GetJobTrigger(
            string key,
            DateTimeOffset start,
            JobKey jobKey,
            string cronScheduleExpression)
        {
#if DEBUG
            return TriggerBuilder.Create()
                .WithIdentity($"{key}Trigger")
                .StartNow()
                .WithSimpleSchedule(builder => builder
                    .WithIntervalInSeconds(60)
                    .RepeatForever())
                .ForJob(jobKey)
                .Build();
#else
            return TriggerBuilder.Create()
                .WithIdentity($"{key}Trigger")
                .StartAt(start)
                .WithCronSchedule(cronScheduleExpression)
                .ForJob(jobKey)
                .Build();
#endif
        }

        public static DateTimeOffset GetNextInterval(DateTimeOffset from, int intervalMinutes)
        {
            if (intervalMinutes <= 0 || intervalMinutes > 60)
                throw new ArgumentOutOfRangeException(nameof(intervalMinutes), "Interval must be between 1 and 60 minutes.");

            int minutes = from.Minute;
            int next = ((minutes / intervalMinutes) * intervalMinutes);
            if (minutes % intervalMinutes != 0)
                next += intervalMinutes;

            if (next >= 60)
            {
                from = from.AddHours(1);
                next = 0;
            }

            var result = new DateTimeOffset(
                from.Year,
                from.Month,
                from.Day,
                from.Hour,
                next,
                0,
                from.Offset
            );

            if (result <= from)
                result = result.AddMinutes(intervalMinutes);

            return result;
        }
    }

    public class WebJob : IJob
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly ILogService _logService;

        public WebJob(ILogService logService)
        {
            _logService = logService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var jobDataMap = context.MergedJobDataMap;
                var url = jobDataMap.GetString("Url") ?? string.Empty;

                if (!string.IsNullOrEmpty(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    var requestMessage = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(url)
                    };
                    requestMessage.Headers.Add("X-Schedule-Job", "true");

                    var response = await _httpClient.SendAsync(requestMessage);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception exception)
            {
                await _logService.LogMessage(new LogMessageEntity
                {
                    Message = $"Error executing web job: {exception.Message}",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception.StackTrace,
                    LogLevel = LogLevelEnum.Error,
                    Module = nameof(WebJob),
                });
            }
        }
    }
}
