using Revit.DAL.DataContext;
using Revit.DAL.DataContext.DataInfrastructure;
using Revit.DML;

namespace Revit.DAL.Utils
{
    public static class DataContextExtensions
    {
        public static BaseEntityProxy GetAutomationItemProxyById(this IDataContext dataContext, int automationItemId)
        {
            var automationItemProxies = GetBaseEntityProxies(dataContext);

            return automationItemProxies
                .FirstOrDefault(e => e.Id == automationItemId);
        }

        public static IEnumerable<BaseEntityProxy> GetBaseEntityProxies(
            this IDataContext dataContext,
            Func<BaseEntity, bool> predicate = null)
        {
            var automationItemEntries = dataContext.Foo.Entries
                .Where(x => predicate?.Invoke(x.Entity) ?? true)
                .OfType<IEntityProxy>()
                .Concat(dataContext.Bar.Entries.Where(x => predicate?.Invoke(x.Entity) ?? true));

            return automationItemEntries.Select(x => new BaseEntityProxy(x));
        }
    }
}
