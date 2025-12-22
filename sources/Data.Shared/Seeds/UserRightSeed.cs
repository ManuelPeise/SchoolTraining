using Data.Entities.Administation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Shared.Seeds
{
    public class UserRightSeed : IEntityTypeConfiguration<UserRightEntity>
    {
        public void Configure(EntityTypeBuilder<UserRightEntity> builder)
        {
            var timeStamp = new DateTime(2024, 6, 12, 12, 0, 0, DateTimeKind.Utc);

            builder.HasData(new List<UserRightEntity>
            {
                new UserRightEntity
                {
                    Id = 1,
                    UserId = 1,
                    RightId = 1,
                    IsActive = true,
                    Deny = false,
                    CanView = true,
                    CanCreate = true,
                    CanEdit = true,
                    CanDelete = true,
                    CreatedAt = timeStamp,
                    CreatedBy = "System"
                },
                new UserRightEntity
                {
                    Id = 2,
                    UserId = 1,
                    RightId = 2,
                    IsActive = true,
                    Deny = false,
                    CanView = true,
                    CanCreate = false,
                    CanEdit = false,
                    CanDelete = false,
                    CreatedAt = timeStamp,
                    CreatedBy = "System"
                },
                new UserRightEntity
                {
                    Id = 3,
                    UserId = 1,
                    RightId = 3,
                    IsActive = true,
                    Deny = false,
                    CanView = true,
                    CanCreate = false,
                    CanEdit = false,
                    CanDelete = false,
                    CreatedAt = timeStamp,
                    CreatedBy = "System"
                },
                new UserRightEntity
                {
                    Id = 4,
                    UserId = 2,
                    RightId = 1,
                    IsActive = true,
                    Deny = true,
                    CanView = false,
                    CanCreate = false,
                    CanEdit = false,
                    CanDelete = false,
                    CreatedAt = timeStamp,
                    CreatedBy = "System"
                },
                new UserRightEntity
                {
                    Id = 5,
                    UserId = 2,
                    RightId = 2,
                    IsActive = true,
                    Deny = false,
                    CanView = true,
                    CanCreate = true,
                    CanEdit = true,
                    CanDelete = true,
                    CreatedAt = timeStamp,
                    CreatedBy = "System"
                },
                new UserRightEntity
                {
                    Id = 6,
                    UserId = 2,
                    RightId = 3,
                    IsActive = true,
                    Deny = false,
                    CanView = true,
                    CanCreate = true,
                    CanEdit = true,
                    CanDelete = true,
                    CreatedAt = timeStamp,
                    CreatedBy = "System"
                }
            });
        }
    }
}
