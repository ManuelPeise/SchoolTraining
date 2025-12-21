using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Shared.Seeds
{
    public class SettingsSeed : IEntityTypeConfiguration<UserSettingsEntity>
    {
        public void Configure(EntityTypeBuilder<UserSettingsEntity> builder)
        {
            var timeStamp = new DateTime(2024, 6, 12, 12, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new UserSettingsEntity
                {
                    Id = 1,
                    CreatedAt = timeStamp,
                    CreatedBy = "System"
                },
                new UserSettingsEntity
                {
                    Id = 2,
                    CreatedAt = timeStamp,
                    CreatedBy = "System"
                }
            );
        }
    }
}
