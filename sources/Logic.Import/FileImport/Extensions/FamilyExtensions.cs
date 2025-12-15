using Data.Entities;
using Shared.Models.Import;
using System.Text;

namespace Logic.Import.FileImport.Extensions
{
    internal static class FamilyExtensions
    {
        internal static FamilyEntity ToImportEntity(this FamilyImportModel model)
        {
            var salt = Guid.NewGuid().ToString();

            return new FamilyEntity
            {
                Name = model.Name,
                ContactMailAddress = model.ContactMailAddress,
                IsActive = model.IsActive,
                Users = (from user in model.FamilyMembers
                         select new UserEntity
                         {
                             FirstName = user.FirstName,
                             LastName = user.LastName,
                             Email = user.Email,
                             DateOfBirth = user.DateOfBirth,
                             UserRole = user.UserRole,
                             Credentials = new UserCredentialsEntity
                             {
                                 Salt = salt,
                                 PasswordHash = GetHashedSecret(user.Credentials.Password, salt),
                                 RefreshToken = string.Empty,
                                 ExpiresAt = DateTime.MinValue
                             },
                             Settings = new UserSettingsEntity(),
                             IsActive = true
                         }).ToList()
            };
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
