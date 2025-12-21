using Logic.Administration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Administration;

namespace Service.Api.Administration
{
    public class UserRightController : ApiControllerBase
    {
        private readonly IUserRightService _userRightService;

        public UserRightController(IUserRightService userRightService)
        {
            _userRightService = userRightService;
        }

        [HttpGet(Name = "GetUserRights")]
        [JwtAuth(AllowSystemAdmin = true, AllowAdmin = true)]
        public async Task<List<UserRight>> GetUserRights([FromQuery]int userId)
        {
            return await _userRightService.GetUserRights(userId);
        }
    }
}
