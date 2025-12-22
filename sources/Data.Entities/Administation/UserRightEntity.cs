using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Administation
{
    public class UserRightEntity : AEntityBase
    {
        public int RightId { get; set; }
        [ForeignKey(nameof(RightId))]
        public RightEntity Right { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }
        public bool IsActive { get; set; }
        public bool Deny { get; set; }
        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}   