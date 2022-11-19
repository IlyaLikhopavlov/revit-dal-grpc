using Revit.DAL.DataContext.DataInfrastructure.Enums;

namespace Revit.DAL.DataContext.DataInfrastructure
{
    public interface IEntityProxy
    {
        public int Id { get; }

        public EntityState EntityState { get; set; }
    }
}