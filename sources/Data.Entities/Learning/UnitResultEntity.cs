using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Learning
{
    public class UnitResultEntity : AEntityBase
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; } = new();

        /// <summary>
        /// Gets or sets the unique identifier for the unit.
        /// </summary>
        public int UnitId { get; set; }
        [ForeignKey(nameof(UnitId))]
        public UnitEntity Unit { get; set; } = new();

        /// <summary>
        /// Gets or sets the score or result for the unit.
        /// </summary>
        public double? Score { get; set; }

        /// <summary>
        /// Gets or sets whether the unit is completed.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the unit was completed.
        /// </summary>
        public DateTime? CompletedAt { get; set; }
    }
}
