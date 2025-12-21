using Data.Entities;
using Data.Entities.Administation;
using Data.Entities.Learning;
using System.Linq.Expressions;


namespace Logic.Shared
{
    public static class IncludeExpressions
    {
        public static List<Expression<Func<FamilyEntity, object>>> IncludeFamilyMembers = new List<Expression<Func<FamilyEntity, object>>> { e => e.Users };
        public static List<Expression<Func<ModuleEntity, object>>> IncludeSubModules = new List<Expression<Func<ModuleEntity, object>>> { e => e.SubModules };
        public static List<Expression<Func<SubModuleEntity, object>>> IncludeModule = new List<Expression<Func<SubModuleEntity, object>>> { e => e.Module, e => e.Module.SubModules };
        public static List<Expression<Func<UserEntity, object>>> IncludeRights = new List<Expression<Func<UserEntity, object>>> { e => e.UserRights };
        public static List<Expression<Func<UserRightEntity, object>>> IncludeUserRights = new List<Expression<Func<UserRightEntity, object>>> { e => e.Right };
    }
}
