using Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Learning
{
    public class UserModuleEntity: AEntityBase
    {
        /// <summary>
        /// Gets or sets the type of the module for the user.
        /// </summary>
        public ModuleTypeEnum ModuleType { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets the user entity associated with this user module.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier for the module.
        /// </summary>
        public int ModuleId { get; set; }
        /// <summary>
        /// Gets or sets the module entity associated with this user module.
        /// </summary>
        [ForeignKey(nameof(ModuleId))]
        public ModuleEntity Module { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the user module is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
