using Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Learning
{
    public class SubModuleEntity : AEntityBase
    {
        /// <summary>
        /// Gets or sets the external identifier associated with the entity [data import].
        /// </summary>
        public string IdExternal { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the title of the submodule.
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the description of the submodule.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the vocabulary direction for this submodule, if applicable.
        /// </summary>
        public VocabularyDirectionEnum? VocabularyDirection { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier for the module.
        /// </summary>
        /// 
        public int ModuleId { get; set; }
        /// <summary>
        /// Gets or sets the parent module entity.
        /// </summary>
        [ForeignKey(nameof(ModuleId))]
        public ModuleEntity Module { get; set; }
        /// <summary>
        /// Gets or sets the collection of units associated with the current submodule.
        /// </summary>
        public ICollection<UnitEntity> Units { get; set; } = new List<UnitEntity>();
    }
}
