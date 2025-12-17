using Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Learning
{
    public class UnitEntity: AEntityBase
    {
        /// <summary>
        /// Gets or sets the external identifier associated with the entity [data import only].
        /// </summary>
        public string IdExternal { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the name of the unit associated with this instance.
        /// </summary>
        public string UnitName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the description of the unit associated with this entity.
        /// </summary>
        public string UnitDescription { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the sort order of the unit within its parent submodule.
        /// </summary>
        public int SortOrder { get; set; }
        /// <summary>
        /// Gets or sets the type of the unit.
        /// </summary>
        public UnitTypeEnum UnitType { get; set; }
        /// <summary>
        /// Should be a json array of objects depending on the UnitType.
        /// </summary>
        public string UnitContentJson { get; set; } = string.Empty; 
        /// <summary>
        /// Gets or sets the unique identifier for the parent submodule.
        /// </summary>
        public int SubModuleId { get; set; }
        [ForeignKey("SubModuleId")]
        public SubModuleEntity SubModule { get; set; }
    }
}
