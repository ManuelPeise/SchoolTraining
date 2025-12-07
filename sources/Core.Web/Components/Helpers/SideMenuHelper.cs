using Shared.Enums;
using Shared.Models.Ui;

namespace Core.Web.Components.Helpers
{
    internal static class SideMenuHelper
    {
        internal static List<SideMenuEntry> GetSideMenuEntries(string userRole)
        {
            if(!Enum.TryParse<UserRoleEnum>(userRole, out var userRoleEnum))
            {
                return new List<SideMenuEntry>();
            }

            return new SideMenuEntry[]
            {
                new SideMenuEntry
                {
                    Label = Resx.Core.LabelFamilyAndUserAdministration,
                    Route = "/administration/family-and-user-administration",
                    Icon = "bi bi-people-fill",
                    AllowedUserRoles = new List<UserRoleEnum>
                    {
                        UserRoleEnum.Admin,
                        UserRoleEnum.SystemAdmin,
                    }
                },
            }
            .Where(item => item.AllowedUserRoles.Contains(userRoleEnum))
            .OrderBy(x => x.Label)
            .ToList();
        }
    }
}
