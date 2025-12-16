using Data.Entities;
using Shared.Models.Administration;
using Shared.Models.Import;
using System.Globalization;

namespace Logic.Administration.Extensions
{
    internal static class FamilyExtensions
    {
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
            if (entity == null)
            {
                return null;
            }

            return new FamilyModel
            {
                FamilyId = entity.Id,
                Name = entity.Name,
                ContactMailAddress = entity.ContactMailAddress,
                IsActive = entity.IsActive,
                LastUpdateAt = entity.UpdatedAt == DateTime.MinValue ?
                    entity.CreatedAt.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture) :
                    entity.UpdatedAt.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                LastUpdateBy = !string.IsNullOrEmpty(entity.UpdatedBy) ?
                    entity.UpdatedBy :
                    entity.CreatedBy
            };
        }
    }
}
