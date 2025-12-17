namespace Data.Entities
{
    public class FamilyEntity : AEntityBase
    {
        /// <summary>
        /// Gets or sets the external identifier associated with the entity [data import only].
        /// </summary>
        public string IdExternal { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the contact email address associated with this entity.
        /// </summary>
        public string ContactMailAddress { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets a value indicating whether the object is currently active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the collection of user entities associated with this instance.
        /// </summary>
        public List<UserEntity> Users { get; set; }
    }
}
