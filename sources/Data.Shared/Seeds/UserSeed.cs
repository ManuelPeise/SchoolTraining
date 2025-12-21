using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace Data.Shared.Seeds
{
    public class UserSeed : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserEntity> builder)
        {
            var timeStamp = new DateTime(2024, 6, 12, 12, 0, 0, DateTimeKind.Utc);
           
            var defaultSystemAdminUser = new UserEntity
            {
                Id = 1,
                IdExternal = "045a9bd4-06f9-4a55-8ab3-2dc1d692706e",
                FirstName = "System",
                LastName = "Admin",
                Username = "System.Admin",
                DateOfBirth = DateTime.Parse("1980-04-20"),
                UserRole = UserRoleEnum.SystemAdmin,
                IsActive = true,
                CredentialsId = 1,
                CreatedAt = timeStamp,
                CreatedBy = "System",
                SettingsId = 1,
            };

            var defaultAdminUser = new UserEntity
            {
                Id = 2,
                IdExternal = "18338480-8150-4f53-9358-d11b0f1fd9ee",
                FirstName = "Family",
                LastName = "Admin",
                Username = "Family.Admin",
                DateOfBirth = DateTime.Parse("1980-04-20"),
                UserRole = UserRoleEnum.LocalAdmin,
                IsActive = true,
                CredentialsId = 2,
                CreatedAt = timeStamp,
                CreatedBy = "System",
                FamilyId = 1,
                SettingsId = 2,
            };

            builder.HasData(defaultSystemAdminUser, defaultAdminUser);
        }
    }
}
