namespace Data.Entities
{
    public class FamilyEntity: AEntityBase
    {
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        // One Family can have multiple Users
        public List<UserEntity> Users { get; set; } = new();
    }
}
