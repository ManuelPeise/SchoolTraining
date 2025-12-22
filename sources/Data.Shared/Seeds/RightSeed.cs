using Data.Entities.Administation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Administration;

namespace Data.Shared.Seeds
{
    public class RightSeed : IEntityTypeConfiguration<RightEntity>
    {
        public void Configure(EntityTypeBuilder<RightEntity> builder)
        {
            var timeStamp = new DateTime(2024, 6, 12, 12, 0, 0, DateTimeKind.Utc);

            builder.HasData(new List<RightEntity>
            {
                new RightEntity
                {
                    Id = 1,
                    RightGuid = Rights.UserRights[Rights.FamilyAdministrationRight],
                    Name = Rights.FamilyAdministrationRight,
                    NameResourceKey = $"common.{Rights.FamilyAdministrationRight}",
                    DescriptionResourceKey = $"common.{Rights.FamilyAdministrationRight}Description",
                    IsActive = true,
                    CreatedAt = timeStamp,
                    CreatedBy = "System"
                },
                new RightEntity
                {
                    Id = 2,
                    RightGuid = Rights.UserRights[Rights.ModuleAdministrationRight],
                    Name = Rights.ModuleAdministrationRight,
                    NameResourceKey = $"common.{Rights.ModuleAdministrationRight}",
                    DescriptionResourceKey = $"common.{Rights.ModuleAdministrationRight}Description",
                    IsActive = true,
                    CreatedAt = timeStamp,
                    CreatedBy = "System"
                },
                new RightEntity
                {
                    Id = 3,
                    RightGuid = Rights.UserRights[Rights.SubModuleAdministrationRight],
                    Name = Rights.SubModuleAdministrationRight,
                    NameResourceKey = $"common.{Rights.SubModuleAdministrationRight}",
                    DescriptionResourceKey = $"common.{Rights.SubModuleAdministrationRight}Description",
                    IsActive = true,
                    CreatedAt = timeStamp,
                    CreatedBy = "System"
                }
            });
        }
    }
}
