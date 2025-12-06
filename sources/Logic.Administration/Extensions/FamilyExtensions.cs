using Data.Entities;
using Shared.Models.Import;
using System.Text;

namespace Logic.Administration.Extensions
{
    internal static class FamilyExtensions
    {
        internal static FamilyEntity? ToImportEntity(this FamilyImportModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new FamilyEntity
            {
                Name = model.Name,
                ContactMailAddress = model.ContactMailAddress,
                IsActive = model.IsActive,
                Users =  model.FamilyMembers.ToImportUserEntities()
            };
        }

        internal static List<UserEntity> ToImportUserEntities(this List<FamilyMember> members)
        {
            var users = new List<UserEntity>();
            
            if (members == null)
            {
                return users;
            }

            foreach (var member in members)
            {
                var salt = Guid.NewGuid().ToString();

                users.Add(new UserEntity
                {
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Email = member.Email,
                    DateOfBirth = member.DateOfBirth,
                    IsActive = true,
                    UserRole = member.UserRole,
                    Credentials = new UserCredentialsEntity
                    {
                        Salt = salt,
                        PasswordHash = GetHashedSecret(member.Credentials.Password, salt),
                        RefreshToken = string.Empty,
                        ExpiresAt = DateTime.MinValue
                    },
                    Settings = new UserSettingsEntity()
                }); 
            }
            return users;
        }

        private static string GetHashedSecret(string? password, string salt)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException(nameof(password));
            }

            var passwordBytes = Encoding.UTF8.GetBytes(password).ToList();
            passwordBytes.AddRange(Encoding.UTF8.GetBytes(salt));

            return Convert.ToBase64String(passwordBytes.ToArray());
        }
    }
}
