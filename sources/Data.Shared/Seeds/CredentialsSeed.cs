using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text;

namespace Data.Shared.Seeds
{
    public class CredentialsSeed : IEntityTypeConfiguration<UserCredentialsEntity>
    {
        public void Configure(EntityTypeBuilder<UserCredentialsEntity> builder)
        {
            var timeStamp = new DateTime(2024, 6, 12, 12, 0, 0, DateTimeKind.Utc);
            var defaultSystemAdminSalt = new Guid("2694d423-17ba-4ed7-88c7-817f334a10ba").ToString();
            var defaultAdminSalt = new Guid("68a30fe5-8d70-491e-9064-facc46052276").ToString();

            builder.HasData(new List<UserCredentialsEntity>
            {
                new UserCredentialsEntity
                {
                    Id = 1,
                    Salt = defaultSystemAdminSalt,
                    PasswordHash = GetPasswordHash("Pass@word", defaultSystemAdminSalt),
                    CreatedAt = timeStamp,
                    CreatedBy = "System",
                },
                new UserCredentialsEntity
                {
                    Id = 2,
                    Salt = defaultAdminSalt,
                    PasswordHash = GetPasswordHash("Pass@word", defaultAdminSalt),
                    CreatedAt = timeStamp,
                    CreatedBy = "System",
                },
            });
        }

        private static string GetPasswordHash(string password, string salt)
        {
            var bytes = Encoding.UTF8.GetBytes(password).ToList();
            bytes.AddRange(Encoding.UTF8.GetBytes(salt));

            return Convert.ToBase64String(bytes.ToArray());
        }
    }
}
