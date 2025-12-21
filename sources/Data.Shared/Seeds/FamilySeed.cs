using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Shared.Seeds
{
    public class FamilySeed : IEntityTypeConfiguration<FamilyEntity>
    {
        public void Configure(EntityTypeBuilder<FamilyEntity> builder)
        {
            var timeStamp = new DateTime(2024, 6, 12, 12, 0, 0, DateTimeKind.Utc);

            builder.HasData(new FamilyEntity
            {
                Id = 1,
                Name = "Default Admin Family",
                IsActive = true,
                CreatedAt = timeStamp,
                CreatedBy = "System",
            });
        }
    }
}
