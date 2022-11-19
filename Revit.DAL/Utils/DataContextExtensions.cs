using Revit.DAL.DataContext;
using Revit.DAL.DataContext.DataInfrastructure;
using Revit.DML;

namespace Revit.DAL.Utils
{
    public static class DataContextExtensions
    {
        public static AutomationItemProxy GetAutomationItemProxyById(this IDataContext dataContext, int automationItemId)
        {
            var automationItemProxies = GetAutomationItemProxies(dataContext);

            return automationItemProxies
                .FirstOrDefault(e => e.Id == automationItemId);
        }

        public static IEnumerable<AutomationItemProxy> GetAutomationItemProxies(
            this IDataContext dataContext,
            Func<BaseEntity, bool> predicate = null)
        {
            var automationItemEntries = dataContext.Foo.Entries
                .Where(x => predicate?.Invoke(x.Entity) ?? true)
                .OfType<IEntityProxy>()
                .Concat(dataContext.Foo.Entries.Where(x => predicate?.Invoke(x.Entity) ?? true))
                .Concat(dataContext.Bar.Entries.Where(x => predicate?.Invoke(x.Entity) ?? true));

            return automationItemEntries.Select(x => new AutomationItemProxy(x));
        }
    }
}
