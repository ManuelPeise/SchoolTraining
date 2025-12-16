using Data.Entities;
using Shared.Models.Import;
using System.Text;

namespace Logic.Import.FileImport.Extensions
{
    internal static class FamilyExtensions
    {
        internal static FamilyEntity ToImportEntity(this FamilyModel model)
        {
            var salt = Guid.NewGuid().ToString();
            var currentDate = DateTime.UtcNow;
            return new FamilyEntity
            {
                IdExternal = string.IsNullOrEmpty(model.IdExternal) ? Guid.NewGuid().ToString() : model.IdExternal,
                Name = model.Name,
                ContactMailAddress = model.ContactMailAddress,
                IsActive = model.IsActive,
                CreatedAt = currentDate,
                CreatedBy = "System",
                Users = (from user in model.FamilyMembers
                         select new UserEntity
                         {
                             IdExternal = string.IsNullOrEmpty(user.IdExternal) ? Guid.NewGuid().ToString() : user.IdExternal,
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
                                 ExpiresAt = DateTime.MinValue,
                                 CreatedAt = currentDate,
                                 CreatedBy = "System",
                             },
                             Settings = new UserSettingsEntity(),
                             IsActive = true,
                             CreatedAt = currentDate,
                             CreatedBy = "System",
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
