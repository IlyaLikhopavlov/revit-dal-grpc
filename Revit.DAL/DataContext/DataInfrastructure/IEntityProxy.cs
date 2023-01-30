using App.DAL.Revit.DataContext.DataInfrastructure.Enums;

namespace App.DAL.Revit.DataContext.DataInfrastructure
{
    public interface IEntityProxy
    {
        public int Id { get; }

        public EntityState EntityState { get; set; }
    }
}