using Shared.Models.Administration;

namespace Logic.Administration.Interfaces
{
    public interface IUserRightService
    {
       Task<List<UserRight>> GetUserRights(int userId);
    }
}
