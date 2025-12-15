using Data.Entities.Learning;
using Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class UserEntity : AEntityBase
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public UserRoleEnum UserRole { get; set; }
        public string? Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated family, if available.
        /// </summary>
        public int? FamilyId { get; set; }
        [ForeignKey(nameof(FamilyId))]
        public FamilyEntity? Family { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the credentials associated with this entity.
        /// </summary>
        public int CredentialsId { get; set; }
        [ForeignKey(nameof(CredentialsId))]
        public UserCredentialsEntity Credentials { get; set; } = new();
        /// <summary>
        /// Gets or sets the unique identifier for the settings configuration.
        /// </summary>
        public int SettingsId { get; set; }
        [ForeignKey(nameof(SettingsId))]
        public UserSettingsEntity Settings { get; set; } = new();

        /// <summary>
        /// Gets or sets the collection of modules associated with the user.
        /// </summary>
        public ICollection<UserModuleEntity> UserModules { get; set; } = new List<UserModuleEntity>();
    }
}
