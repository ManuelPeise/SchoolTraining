namespace Shared.Administration
{
    public static class Rights
    {
        public static Dictionary<string, Guid> UserRights => _userRights;

        public const string FamilyAdministrationRight = "FamilyAdministration";
        public const string ModuleAdministartionRight = "ModuleAdministration";
        public const string SubModuleAdministrationRight = "SubModuleAdministration";

        private static Dictionary<string, Guid> _userRights = new Dictionary<string, Guid>
        {
            { FamilyAdministrationRight, new Guid("551a0d01-dea8-42d8-9268-89584dd43d27") },
            { ModuleAdministartionRight, new Guid("a8711cdd-3991-4169-afd1-414fb49956a9") },
            { SubModuleAdministrationRight, new Guid("2b24d50e-aa2d-4201-970e-45594138e111") }
        };
    }
}
