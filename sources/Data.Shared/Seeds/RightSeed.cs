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
                    CreatedAt = timeStamp,
                    CreatedBy = "System"
                },
                new RightEntity
                {
                    Id = 2,
                    RightGuid = Rights.UserRights[Rights.ModuleAdministartionRight],
                    Name = Rights.ModuleAdministartionRight,
                    NameResourceKey = $"common.{Rights.ModuleAdministartionRight}",
                    DescriptionResourceKey = $"common.{Rights.ModuleAdministartionRight}Description",
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
                    CreatedAt = timeStamp,
                    CreatedBy = "System"
                }
            });
        }
    }
}
