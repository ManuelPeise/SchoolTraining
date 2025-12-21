using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Learning
{
    public class ModuleEntity : AEntityBase
    {
        /// <summary>
        /// Gets or sets the external identifier associated with the entity [data import only].
        /// </summary>
        public string IdExternal { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the title of the module.
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the description of the module.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the unique identifier for the family.
        /// </summary>
        public int? FamilyId { get; set; }
        [ForeignKey(nameof(FamilyId))]
        public FamilyEntity? Family { get; set; }
        /// <summary>
        /// Gets or sets the collection of submodules associated with the module.
        /// </summary>
        public ICollection<SubModuleEntity> SubModules { get; set; } = new List<SubModuleEntity>();
       
    }
}
