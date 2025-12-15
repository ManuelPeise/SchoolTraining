using Data.Entities;
using System.Linq.Expressions;


namespace Logic.Shared
{
    public static class IncludeExpressions
    {
        public static List<Expression<Func<FamilyEntity, object>>> IncludeFamilyMembers = new List<Expression<Func<FamilyEntity, object>>> { e => e.Users };
    }
}
