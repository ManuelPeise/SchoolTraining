using Microsoft.AspNetCore.Mvc;
using Service.Api.Service.Api.Scheduler;

namespace Service.Api.Scheduler
{
    [SchedulerAuthorize]
    public class TestScheduleController: ApiControllerBase
    {
        public TestScheduleController()
        {
            
        }

    
        [HttpPost(Name = "Test")]
        public async Task<IActionResult> Test()
        {
            return new OkResult();
        }
    }
}
