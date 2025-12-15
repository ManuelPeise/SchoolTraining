using Data.Shared;
using Shared.Enums;

namespace Logic.Shared.Interfaces
{
    public interface IDbContextFactory
    {
        ADbContext GetContext(DbContextTypeEnum? contextType = DbContextTypeEnum.MySql);
    }
}
