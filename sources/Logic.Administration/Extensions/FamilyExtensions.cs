using Data.Entities;
using Shared.Models.Administration;
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

        internal static List<FamilyModel> ToFamilyList(this List<FamilyEntity> entities)
        {
            var families = new List<FamilyModel>();
            entities.ForEach(entity =>
            {
                var family = entity.ToFamily();
                if (family != null)
                {
                    families.Add(family);
                }
            });
            return families;
        }

        internal static FamilyModel? ToFamily(this FamilyEntity entity)
        {
            if(entity == null)
            {
                return null;
            }

            return new FamilyModel
            {
                FamilyId = entity.Id,
                Name = entity.Name,
                ContactMailAddress = entity.ContactMailAddress,
                IsActive = entity.IsActive,
                Members = entity.Users.ToFamilyMemberList()
            };
        }

        internal static FamilyMemberModel? ToFamilyMember(this UserEntity entity)
        {
            if(entity == null)
            {
                return null;
            }

            return new FamilyMemberModel
            {
                UserId = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                UserName = entity.Username,
                Email = entity.Email,
                DateOfBirth = entity.DateOfBirth,
                UserRole = entity.UserRole,
                IsActive = entity.IsActive
            };
        }

        internal static List<FamilyMemberModel> ToFamilyMemberList(this List<UserEntity> entities)
        {
            var members = new List<FamilyMemberModel>();

            entities.ForEach(entity =>
            {
                var member = entity.ToFamilyMember();

                if (member != null)
                {
                    members.Add(member);
                }
            });

            return members;
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
